using _Project.Sources.Game.Components;
using _Project.Sources.Game.Rpcs;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace _Project.Sources.Game.Systems.Server
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct GoInGameServerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EntitiesReferences>();
            state.RequireForUpdate<NetworkId>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

            var entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();
            
            foreach ((RefRO<ReceiveRpcCommandRequest> receiveRpcCommandRequest, Entity entity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>().WithAll<GoInGameRequestRpc>().WithEntityAccess())
            {
                entityCommandBuffer.AddComponent<NetworkStreamInGame>(entity);

                entityCommandBuffer.AddComponent<NetworkStreamInGame>(receiveRpcCommandRequest.ValueRO.SourceConnection);
                entityCommandBuffer.DestroyEntity(entity);


                var playerEntity = entityCommandBuffer.Instantiate(entitiesReferences.PlayerPrefab);
                entityCommandBuffer.AddComponent(playerEntity, LocalTransform.FromPosition(new float3(
                    UnityEngine.Random.Range(-5f, 5f), 1, UnityEngine.Random.Range(-5f, 5f))));
                var networkId = SystemAPI.GetComponent<NetworkId>(receiveRpcCommandRequest.ValueRO.SourceConnection);
                entityCommandBuffer.AddComponent(playerEntity, new GhostOwner
                {
                    NetworkId = networkId.Value
                });
                
                entityCommandBuffer.AppendToBuffer(receiveRpcCommandRequest.ValueRO.SourceConnection, new LinkedEntityGroup
                {
                    Value = playerEntity
                });
            }
            
            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}
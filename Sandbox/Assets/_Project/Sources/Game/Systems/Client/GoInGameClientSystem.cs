using _Project.Sources.Game.Rpcs;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace _Project.Sources.Game.Systems.Client
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    public partial struct GoInGameClientSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            
            foreach ((RefRO<NetworkId> networkId, Entity entity) in SystemAPI.Query<RefRO<NetworkId>>().WithNone<NetworkStreamInGame>().WithEntityAccess())
            {
                entityCommandBuffer.AddComponent<NetworkStreamInGame>(entity);
                
                var rpcEntity = entityCommandBuffer.CreateEntity();
                entityCommandBuffer.AddComponent(rpcEntity, new GoInGameRequestRpc());
                entityCommandBuffer.AddComponent(rpcEntity, new SendRpcCommandRequest());
            }
            
            entityCommandBuffer.Playback(state.EntityManager);
        }
        
    }
}
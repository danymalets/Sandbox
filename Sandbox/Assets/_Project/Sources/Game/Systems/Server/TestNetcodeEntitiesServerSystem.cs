using _Project.Sources.Game.Rpcs;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace _Project.Sources.Game.Systems.Server
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct TestNetcodeEntitiesServerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }
        
        //[BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            foreach ((RefRO<SimpleRpc> simpleRpc, RefRO<ReceiveRpcCommandRequest> receiveRpcCommandRequest, Entity entity) 
                     in SystemAPI.Query<RefRO<SimpleRpc>, RefRO<ReceiveRpcCommandRequest>>().WithEntityAccess())
            {
                Debug.Log($"receive{simpleRpc.ValueRO.Value} {receiveRpcCommandRequest.ValueRO.SourceConnection.ToString()}");
                entityCommandBuffer.DestroyEntity(entity);
            }
            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}
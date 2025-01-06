using _Project.Sources.Game.Rpcs;
using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace _Project.Sources.Game.Systems.Client
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    public partial struct TestNetcodeEntitiesClientSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Entity rpcEntity = state.EntityManager.CreateEntity();
                state.EntityManager.AddComponentData(rpcEntity, new SimpleRpc
                {
                    Value = 56,
                });
                state.EntityManager.AddComponentData(rpcEntity, new SendRpcCommandRequest());
            }
        }
    }
}
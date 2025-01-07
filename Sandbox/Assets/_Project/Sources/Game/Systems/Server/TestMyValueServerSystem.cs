using _Project.Sources.Game.Components;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace _Project.Sources.Game.Systems.Server
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct TestMyValueServerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                Debug.Log($"key");
                
                foreach ((RefRW<MyValue> myValue, Entity entity)
                     in SystemAPI.Query<RefRW<MyValue>>().WithEntityAccess())
                {
                
                    myValue.ValueRW.Value = Random.Range(100, 1000);
                    Debug.Log($"set server {myValue.ValueRW.Value}");
                }
            }
        }
    }
}
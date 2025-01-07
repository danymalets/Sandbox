using _Project.Sources.Game.Components;
using _Project.Sources.Game.Rpcs;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;

namespace _Project.Sources.Game.Systems
{
    public partial struct TestNetcodeEntitiesServerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach ((RefRO<MyValue> myValue, Entity entity)
                     in SystemAPI.Query<RefRO<MyValue>>().WithEntityAccess())
            {
                UnityEngine.Debug.Log($"my val {myValue.ValueRO.Value} {entity.ToString()} {state.World.ToString()}");
            }
        }
    }
}
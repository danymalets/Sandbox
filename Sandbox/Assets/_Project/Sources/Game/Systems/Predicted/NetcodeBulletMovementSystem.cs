using _Project.Sources.Game.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

namespace _Project.Sources.Game.Systems.Predicted
{
    [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
    public partial struct NetcodeBulletMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            const float speed = 20f;
            foreach (RefRW<LocalTransform> localTransform in SystemAPI.Query<RefRW<LocalTransform>>()
                         .WithAll<Bullet, Simulate>())
            {
                localTransform.ValueRW.Position += math.forward() * speed * SystemAPI.Time.DeltaTime;
            }
        }
    }
}
using _Project.Sources.Game.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace _Project.Sources.Game.Systems.Server
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct NetcodeBulletLifetimeSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }
    
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
    
            foreach ((RefRW<Bullet> bullet, Entity entity) in SystemAPI.Query<RefRW<Bullet>>()
                         .WithAll<Simulate>().WithEntityAccess())
            {
                bullet.ValueRW.Timer -= SystemAPI.Time.DeltaTime;
                
                if (bullet.ValueRW.Timer <= 0)
                {
                    entityCommandBuffer.DestroyEntity(entity);
                }
            }
            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}
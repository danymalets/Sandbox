using _Project.Sources.Game.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace _Project.Sources.Game.Systems.Predicted
{
    [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
    public partial struct NetcodePlayerShootSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EntitiesReferences>();
            state.RequireForUpdate<NetworkTime>();
            state.RequireForUpdate<NetworkStreamInGame>();
            state.RequireForUpdate<NetcodePlayerInput>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var networkTime = SystemAPI.GetSingleton<NetworkTime>();
            var entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();

            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);


            foreach ((RefRO<NetcodePlayerInput> netcodePlayerInput,
                         RefRO<LocalTransform> localTransform,
                         RefRO<GhostOwner> ghostOwner) in SystemAPI
                         .Query<RefRO<NetcodePlayerInput>, RefRO<LocalTransform>, RefRO<GhostOwner>>()
                         .WithAll<Simulate>())
            {
                if (networkTime.IsFirstTimeFullyPredictingTick)
                {
                    if (netcodePlayerInput.ValueRO.ShootEvent.IsSet)
                    {

                        var bullet = entityCommandBuffer.Instantiate(entitiesReferences.BulletPrefab);
                        entityCommandBuffer.SetComponent(bullet,
                            LocalTransform.FromPosition(localTransform.ValueRO.Position));
                        entityCommandBuffer.SetComponent(bullet,
                            new GhostOwner { NetworkId = ghostOwner.ValueRO.NetworkId });
                    }
                }
            }


            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}
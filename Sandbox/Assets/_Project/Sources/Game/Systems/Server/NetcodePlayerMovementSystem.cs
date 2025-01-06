using _Project.Sources.Game.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace _Project.Sources.Game.Systems.Server
{
    [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
    public partial struct NetcodePlayerMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<NetworkStreamInGame>();
            state.RequireForUpdate<NetcodePlayerInput>();
        }
        
        // [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach ((RefRO<NetcodePlayerInput> netcodePlayerInput, RefRW<LocalTransform> localTransform) in SystemAPI.Query<RefRO<NetcodePlayerInput>, RefRW<LocalTransform>>().WithAll<GhostOwnerIsLocal>())
            {
                const float moveSpeed = 8f;

                var inputDirection = netcodePlayerInput.ValueRO.MoveDirection;

                float3 moveVector = new float3(inputDirection.x, 0, inputDirection.y) * moveSpeed * SystemAPI.Time.DeltaTime;

                localTransform.ValueRW.Position += moveVector;
            }
        }
        
    }
}
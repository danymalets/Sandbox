using _Project.Sources.Game.Components;
using _Project.Sources.Game.Rpcs;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using UnityEngine;

namespace _Project.Sources.Game.Systems.Client
{
    [UpdateInGroup(typeof(GhostInputSystemGroup))]
    public partial struct NetcodePlayerInputSystem : ISystem
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
            foreach (RefRW<NetcodePlayerInput> netcodePlayerInput in SystemAPI.Query<RefRW<NetcodePlayerInput>>().WithAll<GhostOwnerIsLocal>())
            {
                netcodePlayerInput.ValueRW.MoveDirection = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }
        }
        
    }
}
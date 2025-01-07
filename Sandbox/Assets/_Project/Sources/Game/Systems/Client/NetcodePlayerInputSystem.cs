using _Project.Sources.Game.Components;
using _Project.Sources.Game.Rpcs;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

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
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach ((RefRW<NetcodePlayerInput> netcodePlayerInput, RefRW<MyValue> myValue) in SystemAPI.Query<RefRW<NetcodePlayerInput>, RefRW<MyValue>>().WithAll<GhostOwnerIsLocal>())
            {
                netcodePlayerInput.ValueRW.MoveDirection = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    netcodePlayerInput.ValueRW.ShootEvent.Set();
                }
                else
                {
                    netcodePlayerInput.ValueRW.ShootEvent = default;
                }
            }
        }
        
    }
}
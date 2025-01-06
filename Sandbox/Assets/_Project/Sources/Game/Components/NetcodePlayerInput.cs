using Unity.Mathematics;
using Unity.NetCode;

namespace _Project.Sources.Game.Components
{
    public struct NetcodePlayerInput : IInputComponentData
    {
        public float2 MoveDirection;
    }
}
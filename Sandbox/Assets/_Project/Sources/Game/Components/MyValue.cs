using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

namespace _Project.Sources.Game.Components
{
    public struct MyValue : IComponentData
    {
        [GhostField] public int Value;
    }
}
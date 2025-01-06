using Unity.NetCode;

namespace _Project.Sources.Game.Rpcs
{
    public struct SimpleRpc : IRpcCommand
    {
        public int Value;
    }
}
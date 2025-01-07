using _Project.Sources.Game.Components;
using Unity.Entities;
using UnityEngine;

namespace _Project.Sources.Game.Authorings
{
    public class NetcodePlayerInputAuthoring : MonoBehaviour
    {
        public class Baker : Baker<NetcodePlayerInputAuthoring>
        {
            public override void Bake(NetcodePlayerInputAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new NetcodePlayerInput());
            }
        }
    }
}
using _Project.Sources.Game.Components;
using Unity.Entities;
using UnityEngine;

namespace _Project.Sources.Game.Authorings
{
    public class MyValueAuthoring : MonoBehaviour
    {
        public class Baker : Baker<MyValueAuthoring>
        {
            public override void Bake(MyValueAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new MyValue());
            }
        }
    }
}
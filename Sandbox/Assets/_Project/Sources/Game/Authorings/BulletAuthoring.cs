using _Project.Sources.Game.Components;
using Unity.Entities;
using UnityEngine;

namespace _Project.Sources.Game.Authorings
{
    public class BulletAuthoring : MonoBehaviour
    {
        public class Baker : Baker<BulletAuthoring>
        {
            public override void Bake(BulletAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Bullet()
                {
                    Timer = 1f,
                });
            }
        }
    }
}
using Unity.Entities;

namespace _Project.Sources.Game.Components
{
    public struct EntitiesReferences : IComponentData
    {
        public Entity PlayerPrefab;
        public Entity BulletPrefab;
    }
}
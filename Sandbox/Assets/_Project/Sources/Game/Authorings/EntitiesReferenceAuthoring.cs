using _Project.Sources.Game.Components;
using Unity.Entities;
using UnityEngine;

namespace _Project.Sources.Game.Authorings
{
    public class EntitiesReferenceAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        
        public class PlayerAuthoringBaker : Baker<EntitiesReferenceAuthoring>
        {
            public override void Bake(EntitiesReferenceAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new EntitiesReferences()
                {
                    PlayerPrefab = GetEntity(authoring._playerPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}
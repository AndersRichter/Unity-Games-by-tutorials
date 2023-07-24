using System;
using UnityEngine;

namespace Model.Definitions
{
    [CreateAssetMenu(menuName = "Definitions/ThrowableItems", fileName = "ThrowableItems")]
    public class ThrowableItemsDefinition : ScriptableObject
    {
        [SerializeField] private ThrowableItemDefinition[] _items;

        public ThrowableItemDefinition GetFirstOrDefault(string id)
        {
            foreach (var item in _items)
            {
                if (item.Id == id)
                    return item;
            }

            return default;
        }
    }
    
    [Serializable]
    public struct ThrowableItemDefinition
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private GameObject _projectile;

        public string Id => _id;
        public GameObject Projectile => _projectile;

        public bool IsVoid => string.IsNullOrEmpty(_id);
    }
}
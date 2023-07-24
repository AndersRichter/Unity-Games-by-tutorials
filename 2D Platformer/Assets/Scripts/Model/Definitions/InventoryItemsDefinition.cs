using System;
using System.Linq;
using UnityEngine;

// The idea of Inventory is to divide definitions of stored objects and stored objects
// Here we define InventoryItemsDefinition class as ScriptableObject - so we can create it as a resource in Unity inspector
// It will contain ALL objects definitions, that can be stored in inventory
namespace Model.Definitions
{
    [CreateAssetMenu(menuName = "Definitions/InventoryItems", fileName = "InventoryItems")]
    public class InventoryItemsDefinition : ScriptableObject
    {
        [SerializeField] private ItemDefinition[] _items;

        public ItemDefinition GetFirstOrDefault(string id)
        {
            foreach (var item in _items)
            {
                if (item.Id == id)
                    return item;
            }

            return default;
        }
        
#if UNITY_EDITOR
        // open items only in unity editor for hints in dropdown
        public ItemDefinition[] ItemsDefinitionsForEditor => _items;
#endif
    }

    [Serializable]
    public struct ItemDefinition
    {
        [SerializeField] private string _id;
        [SerializeField] private int _maxInStack;
        [SerializeField] private Sprite _icon;
        [SerializeField] private ItemTag[] _tags;

        public string Id => _id;
        public int MaxInStack => _maxInStack < 0 ? int.MaxValue : _maxInStack;
        public Sprite Icon => _icon;

        public bool IsVoid => string.IsNullOrEmpty(_id);

        public bool HasTag(ItemTag tag)
        {
            return _tags.Contains(tag);
        }
    }

    public enum ItemTag
    {
        Usable,
        Throwable,
    }
}
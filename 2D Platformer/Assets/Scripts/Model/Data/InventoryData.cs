using System;
using System.Collections.Generic;
using Model.Definitions;
using UnityEngine;

// The idea of Inventory is to divide definitions of stored objects and stored objects
// Here we define InventoryData - the real store for inventory objects.
// We always should check objects definitions before any actions with store
namespace Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> _inventoryItems = new();

        public delegate void OnInventoryUpdated(string id, int value);

        // event field to subscribe for inventory updates
        public OnInventoryUpdated OnUpdated;

        public void Add(string id, int value)
        {
            if (value <= 0) return;
            if (!DefinitionsFacade.IsItemDefinitionExists(id)) return;

            var item = GetIncompleteStackItem(id);
            var valuesToAdd = value; // how many values we still need to add
            var maxInStack = DefinitionsFacade.GetMaxInStack(id);

            // first part - fill incomplete stack of items
            // add full value if it possible, or until max in stack value
            if (item != null)
            {
                var valueToFillUntilMaxInStack = maxInStack - item.Value;
                var actualValueToAdd = Mathf.Min(valueToFillUntilMaxInStack, value);
                item.Value += actualValueToAdd;
                valuesToAdd -= actualValueToAdd;
            }

            // second part - create new items until we add all value
            while (valuesToAdd > 0)
            {
                var actualValueToAdd = Mathf.Min(maxInStack, valuesToAdd);
                item = new InventoryItemData(id, actualValueToAdd);
                _inventoryItems.Add(item);
                valuesToAdd -= actualValueToAdd;
            }

            OnUpdated?.Invoke(id, GetTotalAmount(id));
        }

        public void Remove(string id, int value)
        {
            if (value <= 0) return;
            if (!DefinitionsFacade.IsItemDefinitionExists(id)) return;
            
            var item = GetIncompleteStackItem(id);
            var valuesToRemove = value; // how many values we still need to remove
            if (valuesToRemove > GetTotalAmount(id))
            {
                valuesToRemove = GetTotalAmount(id);
            }
            var maxInStack = DefinitionsFacade.GetMaxInStack(id);
            
            // first part - remove incomplete stack of items
            // remove full value if it possible, or until empty stack
            if (item != null)
            {
                var valueToRemoveUntilZero = item.Value;
                var actualValueToRemove = Mathf.Min(valueToRemoveUntilZero, value);
                item.Value -= actualValueToRemove;
                valuesToRemove -= actualValueToRemove;
                if (item.Value <= 0)
                {
                    _inventoryItems.Remove(item);
                }
            }
            
            // second part - remove complete items until we remove all value
            while (valuesToRemove > 0)
            {
                var actualValueToRemove = Mathf.Min(maxInStack, valuesToRemove);
                item = GetCompleteStackItem(id);
                item.Value -= actualValueToRemove;
                valuesToRemove -= actualValueToRemove;
                if (item.Value <= 0)
                {
                    _inventoryItems.Remove(item);
                }
            }
            
            OnUpdated?.Invoke(id, GetTotalAmount(id));
        }

        public int GetTotalAmount(string id)
        {
            int amount = 0;
            foreach (var item in _inventoryItems)
            {
                if (item.Id == id)
                    amount += item.Value;
            }

            return amount;
        }

        public InventoryData DeepClone()
        {
            InventoryData clonedData = new();
            clonedData._inventoryItems = _inventoryItems.ConvertAll(item => new InventoryItemData(item));

            return clonedData;
        }
        
        private InventoryItemData GetCompleteStackItem(string id)
        {
            foreach (var item in _inventoryItems)
            {
                if (item.Id == id && item.Value == DefinitionsFacade.GetMaxInStack(id))
                    return item;
            }

            return null;
        }

        private InventoryItemData GetIncompleteStackItem(string id)
        {
            foreach (var item in _inventoryItems)
            {
                if (item.Id == id && item.Value < DefinitionsFacade.GetMaxInStack(id))
                    return item;
            }

            return null;
        }
    }

    [Serializable]
    public class InventoryItemData
    {
        [InventoryId] public string Id;
        public int Value;

        public InventoryItemData(string id, int value)
        {
            Id = id;
            Value = value;
        }
        
        public InventoryItemData(InventoryItemData itemData)
        {
            Id = itemData.Id;
            Value = itemData.Value;
        }
    }
}
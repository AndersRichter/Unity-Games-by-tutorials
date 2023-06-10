using System;
using UnityEngine;

namespace Model.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;

        public InventoryData Inventory => _inventory;
        public int Health;

        public void Initialize(PlayerData playerData)
        {
            Health = playerData.Health;
            _inventory = playerData.Inventory.DeepClone();
        }
    }
}
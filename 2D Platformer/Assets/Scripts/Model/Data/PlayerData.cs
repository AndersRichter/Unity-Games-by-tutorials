using System;
using Model.Data.Properties;
using UnityEngine;

namespace Model.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;

        public InventoryData Inventory => _inventory;
        public IntObservableProperty Health;

        public void Initialize(PlayerData playerData)
        {
            Health = new IntObservableProperty(playerData.Health.Value);
            _inventory = playerData.Inventory.DeepClone();
        }
    }
}
using System;
using Model.Data.Properties;
using Model.Definitions;
using UnityEngine;
using Utils.Disposable;

namespace Model.Data
{
    public class QuickInventoryData : IDisposable
    {
        private readonly PlayerData _playerData;
        
        public InventoryItemData[] Inventory { get; private set; }

        public readonly IntObservableProperty SelectedIndex = new(0);

        public event Action OnChanged;

        public InventoryItemData SelectedItem => Inventory[SelectedIndex.Value];

        public QuickInventoryData(PlayerData playerData)
        {
            _playerData = playerData;

            Inventory = _playerData.Inventory.GetAllItems(ItemTag.Usable);
            _playerData.Inventory.OnUpdated += OnInventoryUpdated;
        }

        public IDisposable Subscribe(Action callback)
        {
            OnChanged += callback;
            return new ActionDisposable(() => OnChanged -= callback);
        }

        private void OnInventoryUpdated(string id, int value)
        {
            Inventory = _playerData.Inventory.GetAllItems(ItemTag.Usable);
            SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, Inventory.Length);
            OnChanged?.Invoke();
        }

        public void SetNextItem()
        {
            SelectedIndex.Value = (int)Mathf.Repeat(SelectedIndex.Value + 1, Inventory.Length);
        }

        public void Dispose()
        {
            _playerData.Inventory.OnUpdated -= OnInventoryUpdated;
        }
    }
}
using System.Collections.Generic;
using Model;
using UnityEngine;
using Utils.Disposable;

namespace UI.HUD.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItemWidget _prefab;

        private readonly CompositeDisposable _subscriptions = new();
        private GameSession _gameSession;
        private List<InventoryItemWidget> _createdItems = new();

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
            _subscriptions.Retain(_gameSession.QuickInventoryData.Subscribe(Rebuild));

            Rebuild();
        }

        private void Rebuild()
        {
            var inventoryItems = _gameSession.QuickInventoryData.Inventory;

            // create items only first time, then use cashed items
            // example: _inventoryItems.Length = 3, _createdItems.Count = 0 => 0 items created, we need to create 3 new items
            // example: _inventoryItems.Length = 3, _createdItems.Count = 3 => 3 items created, we need to create 0 new items
            // example: _inventoryItems.Length = 4, _createdItems.Count = 3 => 3 items created, we need to create 1 new items
            for (int i = _createdItems.Count; i < inventoryItems.Length; i++)
            {
                var item = Instantiate(_prefab, _container);
                _createdItems.Add(item);
            }

            for (int i = 0; i < inventoryItems.Length; i++)
            {
                _createdItems[i].SetData(inventoryItems[i], i);
                _createdItems[i].gameObject.SetActive(true);
            }
            
            // hide unused items
            for (int i = inventoryItems.Length; i < _createdItems.Count; i++)
            {
                _createdItems[i].gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _subscriptions.Dispose();
        }
    }
}
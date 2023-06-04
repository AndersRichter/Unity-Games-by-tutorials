using Model;
using Model.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class RequireInventoryItemComponent : MonoBehaviour
    {
        [SerializeField] private InventoryItemData[] _required;
        [SerializeField] private bool _removeAfterUse;
        
        [SerializeField] private UnityEvent _onSuccess;
        [SerializeField] private UnityEvent _onFail;

        private GameSession _gameSession;

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
        }

        public void Check()
        {
            var areAllRequiredItems = true;

            foreach (var item in _required)
            {
                var itemCount = _gameSession.PlayerData.Inventory.GetTotalAmount(item.Id);

                if (itemCount < item.Value)
                {
                    areAllRequiredItems = false;
                }
            }

            if (areAllRequiredItems)
            {
                if (_removeAfterUse)
                {
                    foreach (var item in _required)
                    {
                        _gameSession.PlayerData.Inventory.Remove(item.Id, item.Value);
                    }
                }
                
                _onSuccess?.Invoke();
            }
            else
            {
                _onFail?.Invoke();
            }
        }
    }
}
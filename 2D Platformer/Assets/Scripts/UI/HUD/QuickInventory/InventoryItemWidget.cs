using Model;
using Model.Data;
using Model.Definitions;
using UnityEngine;
using UnityEngine.UI;
using Utils.Disposable;

namespace UI.HUD.QuickInventory
{
    public class InventoryItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selectedFrame;
        [SerializeField] private Text _value;

        private readonly CompositeDisposable _subscriptions = new();

        private int _index;

        private void Start()
        {
            var gameSession = GameSession.Instance;
            _subscriptions.Retain(gameSession.QuickInventoryData.SelectedIndex.Subscribe(OnIndexChanged));
            OnIndexChanged(gameSession.QuickInventoryData.SelectedIndex.Value, gameSession.QuickInventoryData.SelectedIndex.Value);
        }

        private void OnIndexChanged(int newValue, int oldValue)
        {
            _selectedFrame.SetActive(_index == newValue);
        }

        public void SetData(InventoryItemData item, int index)
        {
            _index = index;
            var definition = DefinitionsFacade.Instance.ItemsDefinition.GetFirstOrDefault(item.Id);
            _icon.sprite = definition.Icon;
            _value.text = item.Value.ToString();
        }

        private void OnDestroy()
        {
            _subscriptions.Dispose();
        }
    }
}
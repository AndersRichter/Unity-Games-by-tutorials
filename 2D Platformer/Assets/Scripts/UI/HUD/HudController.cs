using Model;
using Model.Definitions;
using UI.Widgets;
using UnityEngine;

namespace UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;

        private GameSession _session;

        private void Start()
        {
            _session = GameSession.Instance;
            _session.PlayerData.Health.OnChanged += OnHealthChanged;
            
            OnHealthChanged(_session.PlayerData.Health.Value, 0);
        }

        private void OnHealthChanged(int newValue, int oldValue)
        {
            var maxHealth = DefinitionsFacade.Instance.CharacteristicsDefinition.MaxHealth;
            var value = (float)newValue / maxHealth;
            _healthBar.SetProgress(value);
        }

        private void OnDestroy()
        {
            _session.PlayerData.Health.OnChanged -= OnHealthChanged;
        }
    }
}
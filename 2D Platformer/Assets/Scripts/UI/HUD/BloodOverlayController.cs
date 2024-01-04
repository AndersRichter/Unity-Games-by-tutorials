using Model;
using Model.Definitions;
using UnityEngine;

namespace UI.HUD
{
    [RequireComponent(typeof(Animator))]
    public class BloodOverlayController : MonoBehaviour
    {
        [SerializeField] private Transform _overlay;

        private static readonly int AnimatorHealth = Animator.StringToHash("Health");

        private Animator _animator;
        private Vector3 _overScale;
        private GameSession _session;

        private void Start()
        {
            _session = GameSession.Instance;
            _animator = GetComponent<Animator>();
            _overScale = _overlay.localScale - Vector3.one;
            
            _session.PlayerData.Health.OnChanged += OnHealthChanged;
            OnHealthChanged(_session.PlayerData.Health.Value, 0);
        }

        private void OnHealthChanged(int newValue, int _)
        {
            var maxHealth = DefinitionsFacade.Instance.CharacteristicsDefinition.MaxHealth;
            var hpNormalized = (float)newValue / maxHealth;
            _animator.SetFloat(AnimatorHealth, hpNormalized);

            var overlayModifier = Mathf.Max(hpNormalized - 0.3f, 0f);
            _overlay.localScale = Vector3.one + _overScale * overlayModifier;
        }
        
        private void OnDestroy()
        {
            _session.PlayerData.Health.OnChanged -= OnHealthChanged;
        }
    }
}
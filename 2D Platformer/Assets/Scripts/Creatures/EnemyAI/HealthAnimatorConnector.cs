using Components;
using UnityEngine;

namespace Creatures.EnemyAI
{
    public class HealthAnimatorConnector : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private Animator _animator;
        
        private static readonly int AnimatorHealth = Animator.StringToHash("health");

        private void Awake()
        {
            OnHealthChanged(_health.Health);
        }

        public void OnHealthChanged(int health)
        {
            _animator.SetInteger(AnimatorHealth, health);
        }
    }
}
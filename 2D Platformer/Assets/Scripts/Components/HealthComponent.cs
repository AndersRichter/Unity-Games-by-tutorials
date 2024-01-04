using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int health;
        [SerializeField] private UnityEvent onTakeDamage;
        [SerializeField] private UnityEvent onTakeHeal;
        [SerializeField] private UnityEvent onDie;
        [SerializeField] private UnityEvent<int> onUpdate;
        [SerializeField] private UnityEvent<float> _onUpdateProgress;

        private int _maxHealth;

        public int Health => health;

        private void Awake()
        {
            _maxHealth = health;
        }

        public void ApplyHealthUpdate(int delta)
        {
            if (health > 0)
            {
                health += delta;
            
                onUpdate?.Invoke(health);
                _onUpdateProgress?.Invoke((float)health / _maxHealth);

                if (delta > 0)
                {
                    onTakeHeal?.Invoke();
                }
                else
                {
                    onTakeDamage?.Invoke();
                }
            }
            
            if (health <= 0)
            {
                onDie?.Invoke();
            }
        }

        public void SetInitialHealth(int initialHealth)
        {
            health = initialHealth;
        }
    }
}

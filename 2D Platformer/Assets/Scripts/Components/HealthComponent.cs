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

        public void ApplyHealthUpdate(int delta)
        {
            health += delta;
            
            onUpdate?.Invoke(health);

            if (delta > 0)
            {
                onTakeHeal?.Invoke();
            }
            else
            {
                onTakeDamage?.Invoke();
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

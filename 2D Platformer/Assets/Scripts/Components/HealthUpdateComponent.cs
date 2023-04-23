using UnityEngine;

namespace Components
{
    public class HealthUpdateComponent : MonoBehaviour
    {
        [SerializeField] private int healthDelta;

        public void UpdateHealth(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();

            // we can't use ?. here, because Unity game objects have override null comparison
            if (healthComponent != null)
            {
                healthComponent.ApplyHealthUpdate(healthDelta);
            }
        }
    }
}

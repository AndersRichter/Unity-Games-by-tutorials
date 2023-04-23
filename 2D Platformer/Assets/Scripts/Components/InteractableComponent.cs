using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent action;

        public void Interact()
        {
            action?.Invoke();
        }
    }
}

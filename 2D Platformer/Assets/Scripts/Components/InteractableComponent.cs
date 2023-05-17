using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Components
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent action;

        public void Interact()
        {
            action?.Invoke();
        }
        
#if UNITY_EDITOR
        private GameObject[] _connectedGameObjects;

        private void OnValidate()
        {
            _connectedGameObjects = action.ToGameObjects();
        }

        // Example of using reflection - get targets of "action" and draw lines to them
        public void OnDrawGizmos()
        {
            foreach (var connectedGameObject in _connectedGameObjects)
            {
                if (connectedGameObject != null)
                {
                    Debug.DrawLine(transform.position, connectedGameObject.transform.position, Color.green);
                }
            }
        }
#endif
    }
}

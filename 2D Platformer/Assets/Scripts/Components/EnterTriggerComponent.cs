using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    [RequireComponent(typeof(Collider2D))]
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string[] targetTags;
        [SerializeField] private UnityEvent<GameObject> action;

        private void OnTriggerEnter2D(Collider2D col)
        {
            foreach (var targetTag in targetTags)
            {
                if (col.gameObject.CompareTag(targetTag))
                {
                    action?.Invoke(col.gameObject);
                }
            }
        }

        public void AddEventToAction(UnityAction<GameObject> callback)
        {
            action.AddListener(callback);
        }
    }
}

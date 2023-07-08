using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static Utils.LayerUtils;

namespace Components
{
    [RequireComponent(typeof(Collider2D))]
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string[] targetTags;
        [SerializeField] private string[] ignoreTags;
        [SerializeField] private LayerMask targetLayers = ~0; // Everything
        [SerializeField] private UnityEvent<GameObject> action;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!IsCollisionWithLayer(col.gameObject, targetLayers))
            {
                return;
            }

            if (targetTags.Length > 0 && !targetTags.Any(targetTag => col.gameObject.CompareTag(targetTag)))
            {
                return;
            }

            if (ignoreTags.Length > 0 && ignoreTags.Any(ignoreTag => col.gameObject.CompareTag(ignoreTag)))
            {
                return;
            }

            action?.Invoke(col.gameObject);
        }
    }
}

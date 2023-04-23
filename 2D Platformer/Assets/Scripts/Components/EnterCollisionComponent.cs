using System;
using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    [RequireComponent(typeof(Collider2D))]
    public class EnterCollisionComponent : MonoBehaviour
    {
        [SerializeField] private string[] targetTags;
        [SerializeField] private EnterEvent action;

        private void OnCollisionEnter2D(Collision2D col)
        {
            foreach (var targetTag in targetTags)
            {
                if (col.gameObject.CompareTag(targetTag))
                {
                    action?.Invoke(col.gameObject);
                }
            }
        }
        
        [Serializable]
        public class EnterEvent : UnityEvent<GameObject> {}
    }
}

using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Components
{
    public class GetCircleOverlapsComponent : MonoBehaviour
    {
        [SerializeField] private float radius = 1f;
        
        private readonly Collider2D[] _interactResults = new Collider2D[20];
        
        public GameObject[] GetOverlaps()
        {
            // Another way to detect collisions (CircleCast is another)
            var overlapsArraySize = Physics2D.OverlapCircleNonAlloc(transform.position, radius, _interactResults);
            var overlaps = new List<GameObject>();

            for (int i = 0; i < overlapsArraySize; i++)
            {
                overlaps.Add(_interactResults[i].gameObject);
            }

            return overlaps.ToArray();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = HandlesUtils.TransparentRed;
            UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, radius);
        }
#endif
    }
}
using UnityEngine;

namespace Components
{
    // Analog of component LayerCheckComponent, but works instantly
    // (skip OnTrigger... events, checks Linecast on every frame)
    public class LayerCheckLineCastComponent : MonoBehaviour
    {
        [SerializeField] private LayerMask _layer;
        [SerializeField] private Transform _target;
        
        private bool _isTouchingLayer;
        private RaycastHit2D[] _result = new RaycastHit2D[1];

        public bool IsTouchingLayer => _isTouchingLayer;

        private void Update()
        {
            _isTouchingLayer = Physics2D.LinecastNonAlloc(transform.position, _target.position, _result, _layer) > 0;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.DrawLine(transform.position, _target.position);
        }
#endif
    }
}
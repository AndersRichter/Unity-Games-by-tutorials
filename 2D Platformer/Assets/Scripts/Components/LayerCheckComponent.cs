using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(Collider2D))]
    // Another way to check collisions except RayCast - Collider2D with Is Trigger checkbox
    public class LayerCheckComponent : MonoBehaviour
    {
        [SerializeField] private LayerMask layer;

        private Collider2D _collider;
        private bool _isTouchingLayer;

        public bool IsTouchingLayer => _isTouchingLayer;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(layer);
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(layer);
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(layer);
        }
    }
}
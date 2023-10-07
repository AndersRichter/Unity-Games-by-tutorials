using UnityEngine;

namespace Components
{
    public class ParallaxEffectComponent : MonoBehaviour
    {
        [SerializeField] private float _effectSpeed;
        [SerializeField] private Transform _followTarget;
        
        private float _startX;

        private void Start()
        {
            _startX = transform.position.x;
        }

        private void LateUpdate()
        {
            var currentPosition = transform.position;
            var deltaX = _followTarget.position.x * _effectSpeed;
            transform.position = new Vector3(_startX + deltaX, currentPosition.y, currentPosition.z);
        }
    }
}
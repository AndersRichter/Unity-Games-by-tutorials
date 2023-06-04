using UnityEngine;

namespace Components
{
    public class VerticalLevitationComponent : MonoBehaviour
    {
        [SerializeField] private float _rangeModificator = 1;
        [SerializeField] private float _frequencyModificator = 1;
        [SerializeField] private bool _randomize;
        
        private Rigidbody2D _rigidbody;
        private float _defaultYPosition;
        private float _time;
        private float _seed;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _defaultYPosition = _rigidbody.position.y;

            if (_randomize)
            {
                // 2 Pi = full circle
                _seed = Random.value * Mathf.PI * 2;
            }
        }

        private void FixedUpdate()
        {
            var position = _rigidbody.position;
            position.y = _defaultYPosition + Mathf.Sin(_seed + _time * _frequencyModificator) * _rangeModificator;
            _rigidbody.MovePosition(position);
            _time += Time.fixedDeltaTime;
        }
    }
}
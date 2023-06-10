using UnityEngine;

namespace Components
{
    public class CircularMovementComponent : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _speed = 1f;

        private Rigidbody2D[] _children;
        private Vector2[] _childrenPositions;
        private float _time;

        private void Awake()
        {
            CollectChildren();
        }

        private void CollectChildren()
        {
            _children = GetComponentsInChildren<Rigidbody2D>();
            _childrenPositions = new Vector2[_children.Length];
        }

        private void Update()
        {
            CalculatePositions();

            var allCoinsCollected = true;

            for (var i = 0; i < _children.Length; i++)
            {
                if (_children[i])
                {
                    _children[i].MovePosition(_childrenPositions[i]);
                    allCoinsCollected = false;
                }
            }

            if (allCoinsCollected)
            {
                enabled = false;
                // wait 1 second to all animations are done
                Destroy(gameObject, 1f);
            }

            _time += Time.deltaTime;
        }

        private void CalculatePositions()
        {
            var step = 2 * Mathf.PI / _children.Length;

            Vector2 containerPosition = transform.position;
            for (var i = 0; i < _children.Length; i++)
            {
                var angle = step * i;
                var position = new Vector2(
                    Mathf.Cos(angle + _time * _speed) * _radius,
                    Mathf.Sin(angle + _time * _speed) * _radius
                );
                
                _childrenPositions[i] = position + containerPosition;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            CollectChildren();
            CalculatePositions();

            for (var i = 0; i < _children.Length; i++)
            {
                _children[i].transform.position = _childrenPositions[i];
            }
        }

        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, _radius);
        }
#endif
    }
}
using UnityEngine;

namespace Creatures.Weapons
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 0.1f;

        private Rigidbody2D _rigidbody;
        private int _direction;

        private void Start()
        {
            _direction = transform.lossyScale.x > 0 ? 1 : -1;
            _rigidbody = GetComponent<Rigidbody2D>();
            
            // We can use force to move object (Rigidbody: Body Type - Dynamic, Gravity Scale - can be different, affect object track)
            // var force = new Vector2(_direction * speed, 0);
            // _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        // Or we can use MovePosition to move object (Rigidbody: Body Type - Kinematic, Gravity Scale - 1)
        private void FixedUpdate()
        {
            var position = _rigidbody.position;
            position.x += _direction * speed;
            _rigidbody.MovePosition(position);
        }
    }
}
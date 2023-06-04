using UnityEngine;

namespace Creatures.Weapons
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected float Speed = 0.1f;
        [SerializeField] protected bool InvertX;

        protected Rigidbody2D Rigidbody;
        protected int Direction;

        protected virtual void Start()
        {
            var invertedModificator = InvertX ? -1 : 1;
            Direction = invertedModificator * transform.lossyScale.x > 0 ? 1 : -1;
            Rigidbody = GetComponent<Rigidbody2D>();
            
            // We can use force to move object (Rigidbody: Body Type - Dynamic, Gravity Scale - can be different, affect object track)
            // var force = new Vector2(_direction * speed, 0);
            // _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
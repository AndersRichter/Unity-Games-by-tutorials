using UnityEngine;

namespace Creatures.Weapons
{
    public class ProjectileDirectional : Projectile
    {
        public void Launch(Vector2 direction)
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Rigidbody.AddForce(direction * Speed, ForceMode2D.Impulse);
        }
    }
}
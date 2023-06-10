namespace Creatures.Weapons
{
    public class ProjectileStraight : Projectile
    {
        // Or we can use MovePosition to move object (Rigidbody: Body Type - Kinematic, Gravity Scale - 1)
        private void FixedUpdate()
        {
            var position = Rigidbody.position;
            position.x += Direction * Speed;
            Rigidbody.MovePosition(position);
        }
    }
}
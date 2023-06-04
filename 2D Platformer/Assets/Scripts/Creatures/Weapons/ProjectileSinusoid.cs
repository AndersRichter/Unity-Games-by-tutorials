using UnityEngine;

namespace Creatures.Weapons
{
    public class ProjectileSinusoid : Projectile
    {
        [SerializeField] private float _rangeModificator = 1;
        [SerializeField] private float _frequencyModificator = 1;
        
        private float _defaultYPosition;
        private float _time;
        
        protected override void Start()
        {
            base.Start();
            _defaultYPosition = Rigidbody.position.y;
        }

        private void FixedUpdate()
        {
            var position = Rigidbody.position;
            position.x += Direction * Speed;
            // we use infinitely increasing value of time to move y position up and down
            // according to sinusoid graph
            position.y = _defaultYPosition + Mathf.Sin(_time * _frequencyModificator) * _rangeModificator;
            Rigidbody.MovePosition(position);
            _time += Time.fixedDeltaTime;
        }
    }
}
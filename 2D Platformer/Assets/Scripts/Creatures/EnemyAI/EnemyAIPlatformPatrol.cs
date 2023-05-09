using System.Collections;
using Components;
using UnityEngine;

namespace Creatures.EnemyAI
{
    [RequireComponent(typeof(Creature))]
    public class EnemyAIPlatformPatrol : EnemyAIPatrol
    {
        [SerializeField] private LayerCheckComponent groundCheck;
        [SerializeField] private LayerCheckComponent wallCheck;

        private Creature _creature;
        private Vector2 _currentDirection = Vector2.left;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
        }

        public override IEnumerator Patrol()
        {
            while (enabled)
            {
                if (!groundCheck.IsTouchingLayer || wallCheck.IsTouchingLayer)
                {
                    _currentDirection = _currentDirection == Vector2.left ? Vector2.right : Vector2.left;
                }
                
                _creature.SetDirection(_currentDirection);

                yield return null;
            }
        }
    }
}
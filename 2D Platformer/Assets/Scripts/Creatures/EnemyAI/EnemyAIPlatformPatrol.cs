using System.Collections;
using Components;
using UnityEngine;

namespace Creatures.EnemyAI
{
    [RequireComponent(typeof(Creature))]
    public class EnemyAIPlatformPatrol : EnemyAIPatrol
    {
        [SerializeField] private LayerCheckLineCastComponent _groundCheck;
        [SerializeField] private LayerCheckComponent _wallCheck;

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
                if (!_groundCheck.IsTouchingLayer || _wallCheck.IsTouchingLayer)
                {
                    _currentDirection = _currentDirection == Vector2.left ? Vector2.right : Vector2.left;
                }
                
                _creature.SetDirection(_currentDirection);

                yield return null;
            }
        }
    }
}
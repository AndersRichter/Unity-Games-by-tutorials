using System.Collections;
using UnityEngine;

namespace Creatures.EnemyAI
{
    [RequireComponent(typeof(Creature))]
    public class EnemyAIPointsPatrol : EnemyAIPatrol
    {
        [SerializeField] private Transform[] points;
        [SerializeField] private float accuracy = 0.5f;

        private Creature _creature;
        private int _currentPoint;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
        }

        public override IEnumerator Patrol()
        {
            while (enabled)
            {
                if (IsReachedPoint())
                {
                    _currentPoint = (int)Mathf.Repeat(_currentPoint + 1, points.Length);
                }

                var direction = points[_currentPoint].position - transform.position;
                direction.y = 0;
                _creature.SetDirection(direction.normalized);

                yield return null;
            }
        }

        private bool IsReachedPoint()
        {
            return (points[_currentPoint].position - transform.position).magnitude < accuracy;
        }
    }
}
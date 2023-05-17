using System.Collections;
using Components;
using UnityEngine;

namespace Creatures.EnemyAI
{
    [RequireComponent(typeof(SpawnSetOfComponents), typeof(Creature), typeof(EnemyAIPatrol))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private LayerCheckComponent vision;
        [SerializeField] private LayerCheckComponent canAttack;
        [SerializeField] private float agroDuration = 0.5f;
        [SerializeField] private float missDuration = 0.5f;
        [SerializeField] private float attackCooldown = 1f;

        private IEnumerator _runningCoroutine;
        private GameObject _heroGameObject;
        
        private SpawnSetOfComponents _particles;
        private Creature _creature;
        private EnemyAIPatrol _enemyAIPatrol;

        private void Awake()
        {
            _particles = GetComponent<SpawnSetOfComponents>();
            _creature = GetComponent<Creature>();
            _enemyAIPatrol = GetComponent<EnemyAIPatrol>();
        }

        private void Start()
        {
            StartEnemyState(_enemyAIPatrol.Patrol());
        }

        public void OnHeroInVision(GameObject heroGameObject)
        {
            if (_creature.IsDead)
            {
                return;
            }
            
            _heroGameObject = heroGameObject;
            
            StartEnemyState(AgroToHero());
        }
        
        private IEnumerator AgroToHero()
        {
            LookAtHero();
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(agroDuration);
            
            StartEnemyState(GoToHero());
        }

        private void LookAtHero()
        {
            _creature.UpdateSpriteDirection(GetDirectionToTarget());
        }
        
        private IEnumerator GoToHero()
        {
            while (vision.IsTouchingLayer)
            {
                if (canAttack.IsTouchingLayer)
                {
                    StartEnemyState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }
                yield return null;
            }
            
            StartEnemyState(MissHero());
        }

        private IEnumerator MissHero()
        {
            _particles.Spawn("Miss");
            yield return new WaitForSeconds(missDuration);
            StartEnemyState(_enemyAIPatrol.Patrol());
        }
        
        private IEnumerator Attack()
        {
            while (canAttack.IsTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(attackCooldown);
            }
            
            StartEnemyState(GoToHero());
        }

        private void SetDirectionToTarget()
        {
            _creature.SetDirection(GetDirectionToTarget());
        }

        private Vector2 GetDirectionToTarget()
        {
            var direction = _heroGameObject.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }

        public void OnDie()
        {
            StopRunningCoroutine();
            
            // TODO specific for Sharky, maybe move to separate class
            var capsuleCollider = GetComponent<CapsuleCollider2D>();
            var polygonCollider = GetComponent<PolygonCollider2D>();
            
            if (capsuleCollider != null && polygonCollider != null)
            {
                capsuleCollider.enabled = false;
                polygonCollider.enabled = true;
            }
        }

        private void StopRunningCoroutine()
        {
            _creature.SetDirection(Vector2.zero);
            
            if (_runningCoroutine != null)
            {
                StopCoroutine(_runningCoroutine);
            }
        }

        private void StartEnemyState(IEnumerator coroutine)
        {
            StopRunningCoroutine();

            _runningCoroutine = coroutine;
            StartCoroutine(coroutine);
        }
    }
}
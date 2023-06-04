using Components;
using UnityEngine;
using Utils;

namespace Creatures.EnemyAI
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAIShootingTrap : MonoBehaviour
    {
        [SerializeField] private LayerCheckComponent vision;
        [SerializeField] private LayerCheckComponent canMeleeAttack;
        [SerializeField] private GetCircleOverlapsComponent meleeAttack;
        [SerializeField] private CooldownUtils rangeCooldown;
        [SerializeField] private CooldownUtils meleeAttackCooldown;
        [SerializeField] private SpawnComponent bullet;
        [SerializeField] private string attackIgnoreTag;
        [SerializeField] private int attackDamage;
        
        private Animator _animator;
        
        private static readonly int AnimatorDamageReceived = Animator.StringToHash("damageReceived");
        private static readonly int AnimatorMeleeAttack = Animator.StringToHash("meleeAttack");
        private static readonly int AnimatorRangeAttack= Animator.StringToHash("rangeAttack");
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (vision.IsTouchingLayer)
            {
                if (canMeleeAttack.IsTouchingLayer)
                {
                    if (meleeAttackCooldown.IsReady)
                    {
                        MeleeAttack();
                    }
                    return;
                }

                if (rangeCooldown.IsReady)
                {
                    RangeAttack();
                }
            }
        }

        public void OnTakeDamage()
        {
            _animator.SetTrigger(AnimatorDamageReceived);
        }

        private void MeleeAttack()
        {
            meleeAttackCooldown.Reset();
            _animator.SetTrigger(AnimatorMeleeAttack);
        }

        private void RangeAttack()
        {
            rangeCooldown.Reset();
            _animator.SetTrigger(AnimatorRangeAttack);
        }
        
        // is used by event in animation "bite"
        public void PerformMeleeAttack()
        {
            // TODO refactor, move to common component + move logic into GetCircleOverlapsComponent
            // TODO duplication with Creature
            var overlaps = meleeAttack.GetOverlaps();
            foreach (var gameObj in overlaps)
            {
                var health = gameObj.GetComponent<HealthComponent>();
                var shouldIgnoreAttack = gameObj.CompareTag(attackIgnoreTag);

                if (health != null && !shouldIgnoreAttack)
                {
                    health.ApplyHealthUpdate(-attackDamage); 
                }
            }
        }
        
        // is used by event in animation "rangeAttack"
        public void PerformRangeAttack()
        {
            bullet.Spawn();
        }
    }
}
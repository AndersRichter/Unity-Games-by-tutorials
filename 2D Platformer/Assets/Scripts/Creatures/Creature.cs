using Components;
using UnityEngine;

namespace Creatures
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(HealthComponent))]
    public class Creature : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float speed;
        [SerializeField] private bool invertScale;
        [SerializeField] protected float jumpForce;
        [SerializeField] private float damageJumpForce;
        
        [Space] [Header("Attack")]
        [SerializeField] private GetCircleOverlapsComponent attackRange;
        [SerializeField] private int attackDamage;
        [SerializeField] private string attackIgnoreTag;
        
        [Space] [Header("Ground check")]
        [SerializeField] protected LayerMask groundLayer;
        [SerializeField] private float groundCheckRadius;
        [SerializeField] private Vector3 groundCheckPositionDelta;
        
        [Space] [Header("Particles")]
        [SerializeField] protected SpawnSetOfComponents particlesList;

        protected Rigidbody2D Rigidbody;
        protected HealthComponent HealthComponent;
        protected Vector2 DirectionVector;
        protected Animator Animator;
        protected bool IsGrounded;
        
        [HideInInspector] public bool IsDead;
        
        private bool _isJumping;

        private static readonly int AnimatorIsGrounded = Animator.StringToHash("isGrounded");
        private static readonly int AnimatorIsRunning = Animator.StringToHash("isRunning");
        private static readonly int AnimatorVerticalVelocity = Animator.StringToHash("verticalVelocity");
        private static readonly int AnimatorDamageReceived = Animator.StringToHash("damageReceived");
        private static readonly int AnimatorHealReceived = Animator.StringToHash("healReceived");
        private static readonly int AnimatorIsAttacking = Animator.StringToHash("isAttacking");
        private static readonly int AnimatorIsDead = Animator.StringToHash("isDead");

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            HealthComponent = GetComponent<HealthComponent>();
        }
        
        protected virtual void FixedUpdate()
        {
            Rigidbody.velocity = new Vector2(CalculateXVelocity(), CalculateYVelocity());
            
            SetAnimatorProperties();
            UpdateSpriteDirection(DirectionVector);
        }

        protected virtual void Update()
        {
            IsGrounded = DetectGrounded();
        }
        
        protected virtual void OnDrawGizmos()
        {
            // Draw gizmo with jumping sphere
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position + groundCheckPositionDelta, groundCheckRadius);
        }

        public void SetDirection(Vector2 directionVector)
        {
            DirectionVector = directionVector;
        }
        
        private bool DetectGrounded()
        {
            // One way to detect collisions (OverlapCircleNonAlloc is another)
            var hit = Physics2D.CircleCast(transform.position + groundCheckPositionDelta, groundCheckRadius, Vector2.down, 0, groundLayer);
            return hit.collider != null;
        }
        
        protected bool IsJumping()
        {
            return DirectionVector.y > 0;
        }
        
        public void UpdateSpriteDirection(Vector2 direction)
        {
            var invertScaleMultiplier = invertScale ? -1 : 1;
            switch (direction.x)
            {
                case > 0:
                    transform.localScale = new Vector3(1 * invertScaleMultiplier, 1, 1);
                    break;
                case < 0:
                    transform.localScale = new Vector3(-1 * invertScaleMultiplier, 1, 1);
                    break;
            }
        }
        
        private void SetAnimatorProperties()
        {
            Animator.SetBool(AnimatorIsRunning, DirectionVector.x != 0);
            Animator.SetBool(AnimatorIsGrounded, IsGrounded);
            Animator.SetFloat(AnimatorVerticalVelocity, Rigidbody.velocity.y);
        }
        
        public virtual void TakeHeal()
        {
            Animator.SetTrigger(AnimatorHealReceived);
        }
        
        public void OnDie()
        {
            IsDead = true;
            Animator.SetBool(AnimatorIsDead, true);
        }
        
        public virtual void TakeDamage()
        {
            _isJumping = false;
            Animator.SetTrigger(AnimatorDamageReceived);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, damageJumpForce);
        }
        
        public virtual void Attack()
        {
            Animator.SetTrigger(AnimatorIsAttacking);
            particlesList.Spawn("Attack");
        }
        
        private float CalculateXVelocity()
        {
            return DirectionVector.x * speed;
        }

        protected virtual float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
        
            if (IsGrounded)
            {
                _isJumping = false;
            }

            if (IsJumping())
            {
                _isJumping = true;
                // 0.01 - fixed rare behavior when hero makes unexpected double jump, because raycast sphere is a little bit lower then hero body 
                var isFalling = Rigidbody.velocity.y <= 0.01f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (Rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded)
            {
                yVelocity += jumpForce;
                particlesList.Spawn("Jump");
            }

            return yVelocity;
        }
        
        // is used by animation event
        public void SpawnFootStepsParticles()
        {
            particlesList.Spawn("FootSteps");
        }
        
        // is used by event in animation "attack"
        public void PerformAttack()
        {
            var overlaps = attackRange.GetOverlaps();
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
    }
}
using System;
using System.Collections;
using Components;
using Model;
using UnityEditor.Animations;
using UnityEngine;
using Utils;

namespace Creatures.Hero
{
    public class Hero : Creature
    {
        [Header("Animators")]
        [SerializeField] private AnimatorController armedAnimator;
        [SerializeField] private AnimatorController disarmedAnimator;

        [Space] [Header("Interaction")]
        [SerializeField] private GetCircleOverlapsComponent interactionRange;

        [Space] [Header("Particles")]
        [SerializeField] private ParticleSystem damageParticles;
        [SerializeField] private float longFallLength;

        [Space] [Header("Sticky Walls")]
        [SerializeField] private LayerCheckComponent stickyWallsCheck;
    
        [Space] [Header("Fall damage")]
        [SerializeField] private float fallDamageVelocity;
        
        [Space] [Header("Attack")]
        [SerializeField] private CooldownUtils attackCooldown;

        [Space] [Header("Throw")]
        [SerializeField] private CooldownUtils throwCooldown;
        [SerializeField] private float longThrowInterval;
        
        // TODO maybe move to inventory, to store different type of poisons
        [Space] [Header("Heal")]
        [SerializeField] private int _healByPoisonValue;
        
        private static readonly int AnimatorThrown = Animator.StringToHash("thrown");
        private static readonly int AnimatorIsOnTheWall = Animator.StringToHash("isOnTheWall");

        private int CoinsCount => _gameSession.PlayerData.Inventory.GetTotalAmount("Coins");
        private int SwordsCount => _gameSession.PlayerData.Inventory.GetTotalAmount("Swords");
        private int HealthPoisonsCount => _gameSession.PlayerData.Inventory.GetTotalAmount("HealthPoisons");

        private bool _isOnStickyWall;
        private bool _isDoubleJumpAllowed;
        private float _defaultGravityScale;

        private float? _startFallingPositionY;
        private float? _endFallingPositionY;

        private GameSession _gameSession;

        protected override void Awake()
        {
            base.Awake();

            _defaultGravityScale = Rigidbody.gravityScale;
        }

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
        
            HealthComponent.SetInitialHealth(_gameSession.LevelStartPlayerData.Health);
            _gameSession.PlayerData.Inventory.OnUpdated += OnInventoryUpdated;
            UpdateHeroWeapon(_gameSession.LevelStartPlayerData.Inventory.GetTotalAmount("Swords"));
        }

        private void OnDestroy()
        {
            // We always should unsubscribe from events to clean memory and helping garbage collector
            _gameSession.PlayerData.Inventory.OnUpdated -= OnInventoryUpdated;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            HandleLongFall();
        }
    
        protected override void Update()
        {
            base.Update();
        
            HandleStickyWalls();
        }

        public void OnHealthChanged(int health)
        {
            _gameSession.PlayerData.Health = health;
        }

        public void AddToInventory(string id, int value)
        {
            _gameSession.PlayerData.Inventory.Add(id, value);
        }

        private void OnInventoryUpdated(string id, int value)
        {
            switch (id)
            {
                case "Swords":
                    UpdateHeroWeapon(value);
                    break;
            }
        }

        public override void Attack()
        {
            if (SwordsCount <= 0 || !attackCooldown.IsReady)
            {
                return;
            }

            attackCooldown.Reset();
            base.Attack();
        }

        private void UpdateHeroWeapon(int swordsAmount)
        {
            Animator.runtimeAnimatorController = swordsAmount > 0 ? armedAnimator : disarmedAnimator;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (LayerUtils.IsCollisionWithLayer(col.gameObject, groundLayer))
            {
                var contact = col.contacts[0];
                if (contact.relativeVelocity.y >= fallDamageVelocity)
                {
                    HealthComponent.ApplyHealthUpdate(-1);
                }
            }
        }

        private void HandleStickyWalls()
        {
            if (!stickyWallsCheck) return;
        
            // player is on the wall and press moving arrows in the same direction where hero turned.
            // here we check sprite rotation to define hero turn direction.
            // we use Abs and tolerance value because of float numbers, they can lose precise in math operations
            _isOnStickyWall = stickyWallsCheck.IsTouchingLayer && Math.Abs(DirectionVector.x - transform.localScale.x) < 0.01;
            
            Animator.SetBool(AnimatorIsOnTheWall, _isOnStickyWall);

            if (_isOnStickyWall)
            {
                Rigidbody.gravityScale = 0;
            }
            else
            {
                Rigidbody.gravityScale = _defaultGravityScale;
            }
        }

        private void HandleLongFall()
        {
            var yVelocity = Rigidbody.velocity.y;
        
            // if Hero stood or jumped and now falls
            if (!_startFallingPositionY.HasValue && yVelocity < -0.01)
            {
                _startFallingPositionY = transform.position.y;
            }

            // if Hero fell and now is on the ground
            if (!_endFallingPositionY.HasValue && _startFallingPositionY.HasValue && yVelocity == 0)
            {
                _endFallingPositionY = transform.position.y;
            }

            if (_startFallingPositionY.HasValue && _endFallingPositionY.HasValue)
            {
                var delta = Mathf.Abs(_startFallingPositionY.Value - _endFallingPositionY.Value);

                if (delta >= longFallLength)
                {
                    particlesList.Spawn("LongFall");
                }

                _startFallingPositionY = null;
                _endFallingPositionY = null;
            }
        }

        protected override float CalculateYVelocity()
        {
            if (IsGrounded || _isOnStickyWall)
            {
                _isDoubleJumpAllowed = true;
            }

            if (_isOnStickyWall && !IsJumping())
            {
                return 0f;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsGrounded && _isDoubleJumpAllowed)
            {
                _isDoubleJumpAllowed = false;
                particlesList.Spawn("Jump");
                return jumpForce;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        public override void TakeDamage()
        {
            base.TakeDamage();

            SpawnCoins();
        }

        private void SpawnCoins()
        {
            if (CoinsCount <= 0) return;
        
            var coinsToDispose = Mathf.Min(CoinsCount, 5);
            _gameSession.PlayerData.Inventory.Remove("Coins", coinsToDispose);

            var burst = damageParticles.emission.GetBurst(0);
            burst.count = coinsToDispose;
            damageParticles.emission.SetBurst(0, burst);
            damageParticles.Play();
        }

        public void Interact()
        {
            var overlaps = interactionRange.GetOverlaps();
            foreach (var gameObj in overlaps)
            {
                var interactable = gameObj.GetComponent<InteractableComponent>();
            
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
        
        public void DropFromPlatform()
        {
            var position = transform.position;
            var bottomEndOfLinecast = position + new Vector3(0, -1);
            var resultOfLinecast = Physics2D.Linecast(position, bottomEndOfLinecast, groundLayer);

            if (resultOfLinecast.collider != null)
            {
                var disablePlatformComponent = resultOfLinecast.collider.GetComponent<DisableColliderForTimeComponent>();

                if (disablePlatformComponent != null)
                {
                    disablePlatformComponent.DisableCollider();
                }
            }
        }

        public override void TakeHeal()
        {
            if (HealthPoisonsCount > 0)
            {
                HealthComponent.ApplyHealthUpdate(_healByPoisonValue);
                _gameSession.PlayerData.Inventory.Remove("HealthPoisons", 1);
                base.TakeHeal();
            }
        }

        public void Throw(bool isLongThrow = false)
        {
            var isCoolDownReady = isLongThrow || throwCooldown.IsReady;
            if (SwordsCount <= 1 || !isCoolDownReady)
            {
                return;
            }

            Animator.SetTrigger(AnimatorThrown);
            _gameSession.PlayerData.Inventory.Remove("Swords", 1);
            throwCooldown.Reset();
        }

        // is used by event in animation "throw"
        public void PerformThrow()
        {
            particlesList.Spawn("Throw");
        }

        public void LongThrow()
        {
            if (SwordsCount <= 1)
            {
                return;
            }

            StartCoroutine(PerformLongThrow());
        }

        private IEnumerator PerformLongThrow()
        {
            Throw(true);
            yield return new WaitForSeconds(longThrowInterval);
            
            Throw(true);
            yield return new WaitForSeconds(longThrowInterval);
            
            Throw(true);
            yield return new WaitForSeconds(longThrowInterval);
        }
    }
}

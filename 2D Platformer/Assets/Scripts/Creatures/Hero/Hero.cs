using System;
using Components;
using Model;
using UnityEditor.Animations;
using UnityEngine;
using Utils;

namespace Creatures.Hero
{
    [RequireComponent(typeof(HeroCoinsInventory))]
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
    
        private HeroCoinsInventory _heroCoinsInventory;
        private bool _isOnStickyWall;
        private bool _isDoubleJumpAllowed;
        private float _defaultGravityScale;

        private float? _startFallingPositionY;
        private float? _endFallingPositionY;

        private GameSession _gameSession;

        protected override void Awake()
        {
            base.Awake();

            _heroCoinsInventory = GetComponent<HeroCoinsInventory>();
            _defaultGravityScale = Rigidbody.gravityScale;
        }

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
        
            HealthComponent.SetInitialHealth(_gameSession.LevelStartPlayerData.Health);
            _heroCoinsInventory.SetInitialCoins(_gameSession.LevelStartPlayerData.Coins);
            UpdateHeroWeapon(_gameSession.LevelStartPlayerData.IsArmed);
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
    
        public void OnCoinsChanged(int coins)
        {
            _gameSession.PlayerData.Coins = coins;
        }

        public override void Attack()
        {
            if (!_gameSession.PlayerData.IsArmed)
            {
                return;
            }

            base.Attack();
        }

        public void ArmHero()
        {
            _gameSession.PlayerData.IsArmed = true;
            UpdateHeroWeapon(_gameSession.PlayerData.IsArmed);
        }

        private void UpdateHeroWeapon(bool isArmed)
        {
            Animator.runtimeAnimatorController = isArmed ? armedAnimator : disarmedAnimator;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (LayerUtils.IsCollisionWithLayer(col, groundLayer))
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
        
            _isOnStickyWall = stickyWallsCheck.IsTouchingLayer;

            // player is on the wall and press moving arrows in the same direction where hero turned.
            // here we check sprite rotation to define hero turn direction.
            // we use Abs and tolerance value because of float numbers, they can lose precise in math operations
            if (_isOnStickyWall && Math.Abs(DirectionVector.x - transform.localScale.x) < 0.01)
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
            if (_heroCoinsInventory == null || _heroCoinsInventory.Coins <= 0) return;
        
            var coinsToDispose = Mathf.Min(_heroCoinsInventory.Coins, 5);
            _heroCoinsInventory.RemoveCoin(coinsToDispose);

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
    }
}

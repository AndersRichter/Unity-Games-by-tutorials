using Components;
using Model;
using UnityEditor.Animations;
using UnityEngine;
using Utils;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(HeroCoinsInventory))]
public class Hero : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float damageJumpForce;
    
    [Space] [Header("Ground check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private Vector3 groundCheckPositionDelta;
    [SerializeField] private float fallDamageVelocity;

    [Space] [Header("Attack")]
    [SerializeField] private GetCircleOverlapsComponent attackRange;
    [SerializeField] private int attackDamage;

    [Space] [Header("Animators")]
    [SerializeField] private AnimatorController _armedAnimator;
    [SerializeField] private AnimatorController _disarmedAnimator;

    [Space] [Header("Interaction check")]
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private float interactRadius;
    [SerializeField] private Vector3 interactPositionDelta;

    [Space] [Header("Particles")]
    [SerializeField] private SpawnComponent footStepsParticles;
    [SerializeField] private SpawnComponent jumpParticles;
    [SerializeField] private SpawnComponent longFallParticles;
    [SerializeField] private SpawnComponent swardAttack1Particles;
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private float longFallLength;

    [Space] [Header("Sticky Walls")]
    [SerializeField] private LayerCheckComponent stickyWallsCheck;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private HeroCoinsInventory _heroCoinsInventory;
    private HealthComponent _healthComponent;
    private Vector2 _directionVector;
    private bool _isGrounded;
    private bool _isJumping;
    private bool _isOnStickyWall;
    private bool _isDoubleJumpAllowed;
    private Collider2D[] _interactResults = new Collider2D[1];
    private int _lastPlayerDirectionX = 1;
    private float _defaultGravityScale;

    private float? _startFallingPositionY;
    private float? _endFallingPositionY;

    private static readonly int AnimatorIsGrounded = Animator.StringToHash("isGrounded");
    private static readonly int AnimatorIsRunning = Animator.StringToHash("isRunning");
    private static readonly int AnimatorVerticalVelocity = Animator.StringToHash("verticalVelocity");
    private static readonly int AnimatorDamageReceived = Animator.StringToHash("damageReceived");
    private static readonly int AnimatorHealReceived = Animator.StringToHash("healReceived");
    private static readonly int AnimatorIsAttacking = Animator.StringToHash("isAttacking");

    private GameSession _gameSession;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _heroCoinsInventory = GetComponent<HeroCoinsInventory>();
        _healthComponent = GetComponent<HealthComponent>();
        _defaultGravityScale = _rigidbody.gravityScale;
    }

    private void Start()
    {
        _gameSession = FindObjectOfType<GameSession>();
        
        _healthComponent.SetInitialHealth(_gameSession.LevelStartPlayerData.Health);
        _heroCoinsInventory.SetInitialCoins(_gameSession.LevelStartPlayerData.Coins);
        UpdateHeroWeapon(_gameSession.LevelStartPlayerData.IsArmed);
    }

    public void OnHealthChanged(int health)
    {
        _gameSession.PlayerData.Health = health;
    }
    
    public void OnCoinsChanged(int coins)
    {
        _gameSession.PlayerData.Coins = coins;
    }

    public void SetDirection(Vector2 directionVector)
    {
        _directionVector = directionVector;
    }

    public void Attack()
    {
        if (!_gameSession.PlayerData.IsArmed)
        {
            return;
        }
        _animator.SetTrigger(AnimatorIsAttacking);
        SpawnSwardAttack1Particles();
    }
    
    // is used by event in animation "attack"
    public void PerformAttack()
    {
        var overlaps = attackRange.GetOverlaps();
        foreach (var gameObj in overlaps)
        {
            var health = gameObj.GetComponent<HealthComponent>();
            var isPlayer = gameObj.CompareTag("Player");

            if (health != null && !isPlayer)
            {
                health.ApplyHealthUpdate(-attackDamage);
            }
        }
    }

    public void ArmHero()
    {
        _gameSession.PlayerData.IsArmed = true;
        UpdateHeroWeapon(_gameSession.PlayerData.IsArmed);
    }

    private void UpdateHeroWeapon(bool isArmed)
    {
        _animator.runtimeAnimatorController = isArmed ? _armedAnimator : _disarmedAnimator;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (LayerUtils.IsCollisionWithLayer(col, groundLayer))
        {
            var contact = col.contacts[0];
            if (contact.relativeVelocity.y >= fallDamageVelocity)
            {
                _healthComponent.ApplyHealthUpdate(-1);
            }
        }
    }

    private bool IsGrounded()
    {
        var hit = Physics2D.CircleCast(transform.position + groundCheckPositionDelta, groundCheckRadius, Vector2.down, 0, groundLayer);
        return hit.collider != null;
    }

    private bool IsJumping()
    {
        return _directionVector.y > 0;
    }

    private void OnDrawGizmos()
    {
        // Draw gizmo with jumping sphere
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position + groundCheckPositionDelta, groundCheckRadius);

        // Draw gizmo with interaction sphere
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawSphere(transform.position + interactPositionDelta, interactRadius);
    }

    private void Update()
    {
        _isGrounded = IsGrounded();
        
        HandleStickyWalls();
    }

    private void HandleStickyWalls()
    {
        if (!stickyWallsCheck) return;
        
        _isOnStickyWall = stickyWallsCheck.IsTouchingLayer;

        // player is on the wall and press moving arrows in the same direction where hero turned.
        // here we check sprite rotation to define hero turn direction
        if (_isOnStickyWall && _directionVector.x == transform.localScale.x)
        {
            _rigidbody.gravityScale = 0;
        }
        else
        {
            _rigidbody.gravityScale = _defaultGravityScale;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(CalculateXVelocity(), CalculateYVelocity());

        HandleLongFall();
        SetAnimatorProperties();
        UpdateSpriteDirection();
    }

    private void HandleLongFall()
    {
        var yVelocity = _rigidbody.velocity.y;
        
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
                SpawnLongFallParticles();
            }

            _startFallingPositionY = null;
            _endFallingPositionY = null;
        }
    }

    private float CalculateXVelocity()
    {
        return _directionVector.x * speed;
    }

    private float CalculateYVelocity()
    {
        var yVelocity = _rigidbody.velocity.y;
        
        if (_isGrounded)
        {
            _isDoubleJumpAllowed = true;
            _isJumping = false;
        }

        if (_isOnStickyWall)
        {
            _isDoubleJumpAllowed = true;
        }
        
        if (IsJumping())
        {
            _isJumping = true;
            yVelocity = CalculateJumpVelocity(yVelocity);
        }
        else if (_isOnStickyWall)
        {
            yVelocity = 0f;
        }
        else if (_rigidbody.velocity.y > 0 && _isJumping)
        {
            yVelocity *= 0.5f;
        }

        return yVelocity;
    }

    private float CalculateJumpVelocity(float yVelocity)
    {
        // 0.01 - fixed rare behavior when hero makes unexpected double jump, because raycast sphere is a little bit lower then hero body 
        var isFalling = _rigidbody.velocity.y <= 0.01f;

        if (!isFalling)
        {
            return yVelocity;
        }

        if (_isGrounded)
        {
            yVelocity += jumpForce;
            SpawnJumpParticles();
        }
        else if (_isDoubleJumpAllowed)
        {
            yVelocity = jumpForce;
            _isDoubleJumpAllowed = false;
            SpawnJumpParticles();
        }

        return yVelocity;
    }

    private void SetAnimatorProperties()
    {
        _animator.SetBool(AnimatorIsRunning, _directionVector.x != 0);
        _animator.SetBool(AnimatorIsGrounded, _isGrounded);
        _animator.SetFloat(AnimatorVerticalVelocity, _rigidbody.velocity.y);
    }

    private void UpdateSpriteDirection()
    {
        if (_directionVector.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            _lastPlayerDirectionX = 1;
        }
        else if (_directionVector.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            _lastPlayerDirectionX = -1;
        }
        else // = 0 - player stay without moving
        {
            transform.localScale = new Vector3(_lastPlayerDirectionX, 1, 1);
        }
    }

    public void TakeDamage()
    {
        _isJumping = false;
        _animator.SetTrigger(AnimatorDamageReceived);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, damageJumpForce);

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
    
    public void TakeHeal()
    {
        _animator.SetTrigger(AnimatorHealReceived);
    }

    public void Interact()
    {
        var interactionsArraySize = Physics2D.OverlapCircleNonAlloc(transform.position + interactPositionDelta, interactRadius, _interactResults, interactLayer);
        
        for (int i = 0; i < interactionsArraySize; i++)
        {
            var interactable = _interactResults[i].GetComponent<InteractableComponent>();
            
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    // is used by animation event
    public void SpawnFootStepsParticles()
    {
        footStepsParticles.Spawn();
    }
    
    private void SpawnJumpParticles()
    {
        jumpParticles.Spawn();
    }
    
    private void SpawnLongFallParticles()
    {
        longFallParticles.Spawn();
    }

    private void SpawnSwardAttack1Particles()
    {
        swardAttack1Particles.Spawn();
    }
}

using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameDirector gameDirector;
    public ControlStyle controlStyle;

    public float speed;

    public Rigidbody rb;
    public CameraHolder cameraHolder;
    public Transform cameraTransform;

    public float cameraSmoothTime;

    Vector3 velocity;

    public Vector2 turn;
    public float sensitivity;

    public float jumpPower;
    public LayerMask jumpLayerMask;

    float _tempSpeed;

    public AnimationState _animationState;

    private bool _isCrouching;

    private Animator _animator;

    private bool _hasRecentlyJumped;

    public float maxXRotation;
    public float minXRotation;

    public int startHealth;
    private int _currentHealth;
    public bool isDead;

    public Transform weaponHand;
    public Transform weaponContainer;

    public bool haveKey;

    public Weapon weapon;

    public void RestartPlayer()
    {
        gameObject.SetActive(true);
        _animator = GetComponentInChildren<Animator>();
        transform.position = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        _currentHealth = startHealth;
        isDead = false;
        gameDirector.healthBarUI.SetHealthBar(1);
        Camera.main.GetComponent<CameraCollision>().maxDistance = 3;

        weaponContainer.SetParent(weaponHand);
        weaponContainer.transform.localPosition = Vector3.zero;
        weaponContainer.transform.localRotation = Quaternion.identity;
        weaponContainer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        weaponContainer.GetComponentInChildren<BoxCollider>().enabled = false;
    }

    private void Update()
    {
        if (gameDirector.gameState != GameState.GamePlay || isDead)
        {
            return;
        }
        if (_animationState != AnimationState.Jump && rb.linearVelocity.y > 0) 
        {
            var vel = rb.linearVelocity;
            vel.y = 0;
            rb.linearVelocity = vel;
        }
        if (!_hasRecentlyJumped && GetIfLanded())
        {
            if (_animationState == AnimationState.Jump)
            {
                _animationState = AnimationState.Idle;
            }
        }
        else
        {
            if (_animationState != AnimationState.Jump)
            {
                _animationState = AnimationState.Jump;
                _animator.SetTrigger("JumpAir");
            }            
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_hasRecentlyJumped && GetIfLanded())
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpPower, rb.linearVelocity.z);
                _animator.SetTrigger("Jump");
                _animationState = AnimationState.Jump;
                _hasRecentlyJumped = true;
                Invoke(nameof(SetRecentlyJumpedFalse), .5f);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCrouch();
            _isCrouching = true;
            _tempSpeed = speed / 2f;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _isCrouching = false;
            StopCrouch();
        }
    }

    private void SetRecentlyJumpedFalse()
    {
        _hasRecentlyJumped = false;
    }

    private bool GetIfLanded()
    {
        return Physics.Raycast(transform.position + Vector3.up * .1f, Vector3.down, 1f, jumpLayerMask);
    }

    void FixedUpdate()
    {
        if (gameDirector.gameState != GameState.GamePlay || isDead)
        {
            return;
        }
        turn.x += Input.GetAxis("Mouse X"); 
        turn.y += Input.GetAxis("Mouse Y");

        transform.rotation = Quaternion.Euler(0, turn.x * sensitivity, 0);
        //var xRot = Mathf.Clamp(-turn.y * sensitivity, minXRotation, maxXRotation);

        if (-turn.y * sensitivity < minXRotation)
        {
            turn.y = -minXRotation / sensitivity;
        }
        else if (-turn.y * sensitivity > maxXRotation)
        {
            turn.y = -maxXRotation / sensitivity;
        }

       cameraHolder.transform.rotation = Quaternion.Euler(-turn.y * sensitivity, turn.x * sensitivity, 0);

        Vector3 direction = Vector3.zero;

        if (!_isCrouching)
        {
            _tempSpeed = speed;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _tempSpeed = speed * 2;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += -transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += -transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += transform.right;
        }

        _animator.SetFloat("WalkBlend", Vector3.SignedAngle(transform.forward, direction, Vector3.up) / 180f);

        if (direction == Vector3.zero)
        {
            if (_animationState != AnimationState.Idle && _animationState != AnimationState.Jump)
            {
                _animator.ResetTrigger("Walk");
                _animator.SetTrigger("Idle");
                _animationState = AnimationState.Idle;
            }
        }
        else
        {
            if (_animationState != AnimationState.Walk && _animationState != AnimationState.Jump)
            {
                _animator.ResetTrigger("Idle");
                _animator.SetTrigger("Walk");
                _animationState = AnimationState.Walk;
            }
        }
        var yVelocity = rb.linearVelocity;

        yVelocity.x = 0;
        yVelocity.z = 0;

        rb.linearVelocity = direction.normalized * _tempSpeed + yVelocity;

        cameraHolder.transform.position = Vector3.SmoothDamp(cameraHolder.transform.position,
            transform.position + Vector3.up * 2, ref velocity, cameraSmoothTime);
    }
    private void StartCrouch()
    {
        transform.DOKill();
        transform.DOScaleY(.5f, .3f);
    }

    private void StopCrouch()
    {
        transform.DOKill();
        transform.DOScaleY(1,.3f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            gameDirector.fXManager.PlayCoinCollectedFX(other.transform.position);
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("Key"))
        {
            other.gameObject.SetActive(false);
            haveKey = true;
        }
        if (other.CompareTag("Serum"))
        {
            gameDirector.LevelCompleted();
            other.gameObject.SetActive(false);

            _animator.ResetTrigger("Walk");
            _animator.SetTrigger("Idle");
            _animationState = AnimationState.Idle;
            rb.linearVelocity = Vector3.zero;
        }
        if (other.CompareTag("ShotgunCollectable"))
        {
            other.gameObject.SetActive(false);
            gameDirector.inventoryUI.isShotgunAvailable = true;
            gameDirector.messageUI.ShowMessage("SHOTGUN COLLECTED!");
        }
    }
    public void GetHit(int damage)
    {
        _currentHealth -= damage;
        gameDirector.healthBarUI.SetHealthBar((float)_currentHealth / startHealth);
        gameDirector.getHitUI.Show();
        if (_currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        _animator.SetTrigger("FallBack");
        _animationState = AnimationState.Dead;
        gameDirector.levelManager.StopEnemies();
        Camera.main.GetComponent<CameraCollision>().maxDistance = 10;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        Invoke(nameof(ReleaseWeapon), 1);
        Invoke(nameof(ShowFailUI), 2);
    }

    void ShowFailUI()
    {
        gameDirector.failUI.Show();
        gameDirector.levelUI.Hide();
        gameDirector.inventoryUI.Hide();
    }

    void ReleaseWeapon()
    {
        weaponContainer.SetParent(null);
        weaponContainer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        weaponContainer.GetComponent<Rigidbody>().linearVelocity = transform.forward * -5 + Vector3.up * 3;
        weaponContainer.GetComponent<Rigidbody>().angularVelocity
            = new Vector3(Random.Range(-10f, 10f),
            Random.Range(-10f, 10f),
            Random.Range(-10f, 10f));
        weaponContainer.GetComponentInChildren<BoxCollider>().enabled = true;
    }

    internal float GetHealthRatio()
    {
        return (float)_currentHealth / startHealth;
    }
}

public enum ControlStyle
{
    PlayerLooksToMouse,
    ThirdPerson,
    FPS,
}

public enum AnimationState
{
    Idle,
    Walk,
    Jump,
    Dead,
}
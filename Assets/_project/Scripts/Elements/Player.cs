using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ControlStyle controlStyle;

    public float speed;

    public Rigidbody rb;
    public Transform cameraHolder;
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

    public void RestartPlayer()
    {
        gameObject.SetActive(true);
        _animator = GetComponentInChildren<Animator>();
        transform.position = Vector3.zero;
    }

    private void Start()
    {
        if (controlStyle == ControlStyle.ThirdPerson)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Update()
    {
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
            if (GetIfLanded())
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
        if (controlStyle == ControlStyle.PlayerLooksToMouse)
        {
            Vector3 direction = Vector3.zero;

            _tempSpeed = speed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                _tempSpeed = speed * 2;
            }
            if (Input.GetKey(KeyCode.W))
            {
                direction += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                direction += Vector3.back;
            }
            if (Input.GetKey(KeyCode.A))
            {
                direction += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                direction += Vector3.right;
            }
            rb.position += direction.normalized * _tempSpeed * Time.fixedDeltaTime;
            cameraHolder.position = Vector3.SmoothDamp(cameraHolder.position,
            transform.position, ref velocity, cameraSmoothTime);
        }
        else if (controlStyle == ControlStyle.ThirdPerson)
        {
            turn.x += Input.GetAxis("Mouse X"); 
            turn.y += Input.GetAxis("Mouse Y");

            transform.rotation = Quaternion.Euler(0, turn.x * sensitivity, 0);
            cameraHolder.rotation = Quaternion.Euler(-turn.y * sensitivity, turn.x * sensitivity, 0);

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
                //_animator.SetFloat("WalkBlend", 0);
            }
            if (Input.GetKey(KeyCode.S))
            {
                direction += -transform.forward;
                //_animator.SetFloat("WalkBlend", .5f);
            }
            if (Input.GetKey(KeyCode.A))
            {
                direction += -transform.right;
                
                /*if (Input.GetKey(KeyCode.W))
                {
                    _animator.SetFloat("WalkBlend", .125f);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    _animator.SetFloat("WalkBlend", .375f);
                }
                else
                {
                    _animator.SetFloat("WalkBlend", .25f);
                }*/
            }
            if (Input.GetKey(KeyCode.D))
            {
                direction += transform.right;
                /*if (Input.GetKey(KeyCode.W))
                {
                    _animator.SetFloat("WalkBlend", .875f);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    _animator.SetFloat("WalkBlend", .675f);
                }
                else
                {
                    _animator.SetFloat("WalkBlend", .75f);
                }*/
            }

            print(Vector3.SignedAngle(transform.forward, direction, Vector3.up) / 180f);

            _animator.SetFloat("WalkBlend", Vector3.SignedAngle(transform.forward, direction, Vector3.up) / 180f);

            if (direction == Vector3.zero)
            {
                if (_animationState != AnimationState.Idle && _animationState != AnimationState.Jump)
                {
                    _animator.SetTrigger("Idle");
                    _animationState = AnimationState.Idle;
                }
            }
            else
            {
                if (_animationState != AnimationState.Walk && _animationState != AnimationState.Jump)
                {
                    _animator.SetTrigger("Walk");
                    _animationState = AnimationState.Walk;
                }
            }


            var yVelocity = rb.linearVelocity;

            yVelocity.x = 0;
            yVelocity.z = 0;

            rb.linearVelocity = direction.normalized * _tempSpeed + yVelocity;

            //rb.position += direction.normalized * _tempSpeed * Time.fixedDeltaTime;

            cameraHolder.position = Vector3.SmoothDamp(cameraHolder.position,
                transform.position, ref velocity, cameraSmoothTime);

            
        }
    }
    private void StartCrouch()
    {
        transform.DOKill();
        transform.DOScaleY(.5f, .3f);
    }

    private void StopCrouch()
    {
        transform.DOKill();
        transform.DOScaleY(1, .3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            other.gameObject.SetActive(false);
        }
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
    Jump
}
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int startHealth;
    private int _currentHealth;

    public int damage;

    public float playerDetectDistance;
    public float attackDistance;
    public float speed;

    private Player _player;
    private bool _didSeePlayer;
    private bool _isAttacking;
    private Animator _animator;

    public EnemyState enemyState;

    public HealthBar healthBar;

    private Coroutine _attackCoroutine;

    public void StartEnemy(Player player)
    {
        _currentHealth = startHealth;
        _player = player;
        _animator = GetComponentInChildren<Animator>();
        enemyState = EnemyState.Idle;
        healthBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (enemyState == EnemyState.Dead || _player.isDead)
        {
            return;
        }

        if (!_isAttacking && (_player.transform.position - transform.position).magnitude < attackDistance)
        {
            _isAttacking = true;
            _animator.SetTrigger("Attack");
            enemyState = EnemyState.Attacking;
            _attackCoroutine = StartCoroutine(AttackCoroutine());
        }
        else if (!_isAttacking)
        {
            if (!_didSeePlayer && (_player.transform.position - transform.position).magnitude < playerDetectDistance)
            {
                _didSeePlayer = true;
                _animator.SetTrigger("Walk");
                StartCoroutine(ZombieSoundCoroutine());
            }

            if (_didSeePlayer)
            {
                enemyState = EnemyState.Walking;
                MoveToPlayer();
            }
        }
        
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(1.3f);
        if ((_player.transform.position - transform.position).magnitude < attackDistance)
        {
            _player.GetHit(1);
            _player.gameDirector.audioManager.PlayPlayerHitAS();
        }
        yield return new WaitForSeconds(.8f);
        if (!_player.isDead)
        {
            enemyState = EnemyState.Walking;
            _animator.SetTrigger("Walk");
        }
        else
        {
            enemyState = EnemyState.Idle;
            _animator.SetTrigger("Idle");
        }
        _didSeePlayer = false;
        _isAttacking = false;
    }

    IEnumerator ZombieSoundCoroutine()
    {
        while (enemyState != EnemyState.Dead)
        {
            _player.gameDirector.audioManager.PlayZombieAlertAS();
            yield return new WaitForSeconds(8);
        }
    }

    private void MoveToPlayer()
    {
        var dir = (_player.transform.position - transform.position).normalized;
        dir.y = 0;
        transform.position += dir * Time.deltaTime * speed;
        transform.LookAt(transform.position + dir);
    }

    public void GetHit(int damage)
    {
        _currentHealth -= damage;
        healthBar.gameObject.SetActive(true);
        healthBar.UpdateHealthBar((float)_currentHealth / startHealth);
        if (_currentHealth <= 0)
        {
            Die();
            healthBar.gameObject.SetActive(false);
        }
    }

    private void Die()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }
        enemyState = EnemyState.Dead;
        if (UnityEngine.Random.value < .5f)
        {
            _animator.SetTrigger("FallBack1");
        }
        else
        {
            _animator.SetTrigger("FallBack2");
        }
        Invoke(nameof(DisableColldiers), 2f);
        Destroy(gameObject, 4f);
    }
    private void DisableColldiers()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        foreach (var c in GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }
    }

    public void StopEnemy()
    {
        enemyState = EnemyState.Idle;
        _animator.SetTrigger("Idle");
        speed = 0;
    }
}

public enum EnemyState
{
    Idle,
    Walking,
    Attacking,
    Dead,
}
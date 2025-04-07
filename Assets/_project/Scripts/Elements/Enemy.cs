using DG.Tweening;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int startHealth;
    private int _currentHealth;

    public int damage;

    public float playerDetectDistance;
    public float speed;

    private Player _player;
    private bool _didSeePlayer;
    private Animator _animator;

    public void StartEnemy(Player player)
    {
        _currentHealth = startHealth;
        _player = player;
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!_didSeePlayer && (_player.transform.position - transform.position).magnitude < playerDetectDistance)
        {
            _didSeePlayer = true;
            _animator.SetTrigger("Walk");
        }

        if (_didSeePlayer)
        {
            MoveToPlayer();
        }
    }

    private void MoveToPlayer()
    {
        var dir = (_player.transform.position - transform.position).normalized;
        transform.position += dir * Time.deltaTime * speed;
        transform.LookAt(transform.position + dir);
    }

    public void GetHit(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}

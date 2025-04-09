using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Player _player;
    public float speed;
    public int damage;
    private bool _isBulletStarted;
    private Vector3 _direction;
    private Vector3 _startPos;
    private float _maxDistance;
    public void StartBullet(Player player, Vector3 dir, float maxDistance)
    {
        _player = player;
        _isBulletStarted = true;
        _direction = dir;
        _startPos = transform.position;
        _maxDistance = maxDistance;
    }

    private void Update()
    {
        if (_isBulletStarted)
        {
            transform.position += _direction * Time.deltaTime * speed;

            if((transform.position - _startPos).magnitude > _maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            _player.gameDirector.fXManager.PlayBuleltHitFX(transform.position);
            Destroy(gameObject);
        }
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponentInParent<Enemy>();
            if (enemy.enemyState != EnemyState.Dead)
            {
                enemy.GetHit(damage);
                var dir = transform.position - enemy.transform.position;
                dir.y = 0;
                _player.gameDirector.fXManager.PlayZombieHitFX(transform.position, dir);
                gameObject.SetActive(false);
                Destroy(gameObject);
            }           
        }
    }
}

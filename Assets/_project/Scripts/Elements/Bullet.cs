using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private bool _isBulletStarted;
    private Vector3 _direction;
    private Vector3 _startPos;
    private float _maxDistance;
    public void StartBullet(Vector3 dir, float maxDistance)
    {
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
}

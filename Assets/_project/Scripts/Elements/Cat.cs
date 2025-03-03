using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public Transform wayPoint;

    public Player player;

    public float walkDuration;

    public float threshold;
    public float speed;

    private Vector3 _startPoint;
    private bool _isLookingToStart;

    private bool _isWalking;
    public Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }
    /*private void Start()
    {
        _startPoint = transform.position;
        transform.DOMove(wayPoint.position, walkDuration).
            SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).OnStepComplete(ChangeLookPosition);
        transform.DOMove(wayPoint.position, walkDuration).
            SetEase(Ease.Linear).OnStepComplete(ChangeLookPosition);
    }*/
    private void Update()
    {
        var distance = Vector3.Distance(player.transform.position, transform.position);
        var direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;
        if (distance < threshold)
        {

            if (!_isWalking)
            {
                _animator.SetTrigger("Walk");
                _isWalking = true;
            }
            transform.position += direction * (speed * Time.deltaTime);
            transform.LookAt(transform.position + direction);
        }
    }
    void ChangeLookPosition()
    {
        if (_isLookingToStart)
        {
            _isLookingToStart = false;
            transform.LookAt(wayPoint.position);
        }
        else
        {
            _isLookingToStart = true;
            transform.LookAt(_startPoint);
        }
    }
}

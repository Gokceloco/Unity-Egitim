using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isMovingUpDown;

    private void Start()
    {
        if (isMovingUpDown)
        {
            transform.DOMoveY(0, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCirc);
        }
    }
}

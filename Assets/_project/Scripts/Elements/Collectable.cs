using DG.Tweening;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int bonusAmount;

    public float rotationSpeed;
    public void StartCollectable(float delay)
    {
        transform.DORotate(180 * Vector3.up, 2).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetDelay(delay);
        transform.DOMoveY(transform.position.y + 1, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad).SetDelay(delay);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}

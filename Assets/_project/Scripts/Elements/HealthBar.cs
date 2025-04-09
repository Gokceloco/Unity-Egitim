using DG.Tweening;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Color flashColor;
    public Color healthBarColor;

    public Transform fillBarParent;
    public SpriteRenderer fillBar;

    private void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }
    public void UpdateHealthBar(float ratio)
    {
        fillBarParent.DOKill();
        fillBarParent.DOScaleX(ratio, .2f);

        fillBar.DOKill();
        fillBar.color = healthBarColor;
        fillBar.DOColor(flashColor, .05f).SetLoops(2, LoopType.Yoyo);
    }
}

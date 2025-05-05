using DG.Tweening;
using UnityEngine;

public class GetHitUI : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1f, .2f);
        Hide(.2f);
    }
    public void Hide(float delay)
    {
        _canvasGroup.DOFade(0, .2f).SetDelay(delay)
            .OnComplete(() => gameObject.SetActive(false));
    }
}

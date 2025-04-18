using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    public Image healthBar;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1, .2f);
    }
    public void Hide()
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(0, .2f).OnComplete(() => gameObject.SetActive(false));
    }

    public void SetHealthBar(float ratio)
    {
        healthBar.fillAmount = ratio;
        if(healthBar.fillAmount < .01f)
        {
            Hide();
        }
    }
}

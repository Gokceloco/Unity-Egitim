using DG.Tweening;
using TMPro;
using UnityEngine;

public class MessageUI : MonoBehaviour
{
    public TextMeshProUGUI messageTMP;

    private CanvasGroup _canvasGroup;

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
    public void Hide(float delay)
    {        
        _canvasGroup.DOFade(0, .2f).SetDelay(delay)
            .OnComplete(() => gameObject.SetActive(false));
    }
    public void ShowMessage(string msg)
    {
        Show();
        messageTMP.text = msg;
        Hide(2f);

    }
}

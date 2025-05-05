using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    public TextMeshProUGUI levelTMP;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show(int levelNo)
    {
        gameObject.SetActive(true);
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1f, .2f).SetUpdate(true);
        SetLevelNo(levelNo);
    }
    public void Hide()
    {
        _canvasGroup.DOFade(0, .2f).SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
    }
    public void SetLevelNo(int levelNo)
    {
        levelTMP.text = "Level " + levelNo;
    }
}

using DG.Tweening;
using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    public GameDirector gameDirector;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show()
    {
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1f, .2f);
    }
    public void Hide()
    {
        _canvasGroup.DOFade(0, .2f).OnComplete(() => gameObject.SetActive(false));
    }

    public void LoadNextLevelButtonPressed()
    {
        gameDirector.LoadNextLevel();
        Hide();
    }
}

using DG.Tweening;
using UnityEngine;

public class FailUI : MonoBehaviour
{
    public GameDirector gameDirector;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show()
    {
        gameDirector.gameState = GameState.FailUI;
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1f, .2f);
    }
    public void Hide()
    {
        _canvasGroup.DOFade(0, .2f).OnComplete(() => gameObject.SetActive(false));
    }

    public void RestartButtonPressed()
    {
        Hide();
        gameDirector.RestartLevel();
    }
}

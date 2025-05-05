using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public GameDirector gameDirector;
    private CanvasGroup _canvasGroup;
    public RectTransform imageRectTransform;

    public SaveUI saveUI;
    public LoadUI loadUI;

    Vector2 vel;
    public float smooth;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show()
    {
        gameDirector.levelUI.Hide();
        gameObject.SetActive(true);
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1, .2f).SetUpdate(true);
    }

    private void Update()
    {
        if (gameDirector.gameState == GameState.MainMenu)
        {
            var targetPos = new Vector2(0 + Input.mousePosition.x * .1f, 5 + Input.mousePosition.y * .1f);
            imageRectTransform.anchoredPosition = Vector2.SmoothDamp(imageRectTransform.anchoredPosition,
                targetPos, ref vel, smooth);
        }
    }

    public void Hide()
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(0, .2f).SetUpdate(true).OnComplete(()=>gameObject.SetActive(false));
    }
    public void PlayButtonPressed()
    {
        Hide();
        gameDirector.RestartLevel();
    }

    public void SaveButtonPressed()
    {
        saveUI.Show();
    }

    public void LoadButtonPressed()
    {
        var files = SaveSystem.GetLoadFiles();
        loadUI.Show(files);
        foreach (var f in files)
        {
            print(f);
        }
    }

    public void QuitButtonPressed()
    {
        Application.Quit();
    }
}

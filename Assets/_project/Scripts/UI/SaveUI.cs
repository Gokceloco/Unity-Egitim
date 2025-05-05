using DG.Tweening;
using TMPro;
using UnityEngine;

public class SaveUI : MonoBehaviour
{
    public GameDirector gameDirector;
    private CanvasGroup _canvasGroup;

    public TMP_InputField nameInput;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOKill();
        //_canvasGroup.DOFade(1f, .2f);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        //_canvasGroup.DOFade(0, .2f).OnComplete(() => gameObject.SetActive(false));
    }

    public void SaveButtonPressed()
    {
        SaveSystem.SaveData(nameInput.text, gameDirector.levelManager.levelNo,
            gameDirector.player.GetHealthRatio(),
            gameDirector.player.transform.position, false);
        Hide();
    }
}

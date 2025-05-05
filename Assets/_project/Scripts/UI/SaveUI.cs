using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveUI : MonoBehaviour
{
    public GameDirector gameDirector;
    private CanvasGroup _canvasGroup;

    public TMP_InputField nameField;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1, .2f).SetUpdate(true);
    }
    public void Hide()
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(0, .2f).SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
    }

    public void SaveButtonPressed()
    {
        SaveSystem.SaveData(nameField.text, gameDirector.levelManager.levelNo,
            gameDirector.player.GetHealthRatio(),
            gameDirector.player.transform.position);
        Hide();
    }
}

using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class LoadUI : MonoBehaviour
{
    public GameDirector gameDirector;
    private CanvasGroup _canvasGroup;

    public TMP_Dropdown dropDown;

    private string[] _files;

    private string _selectedFile;

    public MainMenuUI mainMenuUI;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show(string[] files)
    {
        gameObject.SetActive(true);
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1f,  .2f);
        _files = files;
        _selectedFile = _files[0];
        SetDropDownMenu();
    }

    private void SetDropDownMenu()
    {
        dropDown.ClearOptions();
        foreach (var f in _files)
        {
            dropDown.options.Add(new TMP_Dropdown.OptionData(f));
        }
        dropDown.RefreshShownValue();
    }

    public void ChangeSelectedFile(int value)
    {
        _selectedFile = _files[value];
    }

    public void Hide()
    {
        _canvasGroup.DOFade(0, .2f).OnComplete(() => gameObject.SetActive(false));
    }

    public void LoadButtonPressed()
    {
        var data = SaveSystem.LoadData(_selectedFile);
        gameDirector.levelManager.levelNo = data.lastLevelNo;
        gameDirector.RestartLevel();
        var pos = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);
        gameDirector.player.transform.position = pos;
        mainMenuUI.Hide();
        Hide();
    }
}

using System;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public Player player;
    public GameState gameState;

    [Header("Managers")]
    public LevelManager levelManager;
    public FXManager fXManager;
    public AudioManager audioManager;

    [Header("UI")]
    public MainMenuUI mainMenuUI;
    public HealthBarUI healthBarUI;
    public MessageUI messageUI;
    public GetHitUI getHitUI;
    public FailUI failUI;
    public VictoryUI victoryUI;
    public LevelUI levelUI;
    public InventoryUI inventoryUI;

    public bool inventoryUIVisible;

    private void Start()
    {
        mainMenuUI.Show();
        failUI.Hide();
        victoryUI.Hide();
        var data = SaveSystem.LoadData("start");
        if (data != null)
        {
            levelManager.levelNo = data.level;
        }
    }


    private void Update()
    {
        if (gameState == GameState.GamePlay && Input.GetKeyDown(KeyCode.Escape))
        {
            inventoryUI.Hide();
            inventoryUIVisible = false;
            Cursor.lockState = CursorLockMode.None;
            gameState = GameState.MainMenu;
            mainMenuUI.Show();
            healthBarUI.Hide();
            Time.timeScale = 0;            
        }
        if ((gameState == GameState.GamePlay || gameState == GameState.InventoryUI) 
            && Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUIVisible)
            {
                inventoryUI.Hide();
                inventoryUIVisible = false;
            }
            else
            {
                inventoryUI.Show();
                inventoryUIVisible = true;
            }
        }

        if (gameState == GameState.GamePlay && Input.GetKeyDown(KeyCode.R))
        {
            ResetGameData();
        }
    }

    public void LoadNextLevel()
    {
        if (levelManager.levelNo < levelManager.levels.Count)
        {
            levelManager.levelNo += 1;
        }
        RestartLevel();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        gameState = GameState.GamePlay;
        player.RestartPlayer();
        levelManager.DeleteCurrentLevel();
        levelManager.CreateLevel();
        healthBarUI.Show();
        levelUI.Show(levelManager.levelNo);

        if (levelManager.levelNo == 1)
        {
            messageUI.ShowMessage("FIND THE SERUM");

            Invoke(nameof(ShowTutorialMessage), 3f);
        }
        inventoryUI.Hide();
    }

    void ShowTutorialMessage()
    {
        messageUI.ShowMessage("HIT W TO MOVE");
    }

    public void LevelCompleted()
    {
        victoryUI.Show();
        gameState = GameState.VictoryUI;
        inventoryUI.Hide();
        SaveSystem.SaveData("start", levelManager.levelNo + 1, 1, Vector3.zero);
    }

    void ResetGameData()
    {
        SaveSystem.SaveData("start", 1, 1, Vector3.zero);
        levelManager.levelNo = 1;
        RestartLevel();
    }
}
public enum GameState
{
    MainMenu,
    VictoryUI,
    FailUI,
    GamePlay,
    InventoryUI,
}
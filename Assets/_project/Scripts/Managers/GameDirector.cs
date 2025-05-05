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
        levelManager.levelNo = data.lastLevelNo;
        if (levelManager.levelNo < 1)
        {
            levelManager.levelNo = 1;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState == GameState.GamePlay)
            {
                inventoryUI.Hide();
                inventoryUIVisible = false;
                Cursor.lockState = CursorLockMode.None;
                gameState = GameState.MainMenu;
                mainMenuUI.Show();
                healthBarUI.Hide();
                Time.timeScale = 0;
            }
            else if (gameState == GameState.MainMenu)
            {
                mainMenuUI.Hide();
            }
            else if (gameState == GameState.LoadMenu)
            {
                mainMenuUI.loadUI.Hide();
                gameState = GameState.MainMenu;
            }
            else if (gameState == GameState.SaveMenu)
            {
                mainMenuUI.saveUI.Hide();
                gameState = GameState.MainMenu;
            }
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
        inventoryUI.Hide();
        victoryUI.Show();
        gameState = GameState.VictoryUI;
        levelUI.Hide();
        SaveSystem.SaveData("start", levelManager.levelNo+1, 1, Vector3.zero, false);
    }
}
public enum GameState
{
    MainMenu,
    VictoryUI,
    FailUI,
    GamePlay,
    InventoryUI,
    LoadMenu,
    SaveMenu,
}
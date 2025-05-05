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

        levelManager.levelNo = PlayerPrefs.GetInt("LastLevelNo");
        if (levelManager.levelNo < 1)
        {
            levelManager.levelNo = 1;
        }
        /*var data = SaveSystem.LoadData("start");
        levelManager.levelNo = data.lastLevelNo;*/
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

        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (levelManager.levelNo > 0)
            {
                levelManager.levelNo -= 1;
            }
            RestartLevel();
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
        //SaveSystem.SaveData("start", levelManager.levelNo+1, 1, Vector3.zero);
        PlayerPrefs.SetInt("LastLevel", levelManager.levelNo);
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
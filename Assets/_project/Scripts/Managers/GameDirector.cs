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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            gameState = GameState.MainMenu;
            mainMenuUI.Show();
            healthBarUI.Hide();
        }
    }

    public void RestartLevel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gameState = GameState.GamePlay;
        player.RestartPlayer();
        levelManager.DeleteCurrentLevel();
        levelManager.CreateLevel();
        healthBarUI.Show();
    }
}
public enum GameState
{
    MainMenu,
    VictoryUI,
    FailUI,
    GamePlay,
}
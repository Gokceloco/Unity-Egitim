using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public LevelManager levelManager;
    public FXManager fXManager;
    public Player player;

    private void Start()
    {
        RestartLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    private void RestartLevel()
    {
        player.RestartPlayer();
        levelManager.DeleteCurrentLevel();
        levelManager.CreateLevel();
    }
}

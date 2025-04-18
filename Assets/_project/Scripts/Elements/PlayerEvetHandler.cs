using UnityEngine;

public class PlayerEvetHandler : MonoBehaviour
{
    public Player player;
    public void PlayFootStep()
    {
        player.gameDirector.audioManager.PlayFootStepSound();
    }
}

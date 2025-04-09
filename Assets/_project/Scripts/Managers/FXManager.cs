using UnityEngine;

public class FXManager : MonoBehaviour
{
    public GameDirector gameDirector;
    public ParticleSystem coinCollectedPS;
    public ParticleSystem bulletHitPS;
    public ParticleSystem zombieHitPS;
    public void PlayCoinCollectedFX(Vector3 pos)
    {
        var newPS = Instantiate(coinCollectedPS);
        newPS.transform.position = pos;
        newPS.Play();
    }
    public void PlayBuleltHitFX(Vector3 pos)
    {
        var newPS = Instantiate(bulletHitPS);
        newPS.transform.position = pos;
        gameDirector.audioManager.PlayMetalHitAS();
        newPS.Play();
    }
    public void PlayZombieHitFX(Vector3 pos, Vector3 direction)
    {
        var newPS = Instantiate(zombieHitPS);
        newPS.transform.position = pos;
        newPS.transform.LookAt(pos + direction);
        gameDirector.audioManager.PlayZombieHitAS();
        newPS.Play();
    }
}

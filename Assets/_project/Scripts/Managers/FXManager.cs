using UnityEngine;

public class FXManager : MonoBehaviour
{
    public ParticleSystem coinCollectedPS;
    public ParticleSystem bulletHitPS;
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
        newPS.Play();
    }
}

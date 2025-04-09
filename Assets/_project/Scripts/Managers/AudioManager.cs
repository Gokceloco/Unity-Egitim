using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource shootAS;
    public AudioSource shotgunShootAS;
    public AudioSource metalHitAS;
    public AudioSource zombieAlertAS;
    public AudioSource zombieAlertAS2;
    public AudioSource zombieHitAS;

    public AudioSource ambianceAS;

    public void PlayShootAS()
    {
        shootAS.Play();
    }
    public void PlayShotgunShootAS()
    {
        shotgunShootAS.Play();
    }
    public void PlayMetalHitAS()
    {
        metalHitAS.Play();
    }
    public void PlayZombieHitAS()
    {
        zombieHitAS.Play();
    }
    public void PlayZombieAlertAS()
    {
        if (Random.value < .5f)
        {
            zombieAlertAS.Play();
        }
        else
        {
            zombieAlertAS2.Play();
        }
    }

    public void PlayAmbianceSound1()
    {
        ambianceAS.Play();
    }
    public void StopAmbianceSound1()
    {
        ambianceAS.Stop();
    }
}

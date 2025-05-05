using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Player _player;
    public Bullet bulletPrefab;

    public Transform shootPos;

    public WeaponType weaponType;

    public float machinegunAttackRate;
    public float shotgunAttackRate;

    public int shotgunBulletCount;

    public float machinegunSpread;
    public float shotgunSpread;

    public float machinegunMaxDistance;
    public float shotgunMaxDistance;

    private float _lastShootTime;

    public GameObject machinegunMesh;
    public GameObject shotgunMesh;

    public ParticleSystem muzzlePS;

    public Light muzzleLight;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        if (_player.gameDirector.gameState != GameState.GamePlay)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateMachineGun();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateShotgun();
        }
        if (weaponType == WeaponType.Machinegun
            && Input.GetMouseButton(0)
            && Time.time - _lastShootTime > machinegunAttackRate)
        {
            Shoot(machinegunSpread, machinegunMaxDistance);
            _player.gameDirector.audioManager.PlayShootAS();
            _player.cameraHolder.ShakeCamera(.15f, .05f);
        }
        else if (weaponType == WeaponType.Shotgun
            && Input.GetMouseButtonUp(0)
            && Time.time - _lastShootTime > shotgunAttackRate)
        {
            _player.transform.DOMove(_player.transform.position - _player.transform.forward*1, .05f);
            for (int i = 0; i < shotgunBulletCount; i++)
            {
                Shoot(shotgunSpread, shotgunMaxDistance);
            }
            _player.gameDirector.audioManager.PlayShotgunShootAS();
            _player.cameraHolder.ShakeCamera(.25f, .15f);
        }
    }

    public void ActivateShotgun()
    {
        weaponType = WeaponType.Shotgun;
        machinegunMesh.SetActive(false);
        shotgunMesh.SetActive(true);
    }

    public void ActivateMachineGun()
    {
        weaponType = WeaponType.Machinegun;
        machinegunMesh.SetActive(true);
        shotgunMesh.SetActive(false);
    }

    public void Shoot(float spread, float maxDistance)
    {
        var newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = shootPos.position;
        newBullet.transform.rotation = shootPos.rotation;
        _player.gameDirector.levelManager.SetParentToMap(newBullet.transform);

        Vector3 s = Vector3.zero;

        if (weaponType == WeaponType.Machinegun)
        {
            s = _player.transform.right * Random.Range(-spread, spread)
            + Vector3.up * Random.Range(-spread, spread);
            
        }
        else if(weaponType == WeaponType.Shotgun)
        {
            s = _player.transform.right * Random.Range(-spread, spread)
            + Vector3.up * Random.Range(-spread/2f, spread);
        }

        var bulletDirection = _player.transform.forward + s;

        newBullet.transform.LookAt(newBullet.transform.position + bulletDirection);

        newBullet.StartBullet(_player, bulletDirection, maxDistance);
        _lastShootTime = Time.time;
        muzzlePS.Play();


        muzzleLight.DOKill();
        muzzleLight.intensity = 0;
        muzzleLight.DOIntensity(100, .05f).SetLoops(2, LoopType.Yoyo);
    }
}

public enum WeaponType
{
    Machinegun,
    Shotgun,
}
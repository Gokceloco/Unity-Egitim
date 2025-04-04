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

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponType = WeaponType.Machinegun;
            machinegunMesh.SetActive(true);
            shotgunMesh.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponType = WeaponType.Shotgun;
            machinegunMesh.SetActive(false);
            shotgunMesh.SetActive(true);
        }
        if (weaponType == WeaponType.Machinegun
            && Input.GetMouseButton(0)
            && Time.time - _lastShootTime > machinegunAttackRate)
        {
            Shoot(machinegunSpread, machinegunMaxDistance);
        }
        else if (weaponType == WeaponType.Shotgun
            && Input.GetMouseButtonUp(0)
            && Time.time - _lastShootTime > shotgunAttackRate)
        {
            _player.transform.DOMove(_player.transform.position - _player.transform.forward*2, .05f);
            for (int i = 0; i < shotgunBulletCount; i++)
            {
                Shoot(shotgunSpread, shotgunMaxDistance);
            }
        }
    }


    public void Shoot(float spread, float maxDistance)
    {
        var newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = shootPos.position;
        newBullet.transform.rotation = shootPos.rotation;
        _player.gameDirector.levelManager.SetParentToMap(newBullet.transform);

        var bulletDirection = _player.transform.forward
            + _player.transform.right * Random.Range(-spread, spread)
            + Vector3.up * Random.Range(-spread, spread);

        newBullet.transform.LookAt(transform.position + bulletDirection);

        newBullet.StartBullet(_player, bulletDirection, maxDistance);
        _lastShootTime = Time.time;
    }
}

public enum WeaponType
{
    Machinegun,
    Shotgun,
}
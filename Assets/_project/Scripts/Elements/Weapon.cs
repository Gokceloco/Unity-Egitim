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

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }

    private void Update()
    {
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

        newBullet.StartBullet(bulletDirection, maxDistance);
        _lastShootTime = Time.time;
    }
}

public enum WeaponType
{
    Machinegun,
    Shotgun,
}
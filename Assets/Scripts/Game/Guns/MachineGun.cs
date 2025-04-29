using UnityEngine;
using System.Collections;

public class MachineGun : Gun
{
    [Header("Bullet Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;

    [Header("Firing Settings")]
    [SerializeField] private float spreadAngle = 5f; // degrees (randomized)
    [SerializeField] private float fireRate = 0.05f; // time between bullets

    [Header("Ammo Settings")]
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private float reloadTime = 2f;

    private int currentAmmo;
    private bool isReloading = false;
    private float _lastShotTime;

    private void Awake()
    {
        currentAmmo = maxAmmo;
    }

    public override void Fire(Vector2 targetPosition)
    {
        if (firePoint == null) return;
        if (isReloading) return;

        // Enforce fire rate limit
        if (Time.time - _lastShotTime < fireRate)
            return;

        if (currentAmmo <= 0)
        {
            PlayerShoot.Instance.StartReloadCoroutine(this);
            return;
        }

        _lastShotTime = Time.time;

        // Add random spread
        Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;
        float randomSpread = Random.Range(-spreadAngle, spreadAngle);
        Vector2 spreadDirection = Quaternion.Euler(0, 0, randomSpread) * direction;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = spreadDirection * bulletSpeed;

        float rotZ = Mathf.Atan2(spreadDirection.y, spreadDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90f);

        currentAmmo--;
    }

    public override IEnumerator Reload()
    {
        if (isReloading) yield break;

        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}

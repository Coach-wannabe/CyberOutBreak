using UnityEngine;
using System.Collections;

public class Shotgun : Gun
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private int pelletsPerShot = 5;
    [SerializeField] private float spreadAngle = 15f;

    [SerializeField] private int maxAmmo = 6;
    [SerializeField] private float reloadTime = 2f;

    private int currentAmmo;
    private bool isReloading = false;

    private void Awake()
    {
        currentAmmo = maxAmmo;
    }

    public override void Fire(Vector2 targetPosition)
    {
        if (firePoint == null) return;
        if (isReloading) return;

        if (currentAmmo <= 0)
        {
            PlayerShoot.Instance.StartReloadCoroutine(this);
            return;
        }

        Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;

        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (pelletsPerShot - 1);

        for (int i = 0; i < pelletsPerShot; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector2 spreadDir = Quaternion.Euler(0, 0, angle) * direction;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = spreadDir * bulletSpeed;

            float rotZ = Mathf.Atan2(spreadDir.y, spreadDir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90f);
        }

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

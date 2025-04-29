using UnityEngine;
using System.Collections;

public class MachineGun : Gun
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;

    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private float reloadTime = 3f;

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

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletSpeed;

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
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

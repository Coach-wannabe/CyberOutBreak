using UnityEngine;

public class BasicGun : Gun
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 8f;

    public override void Fire(Vector2 targetPosition)
    {
        if (firePoint == null)
        {
            Debug.LogWarning("FirePoint not assigned on BasicGun.");
            return;
        }

        Vector2 fireDirection = (targetPosition - (Vector2)firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = fireDirection * bulletSpeed;

        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}

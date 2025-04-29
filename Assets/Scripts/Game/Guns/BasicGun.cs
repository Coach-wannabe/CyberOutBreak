using UnityEngine;

public class BasicGun : Gun
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float fireCooldown = 0.4f; // feels like semi-auto

    private float _lastFireTime;

    public override void Fire(Vector2 targetPosition)
    {
        if (firePoint == null) return;

        if (Time.time - _lastFireTime < fireCooldown)
            return;

        _lastFireTime = Time.time;

        Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletSpeed;

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90f);
    }
}

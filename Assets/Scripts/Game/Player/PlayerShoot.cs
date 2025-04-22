using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Transform _gunOffset;
    [SerializeField] private float _timeBetweenShots;

    private bool _fireContinuously;
    private float _lastFireTime;

    void Update()
    {
        // Only fire if holding fire button AND enough time has passed
        if (_fireContinuously && Time.time - _lastFireTime >= _timeBetweenShots)
        {
            FireBullet();
            _lastFireTime = Time.time;
        }
    }

    private void FireBullet()
    {
        if (_gunOffset == null)
        {
            Debug.LogWarning("Gun offset not assigned.");
            return;
        }

        GameObject bullet = Instantiate(_bulletPrefab, _gunOffset.position, transform.rotation);
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
        rigidbody.linearVelocity = _bulletSpeed * transform.up;
    }

    public void OnAttack(InputValue inputValue)
    {
        _fireContinuously = inputValue.isPressed;

        if (_fireContinuously)
        {
            FireBullet();
            _lastFireTime = Time.time;
        }
    }
}

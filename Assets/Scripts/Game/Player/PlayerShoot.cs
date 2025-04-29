using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Gun equippedGun;
    [SerializeField] private float timeBetweenShots = 0.5f;

    private CrosshairController _crosshair;
    private float _lastFireTime;
    private bool _isFiring;

    private void Awake()
    {
        _crosshair = FindObjectOfType<CrosshairController>();
    }

    private void Update()
    {
        if (_isFiring && Time.time - _lastFireTime >= timeBetweenShots)
        {
            if (_crosshair != null && equippedGun != null)
            {
                equippedGun.Fire(_crosshair.transform.position);
                _lastFireTime = Time.time;
            }
        }
    }

    public void OnAttack(InputValue inputValue)
    {
        _isFiring = inputValue.isPressed;

        if (_isFiring)
        {
            _lastFireTime = Time.time - timeBetweenShots; // fire immediately
        }
    }
}

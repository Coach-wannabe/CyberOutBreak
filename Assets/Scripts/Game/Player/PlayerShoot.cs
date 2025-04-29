using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Gun[] allGuns;                         // Assign scene gun objects in order: Basic, Shotgun, MachineGun
    //[SerializeField] private float timeBetweenShots = 0.5f;

    private Gun equippedGun;
    private CrosshairController _crosshair;
    private float _lastFireTime;
    private bool _isFiring;

    public static PlayerShoot Instance { get; private set; }

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _crosshair = FindAnyObjectByType<CrosshairController>();

        if (allGuns.Length > 0)
        {
            EquipGun(0); // Equip BasicGun by default
        }
    }

    private void Update()
    {
        HandleWeaponSwitching();

        //if (_isFiring && Time.time - _lastFireTime >= timeBetweenShots)
        //{
        //    if (_crosshair != null && equippedGun != null)
        //    {
        //        equippedGun.Fire(_crosshair.transform.position);
        //        _lastFireTime = Time.time;
        //    }
        //}
        if (_isFiring && equippedGun != null && _crosshair != null)
        {
            equippedGun.Fire(_crosshair.transform.position);
        }
    }

    public void OnAttack(InputValue inputValue)
    {
        _isFiring = inputValue.isPressed;

        if (_isFiring)
        {
            //_lastFireTime = Time.time - timeBetweenShots; // Fire immediately on click
        }
    }

    private void HandleWeaponSwitching()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) EquipGun(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) EquipGun(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) EquipGun(2);
    }

    private void EquipGun(int index)
    {
        if (index < 0 || index >= allGuns.Length)
        {
            Debug.LogWarning("Invalid gun index!");
            return;
        }

        for (int i = 0; i < allGuns.Length; i++)
        {
            bool isSelected = (i == index);

            // Activate the correct gun GameObject
            allGuns[i].gameObject.SetActive(isSelected);

            // Optional: Toggle GunAim separately if needed
            GunAim aim = allGuns[i].GetComponent<GunAim>();
            if (aim != null)
                aim.enabled = isSelected;
        }

        equippedGun = allGuns[index];
    }

    public void StartReloadCoroutine(Gun gun)
    {
        StartCoroutine(gun.Reload());
    }
}

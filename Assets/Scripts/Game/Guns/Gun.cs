using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected Transform firePoint;

    public abstract void Fire(Vector2 targetPosition);
}

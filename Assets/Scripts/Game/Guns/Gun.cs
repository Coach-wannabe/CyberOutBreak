using UnityEngine;
using System.Collections;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected Transform firePoint;

    public abstract void Fire(Vector2 targetPosition);

    public virtual IEnumerator Reload()
    {
        yield break;
    }
}

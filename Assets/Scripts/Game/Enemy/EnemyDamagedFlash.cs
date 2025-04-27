using UnityEngine;

public class EnemyDamagedFlash : MonoBehaviour
{
    [SerializeField] private float _flashDuration;
    [SerializeField] private Color _flashColor;
    [SerializeField] private int _numberOfFlashes;

    private SpritesFlash _spriteFlash;

    private void Awake()
    {
        _spriteFlash = GetComponent<SpritesFlash>();
    }

    public void StartFlash()
    {
        _spriteFlash.StartFlash(_flashDuration, _flashColor, _numberOfFlashes);
    }
}

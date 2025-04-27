using UnityEngine;

public class PlayerDamagedInvincibility : MonoBehaviour
{
    [SerializeField] private float _invincibilityDuration;
    [SerializeField] private Color _flashColor;
    [SerializeField] private int _numberOfFlashes;
    private InvincibilityController _invinciblityController;

    private void Awake()
    {
        _invinciblityController = GetComponent<InvincibilityController>();
    }

    public void StartInvincibility()
    {
        _invinciblityController.StartInvincibility(_invincibilityDuration, _flashColor, _numberOfFlashes);
    }
}

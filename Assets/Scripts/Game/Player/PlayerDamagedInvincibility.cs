using UnityEngine;

public class PlayerDamagedInvincibility : MonoBehaviour
{
    [SerializeField] private float _invincibilityDuration;
    private InvincibilityController _invinciblityController;

    private void Awake()
    {
        _invinciblityController = GetComponent<InvincibilityController>();
    }

    public void StartInvincibility()
    {
        _invinciblityController.StartInvincibility(_invincibilityDuration);
    }
}

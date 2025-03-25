using UnityEngine;

public class EnemyAwarenessController : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; }
    public Vector2 DirectionToPlayer { get; private set; }

    [SerializeField] private float _playerAwarenessDistance;
    private Transform _player;

    private void Awake()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            _player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Is your Player GameObject tagged as 'Player'?");
        }
    }

    private void Update()
    {
        if (_player == null) return;

        Vector2 enemyToPlayerVector = _player.position - transform.position;
        DirectionToPlayer = enemyToPlayerVector.normalized;

        AwareOfPlayer = enemyToPlayerVector.magnitude <= _playerAwarenessDistance;
    }
}

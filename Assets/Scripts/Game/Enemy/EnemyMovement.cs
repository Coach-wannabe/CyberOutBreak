using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _screenBorder;
    [SerializeField] private float _obstacleCheckCircleRadius;
    [SerializeField] private float _obstacleCheckDistance;
    [SerializeField] private LayerMask _obstacleLayerMask;
    [SerializeField] private Transform _graphicsTransform;

    private Rigidbody2D _rigidbody;
    private EnemyAwarenessController _awarenessController;
    private Vector2 _targetDirection;
    private float _changeDirectionCooldown;
    private Camera _camera;
    private RaycastHit2D[] _obstacleCollisions;
    private float _obstacleAvoidanceCooldown;
    private Vector2 _obstacleAvoidanceTargetDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _awarenessController = GetComponent<EnemyAwarenessController>();
        _targetDirection = transform.up;
        _camera = Camera.main;
        _obstacleCollisions = new RaycastHit2D[10];

        if (_graphicsTransform == null)
        {
            Debug.LogWarning("Enemy graphics transform not assigned!");
        }
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection();
        //RotateTowardsTarget();
        SetVelocity();
        FlipGraphicsBasedOnMovement();
    }

    private void UpdateTargetDirection()
    {
        HandleRandomDirectionChange();
        HandlePlayerTargeting();
        HandleObstacles();
        HandleEnemyOffScreen();
    }

    private void HandleRandomDirectionChange()
    {
        _changeDirectionCooldown -= Time.deltaTime;

        if (_changeDirectionCooldown <= 0) {
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            _targetDirection = rotation * _targetDirection;

            _changeDirectionCooldown = Random.Range(1f, 5f);
        }
    }

    private void HandlePlayerTargeting()
    {
        if (_awarenessController != null && _awarenessController.AwareOfPlayer)
        {
            _targetDirection = _awarenessController.DirectionToPlayer;
        }
    }

    private void HandleEnemyOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if ((screenPosition.x < _screenBorder && _targetDirection.x < 0)
            || (screenPosition.x > _camera.pixelWidth - _screenBorder && _targetDirection.x > 0))
        {
            _targetDirection = new Vector2(-_targetDirection.x, _targetDirection.y);
        }
        if ((screenPosition.y < _screenBorder && _targetDirection.y < 0)
            || (screenPosition.y > _camera.pixelHeight - _screenBorder && _targetDirection.y > 0))
        {
            _targetDirection = new Vector2(_targetDirection.x, -_targetDirection.y);
        }
    }

    private void HandleObstacles()
    {
        _obstacleAvoidanceCooldown -= Time.deltaTime;
        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_obstacleLayerMask);

        int numberOfCollisions = Physics2D.CircleCast(
            transform.position,
            _obstacleCheckCircleRadius,
            transform.up,
            contactFilter,
            _obstacleCollisions,
            _obstacleCheckDistance);

        for (int index = 0; index < numberOfCollisions; index++)
        {
            var obstacleCollision = _obstacleCollisions[index];

            if(obstacleCollision.collider.gameObject == gameObject)
            {
                continue;
            }

            if(_obstacleAvoidanceCooldown <= 0)
            {
                _obstacleAvoidanceTargetDirection = obstacleCollision.normal;
                _obstacleAvoidanceCooldown = 0.5f;
            }

            var targetRotation = Quaternion.LookRotation(transform.forward, _obstacleAvoidanceTargetDirection);
            var rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            _targetDirection = rotation * Vector2.up;
            break;
        }
    }

    private void RotateTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        _rigidbody.SetRotation(rotation);
    }

    private void SetVelocity()
    {
        _rigidbody.linearVelocity = _targetDirection * _speed;

        // Flip sprite based on horizontal direction
        if (_targetDirection.x != 0)
        {
            Vector3 localScale = _graphicsTransform.localScale;
            localScale.x = Mathf.Sign(_targetDirection.x) * Mathf.Abs(localScale.x);
            _graphicsTransform.localScale = localScale;
        }
    }

    private void FlipGraphicsBasedOnMovement()
    {
        if (_graphicsTransform == null) return;

        float horizontalVelocity = _rigidbody.linearVelocity.x;

        if (Mathf.Abs(horizontalVelocity) > 0.05f)
        {
            Vector3 localScale = _graphicsTransform.localScale;
            localScale.x = horizontalVelocity > 0 ? 1 : -1;
            _graphicsTransform.localScale = localScale;
        }
    }

}

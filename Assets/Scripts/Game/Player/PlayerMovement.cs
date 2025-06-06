using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _screenBorder;
    [SerializeField] private Transform _graphicsTransform;

    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothedVelocity;
    private Camera _camera;
    private Animator _animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _animator = GetComponent<Animator>();

        if (_graphicsTransform == null)
        {
            Debug.LogWarning("GraphicsTransform not assigned!");
        }
    }

    private void FixedUpdate()
    {
        SetPlayerVelocity();
        //RotateInDirectionOfMovement();
        SetAnimation();
        FlipSprite();
    }

    private void SetAnimation()
    {
        bool isMoving = _movementInput != Vector2.zero;

        _animator.SetBool("isMoving", isMoving);
    }

    private void SetPlayerVelocity()
    {
        _smoothedMovementInput = Vector2.SmoothDamp(
            _smoothedMovementInput,
            _movementInput,
            ref _movementInputSmoothedVelocity,
            0.1f);

        _rigidbody.linearVelocity = _smoothedMovementInput * _speed;
        PreventPlayerGoingOffScreen();
    }

    private void PreventPlayerGoingOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);
        
        if((screenPosition.x < _screenBorder && _rigidbody.linearVelocity.x < 0) 
            || (screenPosition.x > _camera.pixelWidth - _screenBorder && _rigidbody.linearVelocity.x > 0))
        {
            _rigidbody.linearVelocity = new Vector2(0, _rigidbody.linearVelocity.y);
        }
        if ((screenPosition.y < _screenBorder && _rigidbody.linearVelocity.y < 0) 
            || (screenPosition.y > _camera.pixelHeight - _screenBorder && _rigidbody.linearVelocity.y > 0))
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0);
        }
    }

    private void RotateInDirectionOfMovement()
    {
        if (_movementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _smoothedMovementInput);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            _rigidbody.MoveRotation(rotation);
        }
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

    private void FlipSprite()
    {
        if (_movementInput.x > 0.01f)
        {
            _graphicsTransform.localRotation = Quaternion.Euler(0, 0, 0); // Face right
        }
        else if (_movementInput.x < -0.01f)
        {
            _graphicsTransform.localRotation = Quaternion.Euler(0, 180, 0); // Face left
        }
    }

}

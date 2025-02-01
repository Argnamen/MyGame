using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using DG.Tweening;

public class InputService
{
    private readonly PlayerInput _playerInput;
    private readonly CinemachineInputProvider _inputProvider;
    private readonly Character _character;
    private readonly Transform _playerTransform;
    private readonly CinemachineVirtualCamera _virtualCamera;

    private Vector2 _moveInput;

    public InputService(PlayerInput playerInput, Character character, Transform playerTransform, CinemachineVirtualCamera virtualCamera)
    {
        _playerInput = playerInput;
        //_inputProvider = inputProvider;
        _character = character;
        _playerTransform = playerTransform;
        _virtualCamera = virtualCamera;

        _playerInput.actions["Move"].performed += OnMovePerformed;
        _playerInput.actions["Move"].canceled += OnMoveCanceled;
    }


    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _moveInput = Vector2.zero;
    }

    public void Update()
    {
        if (_moveInput != Vector2.zero)
        {
            HandleMovement(_moveInput);
        }
    }

    private void HandleMovement(Vector2 moveInput)
    {
        Vector2 newPos = _character.Position;

        if (moveInput.y > 0) // W
        {
            newPos = _character.Move(Character.Direction.Up);
        }
        else if (moveInput.y < 0) // S
        {
            newPos = _character.Move(Character.Direction.Down);
        }

        if (moveInput.x > 0) // D
        {
            newPos = _character.Move(Character.Direction.Right);
        }
        else if (moveInput.x < 0) // A
        {
            newPos = _character.Move(Character.Direction.Left);
        }

        _playerTransform.DOMove(newPos, 0.2f);
    }
}



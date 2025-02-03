using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using DG.Tweening;

public class InputService
{
    private readonly PlayerInput _playerInput;
    private readonly Player _player;
    private readonly Transform _playerTransform;
    private readonly CameraService _cameraService;

    private Vector2 _moveInput;

    public InputService(PlayerInput playerInput, Player player, Transform playerTransform, CameraService cameraService)
    {
        _playerInput = playerInput;
        _player = player;
        _playerTransform = playerTransform;
        _cameraService = cameraService;

        _playerInput.actions["Move"].performed += OnMovePerformed;
        _playerInput.actions["Move"].canceled += OnMoveCanceled;

        _playerInput.actions["LookAt"].performed += OnLookAtPerformed;
        _playerInput.actions["LookAt"].canceled += OnLookAtCanceled;
    }


    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _moveInput = Vector2.zero;
    }

    private void OnLookAtPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("LookAt Enemy");

        _cameraService.LookAt(_player.Character.GetClosestEnemy());
    }

    private void OnLookAtCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("LookAt Player");

        _cameraService.LookAt(_playerTransform.position);
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
        Vector2 newPos = _player.Character.Position;

        if (moveInput.y > 0) // W
        {
            newPos = _player.Character.Move(Character.Direction.Up);
        }
        else if (moveInput.y < 0) // S
        {
            newPos = _player.Character.Move(Character.Direction.Down);
        }

        if (moveInput.x > 0) // D
        {
            newPos = _player.Character.Move(Character.Direction.Right);
        }
        else if (moveInput.x < 0) // A
        {
            newPos = _player.Character.Move(Character.Direction.Left);
        }

        _playerTransform.DOMove(newPos, 0.2f);
    }
}



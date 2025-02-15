using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using DG.Tweening;
using Zenject;

public class InputService
{
    private readonly PlayerInput _playerInput;
    private readonly Player _player;
    private readonly Transform _playerTransform;
    private readonly ICameraService _cameraService;

    private Vector2 _moveInput;

    private IGameMode _gameMode;

    [Inject]
    public void Construct(IGameMode gameMode)
    {
        _gameMode = gameMode;
    }

    public InputService(PlayerInput playerInput, Player player, Transform playerTransform, ICameraService cameraService)
    {
        _playerInput = playerInput;
        _player = player;
        _playerTransform = playerTransform;
        _cameraService = cameraService;

        _playerInput.actions["Move"].performed += OnMovePerformed;
        _playerInput.actions["Move"].canceled += OnMoveCanceled;

        _playerInput.actions["LookAt"].performed += OnLookAtPerformed;
        _playerInput.actions["LookAt"].canceled += OnLookAtCanceled;

        _playerInput.actions["Steals"].performed += OnStealsMod;
        _playerInput.actions["Steals"].canceled += OnFightMod;

        _playerInput.actions["Fight"].performed += OnAutoAttack;
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
        _cameraService.LookAt(_player.Character.GetClosestEnemy());
    }

    private void OnLookAtCanceled(InputAction.CallbackContext context)
    {
        _cameraService.CancelLookAt();
    }

    private void OnStealsMod(InputAction.CallbackContext context)
    {
        _gameMode.StealsModOn();
    }

    private void OnFightMod(InputAction.CallbackContext context)
    {
        _gameMode.FightModOn();
    }

    
    private void OnAutoAttack(InputAction.CallbackContext context)
    {
        _gameMode.AutoBattleModOn();
    }

    public void Update()
    {
        HandleMovement(_moveInput);
    }

    private void HandleMovement(Vector2 moveInput)
    {
        Vector2 newPos = _player.Character.Position;
        Character.Direction direction = Character.Direction.None;

        if (moveInput.y > 0) // W
        {
            newPos = _player.Character.Move(Character.Direction.Up);
            direction = Character.Direction.Up;
        }
        else if (moveInput.y < 0) // S
        {
            newPos = _player.Character.Move(Character.Direction.Down);
            direction = Character.Direction.Down;
        }

        if (moveInput.x > 0) // D
        {
            newPos = _player.Character.Move(Character.Direction.Right);
            direction = Character.Direction.Right;
        }
        else if (moveInput.x < 0) // A
        {
            newPos = _player.Character.Move(Character.Direction.Left);
            direction = Character.Direction.Left;
        }
        else
        {
            newPos = _player.Character.Move(Character.Direction.None);
            direction = Character.Direction.None;
        }

        Sequence sequence = DOTween.Sequence();

        if (Vector2.Distance(_playerTransform.position, newPos) > 5f)
        {
            sequence.AppendCallback(() =>
            {
                _playerTransform.position = _player.Character.Position;
                _cameraService.ForceCamera();
            });
            sequence.Append(_playerTransform.DOMove(newPos, 0.1f).SetEase(Ease.Flash));
        }
        else
        {
            sequence.Append(_playerTransform.DOMove(newPos, 0.1f).SetEase(Ease.Flash));
        }
    }
}



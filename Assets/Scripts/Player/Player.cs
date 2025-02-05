using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using DG.Tweening;
using Zenject;
using System;

public class Player : MonoBehaviour
{
    private IGameMode _gameMode;

    private Coroutine _battleCoroutine;

    public PlayerCharacter Character;

    private float _inputLag = 0.3f;
    private bool _damageOn = false;
    private bool _cameraUpdate = true;

    private Color32 DamageColor = new Color32(181, 0, 0, 255);
    private Color32 DefaultColor = new Color32(255, 255, 255, 255);

    private InputService _inputService;

    public event Action OnPlayerDied;

    [Inject]
    public void Construct(IGameMode gameMode) 
    { 
        _gameMode = gameMode;
    }

    public void SetInputService(InputService inputService)
    {
        _inputService = inputService;

        _gameMode.AutoBattleMod += Attack;

        Character.IsAttack = _damageOn;
    }

    private IEnumerator DamageLag()
    {
        yield return new WaitForSeconds(Character.Weapon.Uptime);
        _damageOn = false;
    }

    private IEnumerator UpdateCameraPos()
    {
        _cameraUpdate = false;
        yield return new WaitForSeconds(0.2f);
        _cameraUpdate = true;
    }

    private void UpdateBlickColor(Color32 color32)
    {
        Sequence sequence = DOTween.Sequence();
        SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        Color32 baseColor = DefaultColor;
        sequence.Append(spriteRenderer.DOColor(color32, 0.1f));
        sequence.Append(spriteRenderer.DOColor(baseColor, 0.1f));
    }

    private void Attack()
    {
        _damageOn = !_damageOn;
        Character.IsAttack = _damageOn;
    }

    private void Start()
    {
        Character.DamageEvent.AddListener(() => UpdateBlickColor(DamageColor));
        StartCoroutine(Character.Damage(Character.EnemyList.ToArray(), true));
    }

    private void Update()
    {
        if (Character.HP <= 0)
        {
            OnPlayerDied?.Invoke();
            Destroy(this.gameObject);
            Character.DamageEvent.RemoveAllListeners();
            return;
        }

        _inputService.Update();
    }
}


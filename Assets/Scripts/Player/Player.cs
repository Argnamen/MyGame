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
    private bool _peaceOn = true;
    private bool _isAuto = false;
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

        _gameMode.AutoBattleMod += AutoAttackMod;
        _gameMode.PeaceMod += PeaceMod;

        Character.IsPeace = _peaceOn;
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

    private void AutoAttackMod()
    {
        _isAuto = !_isAuto;

        Debug.Log("IsAutoAttack " +  _isAuto);

        Attack();
    }

    private void PeaceMod()
    {
        _peaceOn = !_peaceOn;
        Character.IsPeace = _peaceOn;
    }

    private void Attack()
    {
        Character[] attackCharacters = Character.EnemyList.ToArray();

        if (_peaceOn)
        {
            attackCharacters = new Character[Character.Environments.Length];

            Debug.Log("Peace " + Character.Environments.Length);

            for (int i = 0; i < Character.Environments.Length; i++)
            {
                attackCharacters[i] = Character.Environments[i].Character;
            }
        }

        if (_battleCoroutine != null)
            StopCoroutine(_battleCoroutine);

        _battleCoroutine = StartCoroutine(Character.Damage(attackCharacters, _isAuto));
    }

    private void Start()
    {
        Character.DamageEvent.AddListener(() => UpdateBlickColor(DamageColor));
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


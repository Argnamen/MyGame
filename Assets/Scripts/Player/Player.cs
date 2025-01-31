using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Player : MonoBehaviour
{
    public TestCharacter Character;

    public List<Enemy> EnemyList = new List<Enemy>();

    private float _inputLag = 0.3f;

    private bool _damageStop = false;
    private bool _cameraUpdate = true;

    private Color32 DamageColor = new Color32(181, 0, 0, 255);
    private Color32 DefautColor = new Color32(255, 255, 255, 255);

    [SerializeField] private Camera _camera;

    [Inject]
    public void Construct()
    {
        
    }

    private IEnumerator DamageLag()
    {
        yield return new WaitForSeconds(Character.Weapon.Uptime);

        _damageStop = false;
    }

    private IEnumerator UpdateCameraPos()
    {
        _cameraUpdate = false;

        yield return new WaitForSeconds(0.2f);

        _cameraUpdate = true;
    }

    private void CameraMove()
    {
        Vector2 cameraDefauldPos = new Vector2(_camera.transform.position.x, _camera.transform.position.y + 5);
        float distance = Vector2.Distance(Character.Position, cameraDefauldPos);

        if (Mathf.Abs(distance) >= 2 || _cameraUpdate)
        {
            _camera.transform.DOMove(new Vector3(this.transform.position.x, this.transform.position.y - 5, _camera.transform.position.z), 0.2f);
            StartCoroutine(UpdateCameraPos());
        }
    }

    private void UpdateBlickColor(Color32 color32)
    {
        Sequence sequence = DOTween.Sequence();

        SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        Color32 baseColor = DefautColor;

        sequence.Append(spriteRenderer.DOColor(color32, 0.1f));
        sequence.Append(spriteRenderer.DOColor(baseColor, 0.1f));
    }


    private void Start()
    {
        _camera = Camera.main;

        _camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 5, _camera.transform.position.z);

        Character.DamageEvent.AddListener(() => UpdateBlickColor(DamageColor));

        StartCoroutine(Character.Damage(EnemyList.ToArray(), true));
    }

    private void Update()
    {
        if (Character.HP <= 0)
        {
            Destroy(this.gameObject);

            Character.DamageEvent.RemoveAllListeners();

            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            this.transform.DOMove(Character.Move(global::Character.Direction.Up), 0.2f); 
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.transform.DOMove(Character.Move(global::Character.Direction.Down), 0.2f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.DOMove(Character.Move(global::Character.Direction.Right), 0.2f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.DOMove(Character.Move(global::Character.Direction.Left), 0.2f);
        }

        if (Input.GetKey(KeyCode.L))
        {
            Character.UpdateHP(-Character.HP);
        }

        CameraMove();
    }
}

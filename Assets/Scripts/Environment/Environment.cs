using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public Enemy Enemy;

    private Color32 DamageColor = new Color32(181, 0, 0, 255);

    private void Start()
    {
        Enemy.DamageEvent.AddListener(() => UpdateBlickColor(DamageColor));
    }

    private void Update()
    {
        if (Enemy.HP <= 0)
        {
            GameObject stone = Instantiate<GameObject>((GameObject)Resources.Load("Prefab/Stone"));

            stone.transform.position = this.gameObject.transform.position;

            Destroy(this.gameObject);

            Enemy.DamageEvent.RemoveAllListeners();

            return;
        }
    }

    private void UpdateBlickColor(Color32 color32)
    {
        Sequence sequence = DOTween.Sequence();

        SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        Color32 baseColor = spriteRenderer.color;

        sequence.Append(spriteRenderer.DOColor(color32, 0.1f));
        sequence.Append(spriteRenderer.DOColor(baseColor, 0.1f));
    }
}

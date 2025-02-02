using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public Enemy Character;

    private Color32 DamageColor = new Color32(181, 0, 0, 255);

    private void Start()
    {
        Character.DamageEvent.AddListener(() => UpdateBlickColor(DamageColor));

        StartCoroutine(AutoMove());
        StartCoroutine(Character.Damage(new Character[1] { Character.Target }, true));
    }

    private void Update()
    {
        if (Character.HP <= 0)
        {
            Destroy(this.gameObject);
            Character.DamageEvent.RemoveAllListeners();
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

    private IEnumerator AutoMove()
    {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);

        while (true)
        {
            if (Character != null)
                this.transform.DOLocalMove(Character.MoveToTarget(1f), 0.01f);
            else
                yield break;

            yield return new WaitForSeconds(0.01f);
        }
    }
}

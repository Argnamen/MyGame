using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Projectile projectileObject;
    public List<Character> EnemyList = new List<Character>();

    private void Start()
    {
        StartCoroutine(AutoMove());

        StartCoroutine(projectileObject.Damage(EnemyList.ToArray(), true));
    }

    private IEnumerator AutoMove()
    {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);

        while (true)
        {
            if (projectileObject != null)
                this.transform.DOLocalMove(projectileObject.Move(), 0.01f);
            else
                yield break;

            yield return new WaitForSeconds(0.01f);
        }
    }
}

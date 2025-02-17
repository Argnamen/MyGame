using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Projectile projectileObject;
    public List<Character> EnemyList = new List<Character>();

    private List<Coroutine> coroutines = new List<Coroutine>();

    private void Start()
    {
        coroutines.Add(StartCoroutine(AutoMove()));

        coroutines.Add(StartCoroutine(projectileObject.Damage(EnemyList.ToArray(), true)));

        //coroutines.Add(StartCoroutine(DeathTimer())); 

        projectileObject.DeathEvent.AddListener(Death);
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

    private IEnumerator DeathTimer()
    {
        while (true)
        {
            projectileObject.UpdateHP(-1);

            yield return new WaitForSeconds(1f);
        }
    }

    private void Death()
    {
        foreach (var coroutine in coroutines)
        {
            StopCoroutine(coroutine);
        }

        projectileObject.DeathEvent.RemoveListener(Death);

        Destroy(this.gameObject);
    }
}

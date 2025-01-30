using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInWorld : MonoBehaviour
{
    public Item Item;

    public Action DeathAction;

    private void Start()
    {
        DeathAction = () => Death();
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }
}

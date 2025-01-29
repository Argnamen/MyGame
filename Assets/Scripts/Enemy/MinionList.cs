using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Minions", menuName = "ScriptableObjects/Minions", order = 0)]
public class MinionList : ScriptableObject
{
    public GameObject[] MinionsModels;
}

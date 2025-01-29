using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Test : MonoBehaviour
{
    private Test2 _test;

    [Inject]
    public void Construct(Test2 test)
    {
        _test = test;
        Debug.Log(_test.Text());
    }
}

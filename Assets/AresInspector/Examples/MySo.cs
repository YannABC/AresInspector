using Ares;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu()]
public class MySo : ScriptableObject, IAresObjectV
{
    [System.Serializable]
    public class A
    {
        public int id;
    }
    public int i;
    public int j;

    public A a;

    [ADButton()]
    public void TestButton()
    {

    }
}

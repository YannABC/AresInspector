using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu()]
public class MySo : ScriptableObject
{
    [System.Serializable]
    public class A
    {
        public int id;
    }
    public int i;
    public int j;

    public A a;
}

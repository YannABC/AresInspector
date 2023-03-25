using Ares;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[AresGroup(1, 0, EAresGroupType.Horizontal)]
public class MySo2 : ScriptableObject, IAresObjectH
{
    [System.Serializable]
    public class A
    {
        public int id2;
    }

    //[AresField(groupId =   1)]
    public int i2;
    //[AresField(groupId = 1)]
    public int j2;

    //[AresMethod(groupId = 1)]
    [ADButton]
    void TestMethod()
    {
        Debug.Log("click");
    }

    public A a2;
}

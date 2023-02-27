using Ares;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AresABC
{
    //[AresGroup(id: 0, parentId: 0, EAresGroupType.Vertical)]
    [AresGroup(id: 1, parentId: 0, EAresGroupType.Horizontal)]
    [AresGroup(id: 2, parentId: 0, EAresGroupType.Horizontal)]
    public class CInDefault : MonoBehaviour, IAresObject
    {
        public enum EA
        {
            None = 0,
            A = 1,
            B = 2,
        }

        [AresField(visible = "IsVisible")]
        [Range(33, 44, order = 2)]
        [Header("Header with some space around it", order = 1)]
        [Space(5, order = 2)]
        public int i;

        [AresField(groupId = 1)]
        public int j;

        [AresMethod(groupId = 1)]
        void TestMethod()
        {
            Debug.Log("TestMethod 22 clicked");
        }

        [AresField(groupId = 2)]
        public EA a;

        [AresField(groupId = 2)]
        public EA b;

        //[AresField(groupId = 2)]
        public List<int> lst;

        [HideInInspector]
        public int NotShow;

        //[AresField(groupId = 2)]
        //public C c;
        //public C c1;

        //public C[] c2;

        //[SerializeField]
        //public IC obj;

        public void Start()
        {

        }

        public bool IsVisible()
        {
            return j > 10;
        }
    }

    [Serializable]
    //[AresGroup(id: 0, parentId: 0, EAresGroupType.Vertical)]
    [AresGroup(id: 1, parentId: 0, EAresGroupType.Horizontal)]
    public class C : IAresObject
    {
        [AresField(groupId = 1)]
        public int ca;
        //public C next;
        [AresField(groupId = 1)]
        public int cb;

        public int pyj;
    }
}

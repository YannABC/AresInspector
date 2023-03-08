using Ares;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AresABC
{
    [AresGroup(id: 1, parentId: 0, EAresGroupType.Horizontal)]
    [AresGroup(id: 2, parentId: 0, EAresGroupType.Horizontal)]
    public class CInDefault : MonoBehaviour, IAresObjectV
    {
        public enum EA
        {
            None = 0,
            A = 1,
            B = 2,
        }

        [AresEnableIf("EditorOnly")]
        [ACLabel(null, 120)]
        public bool b;

        [ACLabel("aa", 0)]
        [ADRange(33, 44)]
        [Header("Header with some space around it", order = 1)]
        [ADSpace(50)]
        [AresShowIf("b")]
        [AresOnValueChanged("OnIChanged")]
        public int i;

        [ADHelpBox(ADHelpBox.MessageType.Info, "bbbbbbb", "b")]
        [ACLabel("jjjj", 40)]
        [AresOnValueChanged("OnJChanged")]
        public int j;

        [ADButton()]
        [AresEnableIf("b")]
        void TestMethod()
        {
            Debug.Log("TestMethod 22 clicked");
        }

        void OnIChanged()
        {
            Debug.Log("i changed to " + j);
        }

        void OnJChanged()
        {
            Debug.Log("j changed to " + j);
        }

        //[AresMethod()]
        //void TestMethod2()
        //{
        //    Debug.Log("TestMethod 22 clicked");
        //}

        //[AresMethod()]
        //void TestMethod3()
        //{
        //    Debug.Log("TestMethod 22 clicked");
        //}

        //[AresField(groupId = 2)]
        //public EA a;

        //[AresField(groupId = 2)]
        //public EA b;

        ////[AresField(groupId = 2)]
        //public List<int> lst;

        //[HideInInspector]
        //public int NotShow;

        //[AresField(groupId = 2)]
        //public C c;
        //public C c1;

        //public List<C> c2;

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
    public class B
    {

    }

    [Serializable]
    //[AresGroup(id: 0, parentId: 0, EAresGroupType.Vertical)]
    //[AresGroup(id: 1, parentId: 0, EAresGroupType.Horizontal)]
    public class C : B, IAresObjectH
    {
        //[AresField(groupId = 1)]
        public int ca;
        //public C next;
        // [AresField(groupId = 1)]
        public int cb;
    }
}

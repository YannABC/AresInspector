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
        [ACLabelText("bbb"), ACLabelWidth(50)]
        [ACLabelColor(1, 1, 0)]
        [ACBgColor(1, 0, 0)]
        public bool b;

        [ACLabelText("aa"), ACLabelWidth(50)]
        [ADSlider(33, 44)]
        [Header("Header with some space around it", order = 1)]
        [ADSpace(50)]
        [AresShowIf("b")]
        [AresOnValueChanged("OnIChanged")]
        [ACLabelColor(1, 1, 0)]
        public int i;

        [ADHelpBox(ADHelpBox.MessageType.Info, "bbbbbbb", "b")]
        [ACLabelText("jjjj")/*, ACLabelWidth(40)*/]
        [AresOnValueChanged("OnJChanged")]
        [ACDelayed]
        //[ADDropDown]
        public int j;

        [ACLabelWidth(40)]
        [ADDropDown("choices1")]
        public string df;

        //[ACShowInInspector]
        int _NoneSerializedInt;

        [ADButton, ACFontSize(30)]
        [AresEnableIf("b")]
        [ACLabelColor(1, 0, 0)]
        [ACBgColor(1, 1, 0)]
        void TestMethod()
        {
            Debug.Log("TestMethod 22 clicked");
        }

        void OnIChanged()
        {
            Debug.Log("i changed to " + i);
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
        [ACInline]
        public C c;
        //public C c1;

        [ACInline]
        public List<C> c2;

        //[SerializeField]
        //public IC obj;

        string[] choices1 = new string[] { "x1", "x2", "x3" };
        List<string> choices2 = new List<string>() { "x4", "x5", "x6" };
        List<string> choices3 => choices2;
        List<string> GetLstChoices() { return choices3; }
        Dictionary<string, int> GetDicChoices()
        {
            Dictionary<string, int> ret = new Dictionary<string, int>();
            ret.Add("aaaa", 1);
            ret.Add("bbbb", 2);
            ret.Add("cccc", 3);
            return ret;
        }

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

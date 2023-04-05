using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleShowIf : ScriptableObject, IAresObjectV
    {
        public bool show;

        public int j;

        [ADButton]
        [ACLabelText("Showed when show, with field")]
        [ACShowIf("show")]
        void Button1()
        {
            Debug.Log("Button1");
        }

        [ADButton]
        [ACLabelText("Showed when j >= 10, with property")]
        [ACShowIf("Button2Showed")]
        void Button2()
        {
            Debug.Log("Button2");
        }

        [ADButton]
        [ACLabelText("Showed when j >= 10, with method")]
        [ACShowIf("Button3Showed")]
        void Button3()
        {
            Debug.Log("Button3");
        }

        [ADButton]
        [ACLabelText("EditorOnly, method is in AresGlobals.cs")]
        [ACShowIf("EditorOnly")]
        void Button4()
        {
            Debug.Log("Button4");
        }

        [ADButton]
        [ACLabelText("RuntimeOnly, method is in AresGlobals.cs")]
        [ACShowIf("RuntimeOnly")]
        void Button5()
        {
            Debug.Log("Button5");
        }

        bool Button2Showed => j >= 10;
        bool Button3Showed()
        {
            return j >= 10;
        }
    }
}

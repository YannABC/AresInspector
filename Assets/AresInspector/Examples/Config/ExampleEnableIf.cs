using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleEnableIf : ScriptableObject, IAresObjectV
    {
        public bool enable;

        public int j;

        [ADButton]
        [ACLabelText("Enabled when enable, with field")]
        [ACEnableIf("enable")]
        void Button1()
        {
            Debug.Log("Button1");
        }

        [ADButton]
        [ACLabelText("Enabled when j >= 10, with property")]
        [ACEnableIf("Button2Enabled")]
        void Button2()
        {
            Debug.Log("Button2");
        }

        [ADButton]
        [ACLabelText("Enabled when j >= 10, with method")]
        [ACEnableIf("Button3Enabled")]
        void Button3()
        {
            Debug.Log("Button3");
        }

        [ADButton]
        [ACLabelText("EditorOnly, method is in AresGlobals.cs")]
        [ACEnableIf("EditorOnly")]
        void Button4()
        {
            Debug.Log("Button4");
        }

        [ADButton]
        [ACLabelText("RuntimeOnly, method is in AresGlobals.cs")]
        [ACEnableIf("RuntimeOnly")]
        void Button5()
        {
            Debug.Log("Button5");
        }

        bool Button2Enabled => j >= 10;
        bool Button3Enabled()
        {
            return j >= 10;
        }
    }
}

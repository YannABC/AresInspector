using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleLabelText : ScriptableObject, IAresObjectV
    {
        [ACLabelText("I alias")]
        public int i;

        [ADButton]
        [ACLabelText("Button1 alias")]
        public void Button1()
        {

        }
    }
}

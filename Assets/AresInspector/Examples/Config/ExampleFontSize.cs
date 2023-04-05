using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleFontSize : ScriptableObject, IAresObjectV
    {
        [ADButton]
        [ACFontSize()]
        void Button1()
        {
            Debug.Log("Button1");
        }

        [ADButton]
        [ACFontSize(20)]
        void Button2()
        {
            Debug.Log("Button2");
        }

        [ADButton]
        [ACFontSize(50)]
        void Button3()
        {
            Debug.Log("Button3");
        }
    }
}

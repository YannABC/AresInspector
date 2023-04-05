using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleBgColor : ScriptableObject, IAresObjectV
    {
        [ACBgColor(0, 1, 0)]
        public int i;

        [ADButton]
        [ACBgColor(1, 0, 0)]
        [ACLabelColor(1, 1, 0)]
        void Colorful()
        {
            Debug.Log("colorful");
        }
    }
}

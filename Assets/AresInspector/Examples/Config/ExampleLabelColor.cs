using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleLabelColor : ScriptableObject, IAresObjectV
    {
        [ACLabelColor(0, 1, 0)]
        public int greenLabel;

        [ADButton]
        [ACLabelColor(1, 1, 0)]
        void Colorful()
        {
            Debug.Log("colorful");
        }
    }
}

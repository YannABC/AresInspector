using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleLabelWidth : ScriptableObject, IAresObjectV
    {
        public int i;

        [ADHelpBox(ADHelpBox.MessageType.Info, "当width为0， label width将会自适应")]
        [ACLabelWidth(0)]
        public int j;

        [ACLabelWidth(50)]
        public int k;
    }
}

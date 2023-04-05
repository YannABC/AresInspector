using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleSpace : ScriptableObject, IAresObjectV
    {
        [ADHelpBox(ADHelpBox.MessageType.Info, "i的前面有50像素")]
        [ADSpace(50)]
        public int i;
        public int j;

        [ADHelpBox(ADHelpBox.MessageType.Info, "k的前后各有20像素，用ADField夹在中间")]
        [ADSpace(20)]
        [ADField]
        [ADSpace(20)]
        public int k;
        public int l;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleReadOnly : ScriptableObject, IAresObjectV
    {
        public int i = 9;

        [ACReadOnly]
        public int j = 10;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleTitle : ScriptableObject, IAresObjectV
    {
        [ADTitle("only title")]
        public int i;
        public int j;

        [ADTitle("title", "sub title")]
        public int k;
        public int l;
    }
}

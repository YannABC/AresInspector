using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleInline : ScriptableObject, IAresObjectV
    {
        [Serializable]
        public class C : IAresObjectH
        {
            public int ca;
            public int cb;
        }

        public C c1;

        [ACInline]
        public C c2;

        public C[] c3 = new C[3];

        [ACInline]
        public C[] c4 = new C[3];
    }
}

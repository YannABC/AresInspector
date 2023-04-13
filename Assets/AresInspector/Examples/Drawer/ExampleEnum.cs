using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleEnum : ScriptableObject, IAresObjectV
    {
        public enum NormalEnum
        {
            None = 0,
            Walk = 1,
            Attack = 2,
            Stun = 3,
            Die = 4,
        }

        [Flags]
        public enum FlagsEnum
        {
            Walk = 1,
            Attack = 2,
            Stun = 4,
            Die = 8,
        }

        public NormalEnum defaultEnum;
        public FlagsEnum defaultFlagsEnum;

        [ADToggleEnum]
        public NormalEnum toggleEnum;

        [ADFlagsEnum(false)]
        public FlagsEnum toggleFlagsEnum1;

        [ADFlagsEnum(true)]
        public FlagsEnum toggleFlagsEnum2;
    }
}

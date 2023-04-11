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
            None,
            Walk = 1,
            Attack = 2,
            Stun = 4,
            Die = 8,
        }

        public NormalEnum normalEnum;

        [ADToggleEnum]
        public NormalEnum toggleEnum;

        public FlagsEnum flagsEnum;
    }
}

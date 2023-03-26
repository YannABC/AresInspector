using Ares;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    [AresGroup(1, 0, EAresGroupType.Vertical, true)]
    [AresGroup(2, 0, EAresGroupType.Vertical, true)]
    public class LayoutNest : ScriptableObject, IAresObjectH
    {
        [ACLayout(1)]
        public int i1;
        [ACLayout(1)]
        public int j1;

        [ACLayout(2)]
        public int i2;
        [ACLayout(2)]
        public int j2;

        [ADButton]
        [ACLayout(2)]
        void Button()
        {
            Debug.Log("clicked");
        }
    }
}

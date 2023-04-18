using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleProgressBar : ScriptableObject, IAresObjectV
    {
        [ADSlider(0, 100)]
        [ADProgressBar(100, 1, 1, 0)]
        //[ADDropDown("m_Choices1_int")]
        public int i = 37;

        [ADProgressBar(100, 1, 0, 0)]
        [ACLabelText("")]
        public int j = 34;

        int[] m_Choices1_int = new int[] { 1, 2, 3 };
    }
}

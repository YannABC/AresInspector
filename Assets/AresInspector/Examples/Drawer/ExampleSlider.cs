using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleSlider : ScriptableObject, IAresObjectV
    {
        [ADSlider(33, 44)]
        [AresOnValueChanged("OnIChanged")]
        public int i;

        void OnIChanged()
        {
            Debug.Log("i changed to " + i);
        }
    }
}

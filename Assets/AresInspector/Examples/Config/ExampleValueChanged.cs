using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleValueChanged : ScriptableObject, IAresObjectV
    {
        [ACDelayed]
        [ACValueChanged("OnIChanged")]
        public int i;

        [ADSlider(1, 10000)]
        [ACDelayed]
        [ACValueChanged("OnJChanged")]
        public int j;

        [ACDelayed]
        [ACValueChanged("OnSChanged")]
        public string s;

        void OnIChanged()
        {
            Debug.Log("i changed to " + i);
        }

        void OnJChanged()
        {
            Debug.Log("j changed to " + j);
        }

        void OnSChanged()
        {
            Debug.Log("s changed to " + s);
        }
    }
}

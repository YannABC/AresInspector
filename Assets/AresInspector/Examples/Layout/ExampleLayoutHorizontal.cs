using Ares;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleLayoutHorizontal : ScriptableObject, IAresObjectH
    {
        public int i;
        public int j;

        [ADButton]
        void Button()
        {
            Debug.Log("clicked");
        }
    }
}

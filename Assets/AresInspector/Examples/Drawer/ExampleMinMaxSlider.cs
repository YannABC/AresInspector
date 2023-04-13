using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleMinMaxSlider : ScriptableObject, IAresObjectV
    {
        [ADMinMaxSlider(10f, 20f)]
        public Vector2 minmaxFloat = new Vector2(12f, 14f);

        [ADMinMaxSlider(10, 200)]
        public Vector2Int minmaxInt = new Vector2Int(15, 100);
    }
}

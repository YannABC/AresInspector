using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleAssetsOnly : ScriptableObject, IAresObjectV
    {
        public GameObject objNormal;

        [ACAssetsOnly]
        public GameObject objAssetsOnly;
    }
}

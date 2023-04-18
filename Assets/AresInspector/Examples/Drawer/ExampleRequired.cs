using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleRequired : ScriptableObject, IAresObjectV
    {
        [ACAssetsOnly]
        [ADRequired("cannot be null")]
        public GameObject go;
    }
}

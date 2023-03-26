using Ares;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class DrawerButton : ScriptableObject, IAresObjectV
    {
        [ADButton]
        void Normal()
        {
            Debug.Log("normal");
        }

        [ADButton]
        [ACFontSize(20)]
        void Bigger()
        {
            Debug.Log("Bigger");
        }

        [ADButton]
        [ACFontSize(20)]
        [ACBgColor(1, 0, 0)]
        [ACLabelColor(1, 1, 0)]
        void Colorful()
        {
            Debug.Log("colorful");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ares
{
    /// <summary>
    /// 战神编辑器
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Object), true)]
    public class AresEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            //Debug.LogWarning("CreateInspectorGUI " + target.GetType().Name);
            AresGroup group = AresHelper.GetGroup(target.GetType());
            if (group == null)
            {
                return base.CreateInspectorGUI();
            }
            else
            {
                return group.CreateGUI(new AresContext(serializedObject));
            }
        }
    }
}

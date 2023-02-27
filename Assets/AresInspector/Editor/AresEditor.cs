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
        AresGroup m_RootGroup;
        private void OnEnable()
        {
            //Debug.Log("OnEnable " + target.GetType().Name);

            m_RootGroup = target.GetType().GetCustomAttributes<AresGroup>().
                Where(ag => ag.id == 0).FirstOrDefault();
            if (m_RootGroup == null) return;
            m_RootGroup.Init(target, serializedObject);
        }

        //public override void OnInspectorGUI()
        //{
        //    if (m_RootGroup != null)
        //    {
        //        serializedObject.Update();
        //        m_RootGroup.OnGUI();
        //        serializedObject.ApplyModifiedProperties();
        //    }
        //    else
        //    {
        //        base.OnInspectorGUI();
        //    }
        //}

        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();

            //Draw the legacy IMGUI base
            var imgui = new IMGUIContainer(OnInspectorGUI);
            container.Add(imgui);

            // Create property fields.
            // Add fields to the container.
            imgui.Add(
                     new PropertyField(serializedObject.FindProperty("c2")));
            return container;

        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();
        }
    }
}

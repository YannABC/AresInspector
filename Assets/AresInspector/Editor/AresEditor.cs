using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
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

        public override void OnInspectorGUI()
        {
            if (m_RootGroup != null)
            {
                serializedObject.Update();
                m_RootGroup.OnGUI();
                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                base.OnInspectorGUI();
            }
        }
    }
}

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
        bool m_HasAres; //是否有Ares相关attribute
        //HashSet<string> m_VisibleFields;  //所有可见字段(public Serilizefield Nonserilizefield ShowInInspector）
        AresGroup m_RootGroup;
        private void OnEnable()
        {
            //Debug.Log("OnEnable " + target.GetType().Name);
            m_HasAres = ReflectionUtility.HasAres(target);
            if (!m_HasAres) return;

            //计算所有可见field
            //CalcAllVisibleFields();

            //默认有个 Group0
            m_RootGroup = new AresGroup(id: 0, parentId: 0, EAresGroupType.Vertical)
            { target = target };

            m_RootGroup.Init(serializedObject);
        }

        //void CalcAllVisibleFields()
        //{
        //    m_VisibleFields = new HashSet<string>();
        //    SerializedProperty iterator = serializedObject.GetIterator();
        //    bool enterChildren = true;
        //    while (iterator.NextVisible(enterChildren))
        //    {
        //        m_VisibleFields.Add(iterator.name);
        //        Debug.Log(iterator.propertyPath);
        //        //enterChildren = false;
        //    }
        //}



        private void OnDisable()
        {
            //Debug.Log("OnDisable " + target.GetType().Name);
        }

        public override void OnInspectorGUI()
        {
            if (!m_HasAres)
            {
                base.OnInspectorGUI();
                return;
            }
            serializedObject.Update();
            m_RootGroup.OnGUI();
            serializedObject.ApplyModifiedProperties();
        }
    }
}

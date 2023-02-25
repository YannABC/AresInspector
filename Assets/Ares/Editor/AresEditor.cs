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
        HashSet<string> m_VisibleFields;  //所有可见字段(public Serilizefield Nonserilizefield ShowInInspector）
        AresGroup m_RootGroup;
        private void OnEnable()
        {
            //Debug.Log("OnEnable " + target.GetType().Name);
            m_HasAres = ReflectionUtility.HasAres(target);

            if (!m_HasAres) return;

            //计算所有可见field
            CalcAllVisibleFields();

            //默认有个 Group0
            m_RootGroup = new AresGroup(id: 0, parentId: 0, EAresGroupType.Vertical)
            { target = target, serializedObject = serializedObject };

            //遍历所以基类及自己
            List<System.Type> types = ReflectionUtility.GetSelfAndBaseTypes(target);
            for (int i = types.Count - 1; i >= 0; i--)
            {
                System.Type type = types[i];

                //先添加group
                IEnumerable<AresGroup> groups = type.GetCustomAttributes<AresGroup>();
                foreach (AresGroup group in groups)
                {
                    AddGroup(group);
                }

                //查找当前基类里所有可以序列化且unity可见的字段, 添加到对应的group中
                IEnumerable<FieldInfo> fields = ReflectionUtility.GetAllFieldsFromType(target, type
                    , (f) => m_VisibleFields.Contains(f.Name));

                foreach (FieldInfo fi in fields)
                {
                    IEnumerable<AresField> afs = fi.GetCustomAttributes<AresField>();
                    if (afs.Any())
                    {
                        foreach (AresField af in afs)
                        {
                            af.type = type;
                            af.fieldInfo = fi;
                            AddAttrToGroup(af);
                        }
                    }
                    else
                    {
                        //默认一个
                        AresField af = new AresField();
                        af.type = type;
                        af.fieldInfo = fi;
                        AddAttrToGroup(af);
                    }
                }

                //查找所有带AresMethod标签的函数, 添加到对应的group中
                IEnumerable<MethodInfo> methods = ReflectionUtility.GetAllMethodsFromType(target, type
                    , f => f.GetCustomAttribute<AresMethod>() != null);

                foreach (MethodInfo mi in methods)
                {
                    AresMethod am = mi.GetCustomAttribute<AresMethod>();
                    am.type = type;
                    am.methodInfo = mi;

                    AddAttrToGroup(am);
                }
            }

            //排序
            m_RootGroup.SortMembers();
        }

        void CalcAllVisibleFields()
        {
            m_VisibleFields = new HashSet<string>();
            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                m_VisibleFields.Add(iterator.name);
                enterChildren = false;
            }
        }

        AresGroup FindGroup(int id)
        {
            return m_RootGroup.FindGroup(id);
        }

        void AddGroup(AresGroup group)
        {
            AresGroup ag = FindGroup(group.id);
            if (ag != null)
            {
                Debug.LogError($"duplicate group {group.id}");
                return;
            }

            int parentId = group.parentId;
            ag = FindGroup(parentId);
            if (ag == null)
            {
                Debug.LogError($"parent group {parentId} not found");
                return;
            }

            group.target = target;
            group.serializedObject = serializedObject;

            ag.subGroups.Add(group);
        }

        void AddAttrToGroup(AresMember m)
        {
            AresGroup ag = FindGroup(m.groupId);
            if (ag == null)
            {
                Debug.LogError($"AresGroup {m.groupId} in {target.GetType().Name} not found");
                return;
            }
            m.target = target;
            m.serializedObject = serializedObject;
            m.index = ag.members.Count;
            ag.members.Add(m);
        }

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

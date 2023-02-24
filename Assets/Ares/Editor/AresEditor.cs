using NUnit.Framework;
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
        bool m_HasAres;
        //List<AresGroup> m_Groups;
        AresGroup m_RootGroup;
        private void OnEnable()
        {
            //Debug.Log("OnEnable " + target.GetType().Name);
            m_HasAres = ReflectionUtility.HasAres(target);

            if (!m_HasAres) return;

            //默认有个 Group0
            m_RootGroup = new AresGroup(id: 0, parentId: 0);

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

                //查找所有可以序列化的字段, 添加到对应的group中
                IEnumerable<FieldInfo> fields = ReflectionUtility.GetAllFieldsFromType(target, type
                    , f => f.IsPublic || f.GetCustomAttribute<System.SerializableAttribute>() != null);

                foreach (FieldInfo fi in fields)
                {
                    IEnumerable<AresField> afs = fi.GetCustomAttributes<AresField>();
                    if (afs.Any())
                    {
                        foreach (AresField af in afs)
                        {
                            af.target = target;
                            af.type = type;
                            af.fieldInfo = fi;
                            AddAttrToGroup(af);
                        }
                    }
                    else
                    {
                        //默认一个
                        AresField af = new AresField();
                        af.target = target;
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

                    am.target = target;
                    am.type = type;
                    am.methodInfo = mi;

                    AddAttrToGroup(am);
                }
            }

            //排序
            SortGroupRecursive(m_RootGroup);
        }

        void SortGroupRecursive(AresGroup group)
        {
            group.members.Sort((l, r) =>
            {
                if (l.prority != r.prority) return l.prority - r.prority;
                return l.index - r.index;

            });
            foreach (AresGroup sub in group.groups)
            {
                SortGroupRecursive(sub);
            }
        }

        AresGroup FindGroup(int id)
        {
            return FindGroupRecursive(id, m_RootGroup);
        }

        AresGroup FindGroupRecursive(int id, AresGroup group)
        {
            if (id == group.id) return group;
            foreach (AresGroup sub in group.groups)
            {
                AresGroup f = FindGroupRecursive(id, sub);
                if (f != null) return f;
            }
            return null;
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
            ag = FindGroup(group.id);
            if (ag != null)
            {
                Debug.LogError($"parent group {parentId} not found");
                return;
            }

            ag.groups.Add(group);
        }

        void AddAttrToGroup(AresMember m)
        {
            AresGroup ag = FindGroup(m.groupId);
            if (ag == null)
            {
                Debug.LogError($"AresGroup {m.groupId} in {target.GetType().Name} not found");
                return;
            }
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
            DrawRecursive(m_RootGroup);
            serializedObject.ApplyModifiedProperties();
        }

        void DrawRecursive(AresGroup group)
        {
            EditorGUILayout.BeginVertical();
            foreach (AresMember m in group.members)
            {
                DrawMember(m);
            }
            EditorGUILayout.EndVertical();
        }

        void DrawMember(AresMember m)
        {
            if (m is AresField af)
            {
                DrawField(af);
            }
            else if (m is AresMethod am)
            {
                DrawMethod(am);
            }
        }

        void DrawField(AresField af)
        {
            if (af.property == null)
            {
                af.property = serializedObject.FindProperty(af.fieldInfo.Name);
            }
            SerializedProperty sp = af.property;

            if (sp == null)
            {
                Debug.LogError(af.fieldInfo.Name + " sp == null");
                return;
            }

            bool visible = true;
            if (!visible) return;

            // Validate
            //ValidatorAttribute[] validatorAttributes = PropertyUtility.GetAttributes<ValidatorAttribute>(property);
            //foreach (var validatorAttribute in validatorAttributes)
            //{
            //    validatorAttribute.GetValidator().ValidateProperty(property);
            //}

            // Check if enabled and draw
            EditorGUI.BeginChangeCheck();
            bool enabled = true;

            using (new EditorGUI.DisabledScope(disabled: !enabled))
            {
                string label = af.label == null ? sp.displayName : af.label;
                EditorGUILayout.PropertyField(sp, new GUIContent(label), includeChildren: true);

                //Rect rect = EditorGUILayout.GetControlRect();

                //int index = EditorGUI.Popup(rect, 1, new string[] { "1", "2", "3" });


                //EditorGUILayout.PropertyField(sp, GUIContent.none, includeChildren: true);

                //EditorGUI.IntSlider(EditorGUILayout.GetControlRect(), sp, 2, 10, label);

            }

            // Call OnValueChanged callbacks
            if (EditorGUI.EndChangeCheck())
            {

            }
        }

        void DrawMethod(AresMethod am)
        {

        }
    }
}

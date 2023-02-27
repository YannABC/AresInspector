using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityDebug = UnityEngine.Debug;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    [Conditional("UNITY_EDITOR")]
    public partial class AresGroup : AresAttribute
    {
        public int id;
        public int parentId;
        public EAresGroupType type;
        public List<AresGroup> subGroups = new List<AresGroup>();
        public List<AresMember> members = new List<AresMember>();

        public AresGroup(
            int id,                   //group id
            int parentId,             //parent group id
            EAresGroupType type       //group type
            )
        {
            this.id = id;
            this.parentId = parentId;
            this.type = type;
        }
    }

    public enum EAresGroupType
    {
        Horizontal, Vertical, Foldout
    }

#if UNITY_EDITOR
    public partial class AresGroup
    {
        SerializedObject m_SerializedObject;
        SerializedProperty m_SerializedProperty;

        public VisualElement CreateUI()
        {
            VisualElement root = new VisualElement();
            root.style.flexDirection = type == EAresGroupType.Horizontal ? FlexDirection.Row : FlexDirection.Column;

            foreach (AresMember member in members)
            {

            }

            foreach (AresGroup sub in subGroups)
            {
                root.Add(sub.CreateUI());
            }

            return root;
        }

        public override void OnGUI()
        {
            if (members.Count > 0)
            {
                if (type == EAresGroupType.Vertical)
                {
                    EditorGUILayout.BeginVertical();
                }
                else if (type == EAresGroupType.Horizontal)
                {
                    EditorGUILayout.BeginHorizontal();
                }
                else
                {
                    //foldout
                }

                foreach (AresMember m in members)
                {
                    m.OnGUI();
                }

                if (type == EAresGroupType.Vertical)
                {
                    EditorGUILayout.EndVertical();
                }
                else if (type == EAresGroupType.Horizontal)
                {
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    //foldout
                }
            }

            foreach (AresGroup g in subGroups)
            {
                g.OnGUI();
            }
        }

        void InitMembers()
        {
            members.Sort((l, r) =>
            {
                if (l.order != r.order) return l.order - r.order;
                return l.index - r.index;
            });
            foreach (AresMember m in members)
            {
                m.Init();
            }
            foreach (AresGroup sub in subGroups)
            {
                sub.InitMembers();
            }
        }

        AresGroup FindGroup(int id)
        {
            if (id == this.id) return this;
            foreach (AresGroup sub in subGroups)
            {
                AresGroup f = sub.FindGroup(id);
                if (f != null) return f;
            }
            return null;
        }

        //public void Init(object target, SerializedObject serializedObject)
        //{
        //    this.target = target;
        //    m_SerializedObject = serializedObject;
        //    Init();
        //}

        //public void Init(object target, SerializedProperty property)
        //{
        //    this.target = target;
        //    m_SerializedProperty = property;
        //    Init();
        //}

        public void Init(Type type)
        {
            //遍历所有基类及自己
            List<System.Type> types = AresHelper.GetSelfAndBaseTypes(type);
            for (int i = types.Count - 1; i >= 0; i--)
            {
                System.Type t = types[i];

                //先添加group
                IEnumerable<AresGroup> groups = t.GetCustomAttributes<AresGroup>();
                foreach (AresGroup group in groups)
                {
                    AddGroup(group);
                }

                //查找当前基类里所有可以序列化且unity可见的字段, 添加到对应的group中
                IEnumerable<FieldInfo> fields = AresHelper.GetAllFieldsFromType(target, t
                    , (f) => f.IsUnitySerialized() && f.GetCustomAttribute<HideInInspector>() == null);

                foreach (FieldInfo fi in fields)
                {
                    IEnumerable<AresField> afs = fi.GetCustomAttributes<AresField>();
                    if (afs.Any())
                    {
                        foreach (AresField af in afs)
                        {
                            af.type = t;
                            af.fieldInfo = fi;
                            AddAttrToGroup(af);
                        }
                    }
                    else
                    {
                        //默认一个
                        AresField af = new AresField();
                        af.type = t;
                        af.fieldInfo = fi;
                        AddAttrToGroup(af);
                    }
                }

                //查找所有带AresMethod标签的函数, 添加到对应的group中
                IEnumerable<MethodInfo> methods = AresHelper.GetAllMethodsFromType(target, t
                    , f => f.GetCustomAttribute<AresMethod>() != null);

                foreach (MethodInfo mi in methods)
                {
                    AresMethod am = mi.GetCustomAttribute<AresMethod>();
                    am.type = t;
                    am.methodInfo = mi;

                    AddAttrToGroup(am);
                }
            }

            //排序
            InitMembers();
        }

        void AddGroup(AresGroup group)
        {
            if (group.id <= 0)
            {
                UnityDebug.LogError($"group {group.id} error");
                return;
            }
            AresGroup ag = FindGroup(group.id);
            if (ag != null)
            {
                UnityDebug.LogError($"duplicate group {group.id}");
                return;
            }

            int parentId = group.parentId;
            ag = FindGroup(parentId);
            if (ag == null)
            {
                UnityDebug.LogError($"parent group {parentId} not found");
                return;
            }

            group.target = target;
            //group.serializedObject = serializedObject;

            ag.subGroups.Add(group);
        }

        void AddAttrToGroup(AresMember m)
        {
            AresGroup ag = FindGroup(m.groupId);
            if (ag == null)
            {
                UnityDebug.LogError($"AresGroup {m.groupId} in {target.GetType().Name} not found");
                return;
            }
            m.target = target;
            //m.serializedObject = serializedObject;
            if (m is AresField af)
            {
                if (m_SerializedObject != null)
                {
                    af.property = m_SerializedObject.FindProperty(af.fieldInfo.Name);
                }
                else if (m_SerializedProperty != null)
                {
                    af.property = m_SerializedProperty.FindPropertyRelative(af.fieldInfo.Name);
                }

                if (af.property == null)
                {
                    UnityDebug.LogError(af.fieldInfo.Name + " property is null");
                    return;
                }
            }
            m.index = ag.members.Count;
            ag.members.Add(m);
        }
    }
#endif
}

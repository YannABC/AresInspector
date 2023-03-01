using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        static Dictionary<Type, AresGroup> s_Groups = new Dictionary<Type, AresGroup>();
        public static AresGroup Get(Type type)
        {
            if (!s_Groups.TryGetValue(type, out AresGroup group))
            {
                bool vertical = typeof(IAresObjectV).IsAssignableFrom(type);
                bool horizontal = typeof(IAresObjectH).IsAssignableFrom(type);
                if (!vertical && !horizontal) return null;
                group = new AresGroup(0, 0,
                    vertical ? EAresGroupType.Vertical : EAresGroupType.Horizontal);
                group.Init(type);
                s_Groups.Add(type, group);
            }
            return group;
        }

        public override VisualElement CreateGUI(AresContext context)
        {
            VisualElement root = new VisualElement();
            root.style.flexDirection = type == EAresGroupType.Horizontal ? FlexDirection.Row : FlexDirection.Column;
            root.style.flexGrow = 1;

            foreach (AresMember member in members)
            {
                VisualElement ve = member.CreateGUI(context);
                if (ve != null) root.Add(ve);
            }

            foreach (AresGroup sub in subGroups)
            {
                VisualElement ve = sub.CreateGUI(context);
                if (ve != null) root.Add(ve);
            }

            return root;
        }

        void SortMembers()
        {
            members.Sort((l, r) =>
            {
                if (l.order != r.order) return l.order - r.order;
                return l.index - r.index;
            });
            //foreach (AresMember m in members)
            //{
            //    m.Init();
            //}
            foreach (AresGroup sub in subGroups)
            {
                sub.SortMembers();
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

        public void Init(Type self)
        {
            //遍历所有基类及自己
            List<System.Type> types = self.GetAncestors();
            for (int i = types.Count - 1; i >= 0; i--)
            {
                System.Type ancestor = types[i];

                //先添加group
                IEnumerable<AresGroup> groups = ancestor.GetCustomAttributes<AresGroup>();
                foreach (AresGroup group in groups)
                {
                    AddGroup(group);
                }

                //查找当前基类里所有可以序列化且unity可见的字段, 添加到对应的group中
                IEnumerable<FieldInfo> fields = ancestor.GetDeclareFields((f) => f.IsUnitySerialized() && f.GetCustomAttribute<HideInInspector>() == null);

                foreach (FieldInfo fi in fields)
                {
                    IEnumerable<AresField> afs = fi.GetCustomAttributes<AresField>();
                    if (afs.Any())
                    {
                        foreach (AresField af in afs)
                        {
                            af.ancestor = ancestor;
                            af.fieldInfo = fi;
                            AddAttrToGroup(af, self);
                        }
                    }
                    else
                    {
                        //默认一个
                        AresField af = new AresField();
                        af.ancestor = ancestor;
                        af.fieldInfo = fi;
                        AddAttrToGroup(af, self);
                    }
                }

                //查找所有带AresMethod标签的函数, 添加到对应的group中
                IEnumerable<MethodInfo> methods = ancestor.GetDeclareMethods(f => f.GetCustomAttribute<AresMethod>() != null);

                foreach (MethodInfo mi in methods)
                {
                    AresMethod am = mi.GetCustomAttribute<AresMethod>();
                    am.ancestor = ancestor;
                    am.methodInfo = mi;

                    AddAttrToGroup(am, self);
                }
            }

            //排序
            SortMembers();
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

            //group.target = target;
            //group.serializedObject = serializedObject;

            ag.subGroups.Add(group);
        }

        void AddAttrToGroup(AresMember m, Type self)
        {
            AresGroup ag = FindGroup(m.groupId);
            if (ag == null)
            {
                UnityDebug.LogError($"AresGroup {m.groupId} in {self.Name} not found");
                return;
            }

            m.index = ag.members.Count;
            ag.members.Add(m);
        }
    }
#endif
}

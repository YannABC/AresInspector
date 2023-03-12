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
    public partial class AresGroup : System.Attribute
    {
        public readonly int id;
        public readonly int parentId;
        public readonly EAresGroupType type;
        public readonly bool showBox;

        public AresGroup(
            int id,                   //group id
            int parentId,             //parent group id
            EAresGroupType type,       //group type
            bool showBox = false
            )
        {
            this.id = id;
            this.parentId = parentId;
            this.type = type;
            this.showBox = showBox;
        }
    }

    public enum EAresGroupType
    {
        Horizontal, Vertical, Foldout
    }

#if UNITY_EDITOR
    public partial class AresGroup
    {
        public List<AresGroup> subGroups = new List<AresGroup>();
        public List<AresMember> members = new List<AresMember>();
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

        public VisualElement CreateGUI(AresContext context)
        {
            if (members.Count == 0 && subGroups.Count == 0) return null;//nothing to draw

            VisualElement root = null;

            if (type == EAresGroupType.Foldout)
            {
                var fd = new Foldout();
                fd.text = context.disPlayName;
                root = fd;
            }
            else
            {
                if (showBox)
                {
                    root = new Box();
                }
                else
                {
                    root = new VisualElement();
                }
                root.style.flexDirection = type == EAresGroupType.Horizontal ? FlexDirection.Row : FlexDirection.Column;
                root.style.flexGrow = 1;
            }

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
                    IEnumerable<ACLayout> als = fi.GetCustomAttributes<ACLayout>();
                    if (als.Any())
                    {
                        foreach (ACLayout al in als)
                        {
                            AresMember af = new AresMember();
                            af.ancestor = ancestor;
                            af.fieldInfo = fi;
                            af.groupId = al.groupId;
                            af.order = al.order;
                            AddMemberToGroup(af, self);
                        }
                    }
                    else
                    {
                        //默认一个
                        AresMember af = new AresMember();
                        af.ancestor = ancestor;
                        af.fieldInfo = fi;
                        af.groupId = 0;
                        af.order = 0;
                        AddMemberToGroup(af, self);
                    }
                }

                //查找所有带AresDrawer标签的函数, 添加到对应的group中
                IEnumerable<MethodInfo> methods = ancestor.GetDeclareMethods(f => f.GetCustomAttributes<AresDrawer>().Any());

                foreach (MethodInfo mi in methods)
                {
                    IEnumerable<ACLayout> als = mi.GetCustomAttributes<ACLayout>();
                    if (als.Any())
                    {
                        foreach (ACLayout al in als)
                        {
                            AresMember af = new AresMember();
                            af.ancestor = ancestor;
                            af.methodInfo = mi;
                            af.groupId = al.groupId;
                            af.order = al.order;
                            AddMemberToGroup(af, self);
                        }
                    }
                    else
                    {
                        //默认一个
                        AresMember af = new AresMember();
                        af.ancestor = ancestor;
                        af.methodInfo = mi;
                        af.groupId = 0;
                        af.order = 0;
                        AddMemberToGroup(af, self);
                    }
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

        void AddMemberToGroup(AresMember m, Type self)
        {
            AresGroup ag = FindGroup(m.groupId);
            if (ag == null)
            {
                UnityDebug.LogError($"AresGroup {m.groupId} in {self.Name} not found");
                return;
            }

            m.index = ag.members.Count;
            m.group = ag;
            m.Init();
            ag.members.Add(m);
        }
    }
#endif
}

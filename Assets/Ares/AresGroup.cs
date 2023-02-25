using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

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
            EAresGroupType type       // group type
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

        public void SortMembers()
        {
            members.Sort((l, r) =>
            {
                if (l.prority != r.prority) return l.prority - r.prority;
                return l.index - r.index;

            });
            foreach (AresGroup sub in subGroups)
            {
                sub.SortMembers();
            }
        }

        public AresGroup FindGroup(int id)
        {
            if (id == this.id) return this;
            foreach (AresGroup sub in subGroups)
            {
                AresGroup f = sub.FindGroup(id);
                if (f != null) return f;
            }
            return null;
        }
    }
#endif
}

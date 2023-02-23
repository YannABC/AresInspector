using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

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
        List<AresGroup> m_Groups;
        private void OnEnable()
        {
            //Debug.Log("OnEnable " + target.GetType().Name);
            m_HasAres = ReflectionUtility.HasAres(target);

            //Debug.Log(target.GetType().Name + " " + m_HasAres);

            if (!m_HasAres) return;

            m_Groups = new List<AresGroup>();
            AresGroup group0 = new AresGroup(group: 0);
            m_Groups.Add(group0);


            List<System.Type> types = ReflectionUtility.GetSelfAndBaseTypes(target);

            for (int i = types.Count - 1; i >= 0; i--)
            {
                System.Type type = types[i];

                IEnumerable<AresGroup> groups = type.GetCustomAttributes<AresGroup>();
                foreach (AresGroup group in groups)
                {
                    m_Groups.Add(group);
                }

                IEnumerable<FieldInfo> fields = ReflectionUtility.GetAllFieldsFromType(target, type
                    , f => f.IsPublic || f.GetCustomAttribute<System.SerializableAttribute>() != null);

                foreach (FieldInfo fi in fields)
                {
                    AresField af = fi.GetCustomAttribute<AresField>();

                    if (af == null)
                    {
                        af = new AresField();
                    }
                    af.target = target;
                    af.type = type;
                    af.fieldInfo = fi;

                    AddToGroup(af);
                }

                IEnumerable<MethodInfo> methods = ReflectionUtility.GetAllMethodsFromType(target, type
                    , f => f.GetCustomAttribute<AresMethod>() != null);

                foreach (MethodInfo mi in methods)
                {
                    AresMethod am = mi.GetCustomAttribute<AresMethod>();

                    am.target = target;
                    am.type = type;
                    am.methodInfo = mi;

                    AddToGroup(am);
                }
            }
        }

        void AddToGroup(AresAttribute attr)
        {
            AresGroup ag = m_Groups.Find(a => a.group == attr.group);
            if (ag == null)
            {
                Debug.LogError($"AresGroup {attr.group} in {target.GetType().Name} not found");
                return;
            }

            ag.attrs.Add(attr);
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


        }
    }
}

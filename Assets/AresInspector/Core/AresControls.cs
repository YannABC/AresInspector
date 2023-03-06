using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
public static class AresControls
{
    class Control
    {
        public VisualElement ve;
        public object target;
        public string name;

        bool m_IsShowIf;
        MethodInfo m_MethodInfo;
        FieldInfo m_FieldInfo;

        public bool Init(bool isShowIf)
        {
            m_IsShowIf = isShowIf;

            if (target == null)
            {
                m_MethodInfo = typeof(AresControls).GetMethod(name);
            }
            else
            {
                Type type = target.GetType();
                m_MethodInfo = type.GetMethod(name);
                if (m_MethodInfo.ReturnType != typeof(bool) || m_MethodInfo.GetParameters().Length > 0)
                {
                    m_MethodInfo = null;
                }
                if (m_MethodInfo == null)
                {
                    m_FieldInfo = type.GetField(name);
                    if (m_FieldInfo.FieldType != typeof(bool))
                    {
                        m_FieldInfo = null;
                    }
                    if (m_FieldInfo == null)
                    {
                        m_MethodInfo = typeof(AresControls).GetMethod(name);
                    }
                }
            }

            if (m_MethodInfo == null && m_FieldInfo == null)
            {
                Debug.LogError("cannot find suitable method or field " + name);
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Tick()
        {
            bool v;
            if (m_MethodInfo != null)
            {
                v = (bool)m_MethodInfo.Invoke(target, null);
            }
            else
            {
                v = (bool)m_FieldInfo.GetValue(target);
            }

            if (m_IsShowIf)
            {
                ve.style.display = v ? DisplayStyle.Flex : DisplayStyle.None;
            }
            else
            {
                ve.SetEnabled(v);
            }
        }
    }

    static List<Control> s_Controls = new List<Control>();
    public static void RegisterShowIf(VisualElement ve, object target, string name)
    {
        Control c = new Control();
        c.ve = ve;
        c.target = target;
        c.name = name;
        if (c.Init(true))
        {
            s_Controls.Add(c);
        }
    }
    public static void RegisterEnableIf(VisualElement ve, object target, string name)
    {
        Control c = new Control();
        c.ve = ve;
        c.target = target;
        c.name = name;
        if (c.Init(false))
        {
            s_Controls.Add(c);
        }
    }

    static bool EditorOnly() { return !Application.isPlaying; }
    static bool RuntimeOnly() { return Application.isPlaying; }

    static AresControls()
    {
        EditorApplication.update += Tick;
    }

    static void Tick()
    {
        for (int i = s_Controls.Count - 1; i >= 0; i--)
        {
            Control c = s_Controls[i];
            if (c.ve.panel == null)
            {
                if (i != s_Controls.Count - 1)
                {
                    s_Controls[i] = s_Controls[s_Controls.Count - 1];
                }
                s_Controls.RemoveAt(i);
            }
            else
            {
                c.Tick();
            }
        }
    }
}
#endif
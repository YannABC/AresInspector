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
    class Controler
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
                m_MethodInfo = type.GetMethod(name,
                    BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (m_MethodInfo != null)
                {
                    if (m_MethodInfo.ReturnType != typeof(bool) || m_MethodInfo.GetParameters().Length > 0)
                    {
                        m_MethodInfo = null;
                    }
                }

                if (m_MethodInfo == null)
                {
                    m_FieldInfo = type.GetField(name);
                    if (m_FieldInfo != null)
                    {
                        if (m_FieldInfo.FieldType != typeof(bool))
                        {
                            m_FieldInfo = null;
                        }
                    }

                    if (m_FieldInfo == null)
                    {
                        m_MethodInfo = typeof(AresControls).GetMethod(name,
                            BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
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
    class Initer
    {
        public VisualElement ve;
        public System.Action cbk;
        public int count = 10;
    }


    static List<Controler> s_Controls = new List<Controler>();
    static List<Initer> s_Initers = new List<Initer>();
    public static void RegisterShowIf(VisualElement ve, object target, string name)
    {
        Controler c = new Controler();
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
        Controler c = new Controler();
        c.ve = ve;
        c.target = target;
        c.name = name;
        if (c.Init(false))
        {
            s_Controls.Add(c);
        }
    }

    public static void RegisterOnInit(VisualElement ve, System.Action cbk)
    {
        Initer initer = new Initer();
        initer.cbk = cbk;
        initer.ve = ve;
        s_Initers.Add(initer);
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
            Controler c = s_Controls[i];
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

        for (int i = s_Initers.Count - 1; i >= 0; i--)
        {
            Initer c = s_Initers[i];
            if (c.ve.panel == null || c.ve.childCount > 0)
            {
                if (c.ve.panel != null)
                {
                    c.count--;
                    c.cbk();
                }
                if (c.count <= 0)
                {
                    if (i != s_Initers.Count - 1)
                    {
                        s_Initers[i] = s_Initers[s_Initers.Count - 1];
                    }
                    s_Initers.RemoveAt(i);
                }

            }
            else
            {

            }
        }
    }
}
#endif
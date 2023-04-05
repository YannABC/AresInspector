using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
namespace Ares
{
    public static class AresControls
    {
        class Controler
        {
            public VisualElement ve;
            public object target;
            public string name;

            bool m_IsShowIf;

            public bool Init(bool isShowIf)
            {
                m_IsShowIf = isShowIf;

                Type type = target.GetType();

                return type.GetTypeMember(name) != null;
            }

            public void Tick()
            {
                bool v = (bool)AresHelper.ResolveValue(target, name);

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
}
#endif
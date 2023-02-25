using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Ares
{
    [Conditional("UNITY_EDITOR")]
    public partial class AresMethod : AresMember
    {
        //{{
        public EAresMethodType mt;
        public EAresButtonSize buttonSize;
        //}}

        public AresMethod(
            EAresMethodType mt = EAresMethodType.Button,     // 类型
            EAresButtonSize size = EAresButtonSize.Small     // 大小

            ) : base()
        {
            this.mt = mt;
            this.buttonSize = size;
        }
    }

    public enum EAresMethodType
    {
        Button,    //按钮
        Custom,    //自定义
    }

    public enum EAresButtonSize
    {
        Small = 20,
        Medium = 40,
        Big = 60,
    }

#if UNITY_EDITOR
    public partial class AresMethod
    {
        public MethodInfo methodInfo;

        public override void OnGUI()
        {
            if (GUILayout.Button(methodInfo.Name))
            {
                UnityEngine.Debug.Log(methodInfo.Name + " clicked");
                methodInfo.Invoke(target, null);
            }
        }
    }
#endif
}

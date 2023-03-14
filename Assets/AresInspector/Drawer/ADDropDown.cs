using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public partial class ADDropDown : AresDrawer
    {
        public readonly string name;
        public ADDropDown(string name) : base(false)
        {
            this.name = name;
        }
    }

#if UNITY_EDITOR
    public partial class ADDropDown
    {
        protected override VisualElement CreateFieldGUI(AresContext context)
        {
            SerializedProperty prop = context.FindProperty(member.fieldInfo.Name);

            object v = AresHelper.GetValue(context.target, name);
            if (v == null)
            {
                return new Label(name + " not found");
            }

            VisualElement ret = null;
            switch (prop.propertyType)
            {
                case SerializedPropertyType.String:
                    ret = CreateDropDown<string>(v, prop);
                    break;
                case SerializedPropertyType.Float:
                    ret = CreateDropDown<float>(v, prop);
                    break;
                case SerializedPropertyType.Integer:
                    ret = CreateDropDown<int>(v, prop);
                    break;
                default:
                    return new Label("DropDown only support int,float,string");//Enum  ??
            }

            if (ret == null)
            {
                return new Label("DropDown error occured");
            }
            else
            {
                return ret;
            }
        }

        VisualElement CreateDropDown<T>(object v, SerializedProperty prop)
        {
            List<string> keys = new List<string>();
            List<T> values = new List<T>();

            System.Type type = v.GetType();

            if (v is IEnumerable<T> arr)
            {
                foreach (var i in arr)
                {
                    keys.Add(i.ToString());
                    values.Add(i);
                }
            }
            else if (v is Dictionary<string, T> dic)
            {
                foreach (var kv in dic)
                {
                    keys.Add(kv.Key);
                    values.Add(kv.Value);
                }
            }
            else
            {
                Debug.LogError("unkown DropDown choices type " + type.FullName);
                return null;
            }

            if (keys.Count == 0)
            {
                Debug.LogError("DropDown choices count is 0");
                return null;
            }

            DropdownField df = new DropdownField(prop.displayName);
            df.style.flexGrow = 1;
            df.choices = keys;
            //df.bindingPath = prop.propertyPath;


            int idx = 0;
            if (typeof(T) == typeof(string))
            {
                List<string> ss = values as List<string>;
                idx = ss.IndexOf(prop.stringValue);

                df.RegisterValueChangedCallback<string>(e =>
                {
                    string key = e.newValue;
                    prop.stringValue = ss[keys.IndexOf(key)];
                    prop.serializedObject.ApplyModifiedProperties();
                });
            }
            else if (typeof(T) == typeof(int))
            {
                List<int> ss = values as List<int>;
                idx = ss.IndexOf(prop.intValue);

                df.RegisterValueChangedCallback<string>(e =>
                {
                    string key = e.newValue;
                    prop.intValue = ss[keys.IndexOf(key)];
                    prop.serializedObject.ApplyModifiedProperties();
                });
            }
            else if (typeof(T) == typeof(float))
            {
                List<float> ss = values as List<float>;
                idx = ss.IndexOf(prop.floatValue);

                df.RegisterValueChangedCallback<string>(e =>
                {
                    string key = e.newValue;
                    prop.floatValue = ss[keys.IndexOf(key)];
                    prop.serializedObject.ApplyModifiedProperties();
                });
            }

            if (idx < 0) idx = 0;
            df.value = keys[idx];//            history

            return df;
        }
    }
#endif
}

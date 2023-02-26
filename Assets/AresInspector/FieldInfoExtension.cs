using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class AresExtensions
{
    public static bool IsUnitySerialized(this FieldInfo fieldInfo)
    {
        if (fieldInfo.IsStatic) return false;
        object[] customAttributes = fieldInfo.GetCustomAttributes(true);
        if (customAttributes.Any(x => x is NonSerializedAttribute))
        {
            return false;
        }
        if (fieldInfo.IsPrivate && !customAttributes.Any(x => x is SerializeField))
        {
            return false;
        }
        return fieldInfo.FieldType.IsUnitySerialized();
    }

    public static bool IsUnitySerialized(this Type type)
    {
        if (type.IsGenericType)
        {
            if (type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return type.GetGenericArguments()[0].IsUnitySerialized();
            }
            return false;
        }
        if (type.IsEnum) return true;
        if (type.IsValueType) return true;
        Type typeUnityObject = typeof(UnityEngine.Object);
        if (type.IsAssignableFrom(typeUnityObject)) return true;
        if (type.IsSerializable) return true;

        Type[] typesNative =
        {
             typeof(bool),
             typeof(byte),
             typeof(float),
             typeof(int),
             typeof(string)
         };
        if (typesNative.Contains(type) || (type.IsArray && typesNative.Contains(type.GetElementType())))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Gets the object the property represents.
    /// </summary>
    /// <param name="prop"></param>
    /// <returns></returns>
    public static object GetTargetObjectOfProperty(this SerializedProperty prop)
    {
        var path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        var elements = path.Split('.');
        foreach (var element in elements)
        {
            if (element.Contains("["))
            {
                var elementName = element.Substring(0, element.IndexOf("["));
                var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = GetValue_Imp(obj, elementName, index);
            }
            else
            {
                obj = GetValue_Imp(obj, element);
            }
        }
        return obj;
    }

    private static object GetValue_Imp(object source, string name)
    {
        if (source == null)
            return null;
        var type = source.GetType();

        while (type != null)
        {
            var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (f != null)
                return f.GetValue(source);

            var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (p != null)
                return p.GetValue(source, null);

            type = type.BaseType;
        }
        return null;
    }

    private static object GetValue_Imp(object source, string name, int index)
    {
        var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
        if (enumerable == null) return null;
        var enm = enumerable.GetEnumerator();
        //while (index-- >= 0)
        //    enm.MoveNext();
        //return enm.Current;

        for (int i = 0; i <= index; i++)
        {
            if (!enm.MoveNext()) return null;
        }
        return enm.Current;
    }
}
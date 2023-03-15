using Codice.CM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

#if UNITY_EDITOR
public static class AresHelper
{
    public static object ResolveValue(object target, string name)
    {
        if (target == null) return null;
        Type type = target.GetType();

        object obj = type.GetTypeMember(name);
        if (obj == null) return null;
        if (obj is FieldInfo fi)
        {
            return fi.GetValue(target);
        }
        if (obj is PropertyInfo pi)
        {
            return pi.GetValue(target);
        }
        if (obj is MethodInfo mi)
        {
            return mi.Invoke(target, null);
        }
        return null;
    }

    static Dictionary<Type, Dictionary<string, object>> s_Members = new Dictionary<Type, Dictionary<string, object>>();
    public static object GetTypeMember(this Type type, string key)
    {
        if (!s_Members.TryGetValue(type, out Dictionary<string, object> dic))
        {
            dic = new Dictionary<string, object>();
            s_Members.Add(type, dic);
        }
        if (!dic.TryGetValue(key, out object obj))
        {
            FieldInfo fi = type.GetField(key, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            if (fi != null)
            {
                obj = fi;
            }
            else
            {
                PropertyInfo pi = type.GetProperty(key, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                if (pi != null)
                {
                    obj = pi;
                }
                else
                {
                    MethodInfo mi = type.GetMethod(key, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                    if (mi != null)
                    {
                        obj = pi;
                    }
                    else
                    {
                        mi = typeof(AresGlobals).GetMethod(key, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                        if (mi != null)
                        {
                            obj = mi;
                        }
                        else
                        {
                            Debug.LogError(key + " field or property or method not found");
                            obj = null;
                        }
                    }
                }
            }
            dic.Add(key, obj);
        }
        return obj;
    }

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
        //Debug.LogWarning(prop.displayName + " roperty type = " + prop.propertyType + " property path = " + prop.propertyPath);
        string path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        string[] elements = path.Split('.');
        foreach (string element in elements)
        {
            if (element.Contains("["))
            {
                string elementName = element.Substring(0, element.IndexOf("["));
                int index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = GetValueImp(obj, elementName, index);
            }
            else
            {
                obj = GetValueImp(obj, element);
            }
        }
        return obj;
    }

    private static object GetValueImp(object source, string name)
    {
        if (source == null)
            return null;
        Type type = source.GetType();

        while (type != null)
        {
            FieldInfo f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (f != null)
                return f.GetValue(source);

            PropertyInfo p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (p != null)
                return p.GetValue(source, null);

            type = type.BaseType;
        }
        return null;
    }

    private static object GetValueImp(object source, string name, int index)
    {
        var enumerable = GetValueImp(source, name) as System.Collections.IEnumerable;
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

    public static IEnumerable<FieldInfo> GetDeclareFields(this Type type, Func<FieldInfo, bool> predicate)
    {
        IEnumerable<FieldInfo> fieldInfos = type
            .GetFields(BindingFlags.Instance /*| BindingFlags.Static*/ | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(predicate);

        foreach (FieldInfo fieldInfo in fieldInfos)
        {
            yield return fieldInfo;
        }
    }

    public static IEnumerable<MethodInfo> GetDeclareMethods(this Type type, Func<MethodInfo, bool> predicate)
    {
        IEnumerable<MethodInfo> methodInfos = type
            .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(predicate);

        foreach (MethodInfo methodInfo in methodInfos)
        {
            yield return methodInfo;
        }
    }

    /// <summary>
    ///		Get self and all base types
    /// </summary>
    public static List<Type> GetAncestors(this Type type)
    {
        List<Type> types = new List<Type>() { type };

        while (types.Last().BaseType != null)
        {
            types.Add(types.Last().BaseType);
        }

        return types;
    }
}
#endif
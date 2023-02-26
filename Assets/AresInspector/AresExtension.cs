using Ares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class AresHelper
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

    public static bool HasAres(Type type)
    {
        bool has = type.GetFields(BindingFlags.Instance /* | BindingFlags.Static*/ | BindingFlags.NonPublic | BindingFlags.Public)
            .Where(f => f.GetCustomAttributes(typeof(AresAttribute), true).Length > 0).Any();
        if (has) return has;

        has = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
            .Where(f => f.GetCustomAttributes(typeof(AresAttribute), true).Length > 0).Any();

        return has;
    }

    public static IEnumerable<FieldInfo> GetAllFieldsFromType(object target, Type type, Func<FieldInfo, bool> predicate)
    {
        if (target == null)
        {
            Debug.LogError("The target object is null. Check for missing scripts.");
            yield break;
        }

        IEnumerable<FieldInfo> fieldInfos = type
            .GetFields(BindingFlags.Instance /*| BindingFlags.Static*/ | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(predicate);

        foreach (FieldInfo fieldInfo in fieldInfos)
        {
            yield return fieldInfo;
        }
    }

    public static IEnumerable<PropertyInfo> GetAllPropertiesFromType(object target, Type type, Func<PropertyInfo, bool> predicate)
    {
        if (target == null)
        {
            Debug.LogError("The target object is null. Check for missing scripts.");
            yield break;
        }

        IEnumerable<PropertyInfo> propertyInfos = type
            .GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(predicate);

        foreach (PropertyInfo propertyInfo in propertyInfos)
        {
            yield return propertyInfo;
        }
    }

    public static IEnumerable<MethodInfo> GetAllMethodsFromType(object target, Type type, Func<MethodInfo, bool> predicate)
    {
        if (target == null)
        {
            Debug.LogError("The target object is null. Check for missing scripts.");
            yield break;
        }


        IEnumerable<MethodInfo> methodInfos = type
            .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(predicate);

        foreach (MethodInfo methodInfo in methodInfos)
        {
            yield return methodInfo;
        }
    }

    public static IEnumerable<FieldInfo> GetAllFields(object target, Func<FieldInfo, bool> predicate)
    {
        if (target == null)
        {
            Debug.LogError("The target object is null. Check for missing scripts.");
            yield break;
        }

        List<Type> types = GetSelfAndBaseTypes(target);

        for (int i = types.Count - 1; i >= 0; i--)
        {
            IEnumerable<FieldInfo> fieldInfos = types[i]
                .GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(predicate);

            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                yield return fieldInfo;
            }
        }
    }

    public static IEnumerable<PropertyInfo> GetAllProperties(object target, Func<PropertyInfo, bool> predicate)
    {
        if (target == null)
        {
            Debug.LogError("The target object is null. Check for missing scripts.");
            yield break;
        }

        List<Type> types = GetSelfAndBaseTypes(target);

        for (int i = types.Count - 1; i >= 0; i--)
        {
            IEnumerable<PropertyInfo> propertyInfos = types[i]
                .GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(predicate);

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                yield return propertyInfo;
            }
        }
    }

    public static IEnumerable<MethodInfo> GetAllMethods(object target, Func<MethodInfo, bool> predicate)
    {
        if (target == null)
        {
            Debug.LogError("The target object is null. Check for missing scripts.");
            yield break;
        }

        List<Type> types = GetSelfAndBaseTypes(target);

        for (int i = types.Count - 1; i >= 0; i--)
        {
            IEnumerable<MethodInfo> methodInfos = types[i]
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(predicate);

            foreach (MethodInfo methodInfo in methodInfos)
            {
                yield return methodInfo;
            }
        }
    }

    public static FieldInfo GetField(object target, string fieldName)
    {
        return GetAllFields(target, f => f.Name.Equals(fieldName, StringComparison.Ordinal)).FirstOrDefault();
    }

    public static PropertyInfo GetProperty(object target, string propertyName)
    {
        return GetAllProperties(target, p => p.Name.Equals(propertyName, StringComparison.Ordinal)).FirstOrDefault();
    }

    public static MethodInfo GetMethod(object target, string methodName)
    {
        return GetAllMethods(target, m => m.Name.Equals(methodName, StringComparison.Ordinal)).FirstOrDefault();
    }

    /// <summary>
    ///		Get type and all base types of target, sorted as following:
    ///		<para />[target's type, base type, base's base type, ...]
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static List<Type> GetSelfAndBaseTypes(object target)
    {
        List<Type> types = new List<Type>()
            {
                target.GetType()
            };

        while (types.Last().BaseType != null)
        {
            types.Add(types.Last().BaseType);
        }

        return types;
    }
}
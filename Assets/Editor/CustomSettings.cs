using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;
using System.Runtime.Serialization;

/// <summary>
/// 用于存储我们的设置配置
/// </summary>
public class EditorLearnSettings : ScriptableObject
{
    /// <summary>
    /// 用于我们设置的保存位置
    /// </summary>
    private const string SettingsFile = "Assets/Settings/EditorLearnSetting.asset";

    #region 此处可以存储我们自定义的配置信息
    /// <summary>
    /// 模拟我们的配置
    /// </summary>
    public int m_ConfigVersionCode = 1;
    #endregion

    /// <summary>
    /// 这里提供一个全局的设置获取方式
    /// </summary>
    /// <returns></returns>
    public static EditorLearnSettings GetOrCreateSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<EditorLearnSettings>(SettingsFile);
        if (settings == null)
        {
            settings = ScriptableObject.CreateInstance<EditorLearnSettings>();
            settings.m_ConfigVersionCode = 0;
            if (!Directory.Exists(Path.GetDirectoryName(SettingsFile)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsFile));
            }
            AssetDatabase.CreateAsset(settings, SettingsFile);
            AssetDatabase.SaveAssets();
        }
        return settings;
    }

    /// <summary>
    /// 提供一个SerializedObject的获取
    /// </summary>
    /// <returns></returns>
    public static SerializedObject GetSerializedSettings()
    {
        //return AssetSettingsProvider.CreateProviderFromAssetPath("Project/Input Manager", "ProjectSettings/InputManager.asset", SettingsProvider.GetSearchKeywordsFromPath("ProjectSettings/InputManager.asset"));
        return new SerializedObject(GetOrCreateSettings());
    }
}

public class EditorLearnSettingsProvider : SettingsProvider
{
    private const string ProjectSettingPtah = "Project/编辑器学习";
    public EditorLearnSettingsProvider(string path, SettingsScope scope) : base(path, scope) { }

    private SerializedObject serializedObject;
    /// <summary>
    /// 当模块被激活时被调用
    /// </summary>
    /// <param name="searchContext">用户搜索的内容</param>
    /// <param name="rootElement">UIElements根节点。如果添加到此，则 SettingsProvider 使用 UIElements 而不是调用 SettingsProvider.OnGUI 来构建 UI。如果不添加到此 VisualElement，则必须使用 IMGUI 来构建 UI。</param>
    public override void OnActivate(string searchContext, VisualElement rootElement)
    {
        serializedObject = EditorLearnSettings.GetSerializedSettings();
    }

    public override void OnGUI(string searchContext)
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ConfigVersionCode"), new GUIContent("版本码："));
    }

    [SettingsProvider]
    public static SettingsProvider CreateMyCustomSettingsProvider()
    {
        var provider = new EditorLearnSettingsProvider(ProjectSettingPtah, SettingsScope.User);
        provider.keywords = new[] { "Learn", "KTGame" };
        return provider;
    }
}
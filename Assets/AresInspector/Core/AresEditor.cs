using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ares
{
#if UNITY_EDITOR
    /// <summary>
    /// 战神编辑器
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Object), true)]
    public class AresEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            //Debug.LogWarning("CreateInspectorGUI " + target.GetType().Name);
            AresGroup group = AresGroup.Get(target.GetType());
            if (group == null)
            {
                return base.CreateInspectorGUI();
            }
            else
            {
                return group.CreateGUI(new AresContext(serializedObject));
            }
        }
    }
#endif
}

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Ares
{
#if UNITY_EDITOR
    /// <summary>
    /// 战神编辑器
    /// https://www.foundations.unity.com/getting-started
    /// https://css-tricks.com/snippets/css/a-guide-to-flexbox/
    /// https://yogalayout.com/
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Object), true)]
    public class AresEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
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

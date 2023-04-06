using UnityEngine;
using UnityEngine.UIElements;

namespace Ares
{
    public partial class ADTitle : AresDrawer
    {
        public readonly string title;
        public readonly string subTitle;

        //If the condition is null or empty, the help box will always be shown.
        public ADTitle(string title, string subTitle = null) : base(true)
        {
            this.title = title;
            this.subTitle = subTitle;
        }
    }

#if UNITY_EDITOR
    public partial class ADTitle
    {
        public override VisualElement CreateGUI(AresContext context)
        {
            if (string.IsNullOrEmpty(title)) return null;

            VisualElement root = new VisualElement();
            root.style.flexGrow = 1f;
            root.style.flexShrink = 1f;

            var lblTitle = new Label(title);
            root.Add(lblTitle);
            lblTitle.style.fontSize = 14;
            lblTitle.style.unityFontStyleAndWeight = FontStyle.Bold;
            lblTitle.style.marginTop = 5f;

            if (!string.IsNullOrEmpty(subTitle))
            {
                var lblSubTitle = new Label(subTitle);
                root.Add(lblSubTitle);
                lblSubTitle.style.fontSize = 10;
                lblSubTitle.style.color = UnityEngine.Color.gray;
            }
            VisualElement line = new VisualElement();
            line.style.height = 1f;
            line.style.backgroundColor = UnityEngine.Color.grey;
            root.Add(line);
            return root;
        }
    }
#endif
}
using System;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

namespace Ares
{
    /// <summary>
    /// 进度条
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public partial class ADProgressBar : AresDrawer
    {
        public readonly float max;
        public readonly float r;
        public readonly float g;
        public readonly float b;
        public ADProgressBar(float max, float r, float g, float b) : base(false)
        {
            this.max = max;
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }

#if UNITY_EDITOR
    public partial class ADProgressBar
    {
        VisualElement _C2;
        Label _Label;
        SerializedProperty _Prop;
        protected override VisualElement CreateCustomGUI(AresContext context)
        {
            SerializedProperty prop = context.FindProperty(member.fieldInfo.Name);
            _Prop = prop;

            VisualElement root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.flexGrow = 1;
            root.style.marginLeft = 3;
            root.style.marginRight = 1;
            root.style.marginTop = 1;
            root.style.marginBottom = 1;

            string labelText = member.GetLabelText(prop.displayName);
            if (!string.IsNullOrEmpty(labelText))
            {
                root.Add(new Label(labelText));
            }

            root.Add(CreateBar(context));

            AresHelper.DoUntil(() =>
            {
                SetC2Width();
                SetLabelText();
                return false;
            }, int.MaxValue);

            return root;
        }

        VisualElement CreateBar(AresContext context)
        {
            VisualElement root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.flexGrow = 1;
            root.style.height = 20;

            VisualElement c1 = new VisualElement();
            c1.style.backgroundColor = Color.black;
            c1.style.position = Position.Absolute;
            c1.style.left = 0;
            c1.style.top = 0;
            c1.style.width = Length.Percent(100f);
            c1.style.height = Length.Percent(100f);
            root.Add(c1);

            VisualElement c2 = new VisualElement();
            c2.style.backgroundColor = new Color(r, g, b);
            c2.style.position = Position.Absolute;
            int border = 2;
            c2.style.borderLeftWidth = border;
            c2.style.borderRightWidth = border;
            c2.style.borderTopWidth = border;
            c2.style.borderBottomWidth = border;
            c2.style.borderLeftColor = Color.black;
            c2.style.borderTopColor = Color.black;
            c2.style.borderRightColor = Color.black;
            c2.style.borderBottomColor = Color.black;
            c2.style.left = 0;
            c2.style.top = 0;
            //c2.style.width = Length.Percent(50f);
            c2.style.height = Length.Percent(100f);
            root.Add(c2);
            _C2 = c2;

            SetC2Width();

            Label lbl = new Label("test");
            lbl.style.position = Position.Absolute;
            lbl.style.left = 0;
            lbl.style.top = 0;
            lbl.style.width = Length.Percent(100f);
            lbl.style.height = Length.Percent(100f);
            lbl.style.unityTextAlign = TextAnchor.MiddleCenter;
            root.Add(lbl);
            _Label = lbl;

            SetLabelText();

            return root;
        }

        float GetValue()
        {
            float v = 0f;
            if (_Prop.propertyType == SerializedPropertyType.Float)
            {
                v = _Prop.floatValue;
            }
            else if (_Prop.propertyType == SerializedPropertyType.Integer)
            {
                if (_Prop.type == "int")
                {
                    v = _Prop.intValue;
                }
                else if (_Prop.type == "long")
                {
                    v = _Prop.longValue;
                }
            }
            if (v < 0f) v = 0f;

            return v;
        }

        void SetC2Width()
        {
            float v = GetValue();

            float m = max;
            if (m < v) m = v;
            if (m == 0f)
            {
                _C2.style.width = Length.Percent(100f);
            }
            else
            {
                _C2.style.width = Length.Percent(100.0f * v / m);
            }

        }

        void SetLabelText()
        {
            _Label.text = $"{GetValue()}/{max}";
        }
    }
#endif
}

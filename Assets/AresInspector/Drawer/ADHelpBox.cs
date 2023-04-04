using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Ares
{
    public partial class ADHelpBox : AresDrawer
    {
        public enum MessageType
        {
            None,
            Info,
            Warning,
            Error
        }
        public readonly MessageType type;
        public readonly string info;
        public readonly string condition;

        //If the condition is null or empty, the help box will always be shown.
        public ADHelpBox(MessageType type, string info, string condition = null) : base(true)
        {
            this.type = type;
            this.info = info;
            this.condition = condition;
        }
    }

#if UNITY_EDITOR
    public partial class ADHelpBox
    {
        public override VisualElement CreateGUI(AresContext context)
        {
            HelpBoxMessageType t = HelpBoxMessageType.Info;
            switch (type)
            {
                case MessageType.None: t = HelpBoxMessageType.None; break;
                case MessageType.Info: t = HelpBoxMessageType.Info; break;
                case MessageType.Warning: t = HelpBoxMessageType.Warning; break;
                case MessageType.Error: t = HelpBoxMessageType.Error; break;
            }

            HelpBox hb = new HelpBox(info, t);

            if (!string.IsNullOrEmpty(condition))
            {
                AresControls.RegisterShowIf(hb, context.target, condition);
            }

            return hb;
        }
    }
#endif
}
using UnityEngine;
using Verse;

namespace Defaults
{
    public abstract class DefaultSettingsCategoryWorker
    {
        private static readonly Color buttonColor = new Color(0.2f, 0.2f, 0.2f);
        private static readonly float buttonIconSize = 50f;
        private static readonly float buttonPadding = 10f;

        public readonly DefaultSettingsCategoryDef def;

        public DefaultSettingsCategoryWorker(DefaultSettingsCategoryDef def)
        {
            this.def = def;
        }

        public abstract void OpenSettings();

        public void DoButton(Rect rect)
        {
            Widgets.DrawRectFast(rect, buttonColor);
            Widgets.DrawHighlightIfMouseover(rect);
            Rect iconRect = new Rect(rect.x + (rect.width - buttonIconSize) / 2, rect.y + buttonPadding, buttonIconSize, buttonIconSize);
            Widgets.DrawTextureFitted(iconRect, def.Icon, 1f);
            Rect labelRect = new Rect(rect.x + buttonPadding, rect.y + buttonPadding + buttonIconSize + buttonPadding, rect.width - buttonPadding * 2, rect.height - buttonPadding - buttonIconSize - buttonPadding - buttonPadding);
            Text.Anchor = TextAnchor.LowerCenter;
            Widgets.Label(labelRect, def.LabelCap);
            Text.Anchor = default;
            TooltipHandler.TipRegion(rect, def.description);
            if (Widgets.ButtonInvisible(rect))
            {
                OpenSettings();
            }
        }
    }
}

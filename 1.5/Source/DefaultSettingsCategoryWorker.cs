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

        public virtual bool GetSetting<T>(string key, out T value)
        {
            foreach (DefaultSettingDef def in def.DefaultSettings)
            {
                if (def.Worker.Key == key)
                {
                    DefaultSettingWorker<T> worker = def.Worker as DefaultSettingWorker<T>;
                    if (worker != null)
                    {
                        value = worker.setting;
                        return true;
                    }
                }
            }
            value = default;
            return false;
        }

        public virtual bool SetSetting<T>(string key, T value)
        {
            foreach (DefaultSettingDef def in def.DefaultSettings)
            {
                if (def.Worker.Key == key)
                {
                    (def.Worker as DefaultSettingWorker<T>).setting = value;
                    return true;
                }
            }
            return false;
        }

        public virtual void ExposeData()
        {
            foreach (DefaultSettingDef def in def.DefaultSettings)
            {
                def.Worker.ExposeData();
            }
        }

        public virtual void SetDefaults()
        {
            foreach (DefaultSettingDef def in def.DefaultSettings)
            {
                def.Worker.SetDefault();
            }
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

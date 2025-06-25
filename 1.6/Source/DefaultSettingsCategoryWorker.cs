using System.Collections.Generic;
using System.Linq;
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

        public static T GetWorker<T>() where T : DefaultSettingsCategoryWorker => DefDatabase<DefaultSettingsCategoryDef>.AllDefsListForReading.First(d => d.Worker is T).Worker as T;

        public DefaultSettingsCategoryWorker(DefaultSettingsCategoryDef def)
        {
            this.def = def;
            ResetCategorySettings(false);
        }

        public virtual bool GetSetting<T>(string key, out T value)
        {
            if (GetCategorySetting(key, out object o))
            {
                value = (T)o;
                return true;
            }

            foreach (DefaultSettingDef def in def.DefaultSettings)
            {
                if (def.Worker.Key == key)
                {
                    if (def.Worker is DefaultSettingWorker<T> worker)
                    {
                        value = worker.setting;
                        return true;
                    }
                }
            }

            value = default;
            return false;
        }

        protected virtual bool GetCategorySetting(string key, out object value)
        {
            value = default;
            return false;
        }

        public virtual bool SetSetting<T>(string key, T value)
        {
            if (SetCategorySetting(key, value))
            {
                return true;
            }

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

        protected virtual bool SetCategorySetting(string key, object value) => false;

        public virtual void HandleNewDefs(IEnumerable<Def> defs)
        {
        }

        protected virtual void ExposeCategorySettings()
        {
        }

        public void ExposeData()
        {
            ExposeCategorySettings();
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                ResetCategorySettings(false);
            }
            foreach (DefaultSettingDef def in def.DefaultSettings)
            {
                def.Worker.ExposeData();
            }
        }

        protected virtual void ResetCategorySettings(bool forced)
        {
        }

        public void ResetSettings()
        {
            ResetCategorySettings(true);
            foreach (DefaultSettingDef def in def.DefaultSettings)
            {
                def.Worker.ResetSetting(true);
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

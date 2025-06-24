using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults
{
    public abstract class Dialog_SettingsCategory_List : Dialog_SettingsCategory
    {
        private static Vector2 scrollPosition;

        private float settingsHeight;
        private float totalHeight;
        private readonly List<DefaultSettingDef> settings;

        public Dialog_SettingsCategory_List(DefaultSettingsCategoryDef category) : base(category)
        {
            settings = category.DefaultSettings.OrderBy(d => d.uiOrder).ToList();
        }

        public virtual float DoPostSettings(Rect rect) => 0f;

        public override void DoSettings(Rect rect)
        {
            Rect viewRect = new Rect(0f, 0f, rect.width - 20f, totalHeight);
            Widgets.BeginScrollView(new Rect(rect.x, rect.y, rect.width, rect.height - CloseButSize.y - 10f - ResetButtonSize.y - 10f), ref scrollPosition, viewRect);

            Listing_Standard listing = new Listing_StandardHighlight() { maxOneColumn = true };
            listing.Begin(viewRect);

            foreach (DefaultSettingDef def in settings)
            {
                Rect rowRect = listing.GetRect(30f);
                def.Worker.DoSetting(rowRect);
            }

            settingsHeight = listing.CurHeight + Margin;
            float postSettingsHeight = DoPostSettings(new Rect(viewRect.x, settingsHeight, viewRect.width, totalHeight));
            if (postSettingsHeight > 0f)
            {
                Widgets.DrawLineHorizontal(viewRect.x, settingsHeight - Margin / 2, viewRect.width);
            }

            totalHeight = settingsHeight + postSettingsHeight;
            listing.End();
            Widgets.EndScrollView();
        }
    }
}

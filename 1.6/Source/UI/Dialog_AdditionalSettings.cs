using Defaults.Defs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults.UI
{
    public class Dialog_AdditionalSettings : Dialog_Common
    {
        private static Vector2 scrollPosition;

        private readonly List<DefaultSettingDef> settings;
        private float y;

        public Dialog_AdditionalSettings(IEnumerable<DefaultSettingDef> settings)
        {
            this.settings = settings.ToList();
        }

        public override Vector2 InitialSize => new Vector2(500f, 480f + CloseButSize.y + Margin);

        public override void DoWindowContents(Rect inRect)
        {
            base.DoWindowContents(inRect);

            using (new TextBlock(GameFont.Medium)) Widgets.Label(inRect, "Defaults_AdditionalSettings".Translate());

            Rect viewRect = new Rect(0f, 0f, inRect.width - 20f, y);
            Widgets.BeginScrollView(inRect.TopPartPixels(inRect.height - CloseButSize.y - Margin).BottomPartPixels(400f), ref scrollPosition, viewRect);
            y = UIUtility.DoSettingsList(viewRect, settings);
            Widgets.EndScrollView();
        }
    }
}

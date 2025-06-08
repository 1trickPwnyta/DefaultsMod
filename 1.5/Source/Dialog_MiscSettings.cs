using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults
{
    [StaticConstructorOnStartup]
    public class Dialog_MiscSettings : Window
    {
        private static readonly List<DefaultSettingDef> settings = DefDatabase<DefaultSettingDef>.AllDefsListForReading.Where(d => d.category == DefDatabase<DefaultSettingsCategoryDef>.GetNamed("Misc")).OrderBy(d => d.uiOrder).ToList();

        private static Vector2 scrollPosition;

        private float y;

        public Dialog_MiscSettings()
        {
            doCloseX = true;
            doCloseButton = true;
            optionalTitle = "Defaults_MiscSettings".Translate();
        }

        public override Vector2 InitialSize => new Vector2(600f, 550f);

        public override void DoWindowContents(Rect inRect)
        {
            Rect viewRect = new Rect(0f, 0f, inRect.width - 20f, y);
            Widgets.BeginScrollView(inRect, ref scrollPosition, viewRect);
            Listing_Standard listing = new Listing_StandardHighlight() { maxOneColumn = true };
            listing.Begin(viewRect);

            foreach (DefaultSettingDef def in settings)
            {
                Rect rect = listing.GetRect(30f);
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(rect, def.LabelCap);
                Text.Anchor = TextAnchor.UpperLeft;
                def.Worker.DoSetting(rect);
            }

            y = listing.CurHeight;
            listing.End();
            Widgets.EndScrollView();
        }
    }
}

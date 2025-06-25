using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults
{
    public class DefaultsSettings : ModSettings
    {
        public static List<string> KnownDLCs;
        private static List<FactionDef> PreviousFactionDefs;
        private static List<ThingDef> PreviousThingDefs;
        private static List<SpecialThingFilterDef> PreviousSpecialThingFilterDefs;

        private static readonly Vector2 settingsCategoryButtonSize = new Vector2(150f, 120f);
        private static readonly float settingsCategoryButtonMargin = 30f;
        private static readonly List<DefaultSettingsCategoryDef> categories = DefDatabase<DefaultSettingsCategoryDef>.AllDefsListForReading;

        private static float y;
        private static Vector2 scrollPosition;
        private static readonly QuickSearchWidget search = new QuickSearchWidget();

        public static void ResetAllSettings()
        {
            foreach (DefaultSettingsCategoryDef def in categories)
            {
                def.Worker.ResetSettings();
            }
        }

        public static void CheckForNewContent()
        {
            HandleNewDefs(ref PreviousFactionDefs);
            HandleNewDefs(ref PreviousThingDefs);
            HandleNewDefs(ref PreviousSpecialThingFilterDefs);
            DefaultsMod.Settings.Write();
        }

        private static void HandleNewDefs<T>(ref List<T> previousDefs) where T : Def
        {
            List<T> currentDefs = DefDatabase<T>.AllDefsListForReading;
            if (previousDefs != null)
            {
                List<T> newDefs = currentDefs.Except(previousDefs).ToList();
                if (newDefs.Any())
                {
                    foreach (DefaultSettingsCategoryDef def in categories)
                    {
                        def.Worker.HandleNewDefs(newDefs);
                    }
                }
            }
            previousDefs = currentDefs.ListFullCopy();
        }

        public static void DoSettingsWindowContents(Rect inRect)
        {
            search.OnGUI(new Rect(inRect.xMax - 250f - 20f, inRect.y - 15f - QuickSearchWidget.WidgetHeight, 250f, QuickSearchWidget.WidgetHeight));

            float width = settingsCategoryButtonSize.x * 4 + settingsCategoryButtonMargin * 3;
            Rect outRect = new Rect(inRect.x + (inRect.width - width) / 2, inRect.y, inRect.width - (inRect.width - width) / 2, inRect.height - 30f - 10f);
            Rect viewRect = new Rect(0f, 0f, width, y);
            y = 0f;
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);

            IEnumerable<DefaultSettingsCategoryDef> categories = DefDatabase<DefaultSettingsCategoryDef>.AllDefsListForReading;
            if (search.filter.Active)
            {
                categories = categories.Where(c => c.Matches(search.filter));
            }

            if (search.filter.Active)
            {
                using (new TextBlock(GameFont.Medium))
                {
                    Widgets.Label(new Rect(viewRect.x, y, viewRect.width, Text.LineHeight), ref y, "Defaults_SearchResults".Translate());
                    y += 10f;
                }
            }
            if (categories.Any())
            {
                DoCategories(viewRect, ref y, categories);
            }
            if (search.filter.Active)
            {
                IEnumerable<DefaultSettingDef> settings = DefDatabase<DefaultSettingDef>.AllDefsListForReading.Where(s =>
                    s.Matches(search.filter)
                );
                if (settings.Any())
                {
                    DoSettings(viewRect, ref y, settings);
                }

                if (!categories.Any() && !settings.Any())
                {
                    Widgets.Label(new Rect(viewRect.x, y, viewRect.width, Text.LineHeight), ref y, "Defaults_NoSearchResults".Translate(search.filter.Text));
                }
            }

            Widgets.EndScrollView();

            if (Widgets.ButtonText(new Rect(inRect.x + inRect.width / 3, inRect.yMax - 30f - 8f, inRect.width / 3, 30f), "Defaults_ResetAllSettings".Translate()))
            {
                Find.WindowStack.Add(new Dialog_MessageBox("Defaults_ConfirmResetAllSettings".Translate(), "Confirm".Translate(), ResetAllSettings, "GoBack".Translate(), null, null, true, ResetAllSettings));
            }
        }

        private static void DoCategories(Rect rect, ref float y, IEnumerable<DefaultSettingsCategoryDef> categories)
        {
            if (search.filter.Active)
            {
                Widgets.Label(new Rect(rect.x, y, rect.width, Text.LineHeight), ref y, "Defaults_DefaultSettingsCategories".Translate());
                Widgets.DrawLineHorizontal(rect.x, y, rect.width);
                y += 10f;
            }

            float x = 0f;
            foreach (DefaultSettingsCategoryDef def in categories)
            {
                Rect buttonRect = new Rect(x, y, settingsCategoryButtonSize.x, settingsCategoryButtonSize.y);
                def.Worker.DoButton(buttonRect);
                x += buttonRect.width + settingsCategoryButtonMargin;
                if (x + settingsCategoryButtonSize.x > rect.xMax)
                {
                    x = 0f;
                    y += settingsCategoryButtonSize.y + settingsCategoryButtonMargin;
                }
            }
            if (x > 0)
            {
                y += settingsCategoryButtonSize.y + settingsCategoryButtonMargin;
            }
        }

        private static void DoSettings(Rect rect, ref float y, IEnumerable<DefaultSettingDef> settings)
        {
            Widgets.Label(new Rect(rect.x, y, rect.width, Text.LineHeight), ref y, "Defaults_DefaultSettings".Translate());
            Widgets.DrawLineHorizontal(rect.x, y, rect.width);
            y += 10f;

            Listing_Standard listing = new Listing_StandardHighlight() { maxOneColumn = true };
            listing.Begin(new Rect(rect.x, y, rect.width, rect.height - y));

            foreach (DefaultSettingDef def in settings)
            {
                Rect rowRect = listing.GetRect(30f);
                def.Worker.DoSetting(rowRect);
            }

            y += listing.CurHeight;
            listing.End();
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref KnownDLCs, "KnownDLCs");
            Scribe_Collections.Look(ref PreviousFactionDefs, "PreviousFactionDefs");
            Scribe_Collections.Look(ref PreviousThingDefs, "PreviousThingDefs");
            Scribe_Collections.Look(ref PreviousSpecialThingFilterDefs, "PreviousSpecialThingFilterDefs");

            foreach (DefaultSettingsCategoryDef def in categories)
            {
                def.Worker.ExposeData();
            }
        }
    }
}

using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults.BabyFeeding
{
    public class Dialog_BabyFeedingSettings : Window
    {
        private static Vector2 scrollPosition;

        private float y;

        public Dialog_BabyFeedingSettings()
        {
            this.doCloseX = true;
            this.doCloseButton = true;
            this.optionalTitle = "Defaults_BabyFeedingSettings".Translate();
        }

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(500f, 550f);
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label("AutofeedSectionHeader".Translate().CapitalizeFirst());
            listing.GapLine();
            DoAutofeedRow(listing, "Defaults_BirtherParent", DefaultsSettings.DefaultBabyFeedingOptions.BirtherParent, mode => DefaultsSettings.DefaultBabyFeedingOptions.BirtherParent = mode);
            DoAutofeedRow(listing, "Defaults_ParentLactating", DefaultsSettings.DefaultBabyFeedingOptions.ParentLactating, mode => DefaultsSettings.DefaultBabyFeedingOptions.ParentLactating = mode);
            DoAutofeedRow(listing, "Defaults_ParentNonlactating", DefaultsSettings.DefaultBabyFeedingOptions.ParentNonlactating, mode => DefaultsSettings.DefaultBabyFeedingOptions.ParentNonlactating = mode);
            DoAutofeedRow(listing, "Defaults_NonparentLactating", DefaultsSettings.DefaultBabyFeedingOptions.NonparentLactating, mode => DefaultsSettings.DefaultBabyFeedingOptions.NonparentLactating = mode);
            DoAutofeedRow(listing, "Defaults_NonparentNonlactating", DefaultsSettings.DefaultBabyFeedingOptions.NonparentNonlactating, mode => DefaultsSettings.DefaultBabyFeedingOptions.NonparentNonlactating = mode);

            listing.Gap(24f);

            Rect lockRect = new Rect(inRect.width - 24f, listing.CurHeight, 24f, 24f);
            UIUtility.DrawCheckButton(lockRect, UIUtility.LockIcon, "Defaults_LockSetting".Translate(), ref DefaultsSettings.DefaultBabyFeedingOptions.locked);
            listing.Label("BabyFoodConsumables".Translate().CapitalizeFirst());
            listing.GapLine();
            Rect outRect = listing.GetRect(200f);
            Rect viewRect = new Rect(0f, 0f, inRect.width, y);
            y = 0f;
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
            foreach (ThingDef def in ITab_Pawn_Feeding.BabyConsumableFoods)
            {
                DoConsumableRow(new Rect(viewRect.x, y, viewRect.width, Text.LineHeight), def);
            }
            Widgets.EndScrollView();

            listing.End();
        }

        private void DoAutofeedRow(Listing_Standard listing, string key, AutofeedMode mode, Action<AutofeedMode> callback)
        {
            if (listing.ButtonTextLabeled(key.Translate(), mode.Translate().CapitalizeFirst()))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (AutofeedMode option in Enum.GetValues(typeof(AutofeedMode)))
                {
                    list.Add(new FloatMenuOption(option.Translate().CapitalizeFirst(), () =>
                    {
                        callback(option);
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            }
        }

        private void DoConsumableRow(Rect rect, ThingDef def)
        {
            Widgets.DefIcon(new Rect(rect.x, rect.y, rect.height, rect.height), def);
            Widgets.Label(new Rect(rect.x + rect.height + 12f, rect.y, rect.width / 2f, rect.height), def.LabelCap);
            Widgets.InfoCardButton(rect.xMax - 24f, rect.y, def);
            bool allowed = DefaultsSettings.DefaultBabyFeedingOptions.AllowedConsumables.Contains(def);
            Widgets.Checkbox(rect.xMax - 24f - 12f - 24f, rect.y, ref allowed);
            if (allowed)
            {
                DefaultsSettings.DefaultBabyFeedingOptions.AllowedConsumables.Add(def);
            }
            else
            {
                DefaultsSettings.DefaultBabyFeedingOptions.AllowedConsumables.Remove(def);
            }
            y += rect.height;
        }
    }
}

using Defaults.Defs;
using Defaults.UI;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Defaults.WorkPriorities
{
    public class Dialog_WorkPriorities : Dialog_SettingsCategory_List
    {
        private int? dragValue;

        public Dialog_WorkPriorities(DefaultSettingsCategoryDef category) : base(category)
        {
        }

        public override Vector2 InitialSize => new Vector2(500f, 700f);

        public override float DoPostSettings(Rect rect)
        {
            float y = rect.y;

            bool advancedMode = Settings.Get<bool>(Settings.WORK_PRIORITIES_ADVANCED_MODE);
            Widgets.CheckboxLabeled(new Rect(rect.x, y, rect.width, 30f), "Defaults_WorkPrioritiesAdvancedMode".Translate(), ref advancedMode, placeCheckboxNearText: true);
            Settings.Set(Settings.WORK_PRIORITIES_ADVANCED_MODE, advancedMode);
            y += 30f;

            if (advancedMode)
            {
                DoAdvancedMode(rect, ref y);
            }
            else
            {
                DoBasicMode(rect, ref y);
            }

            return y - rect.y;
        }

        private void DoBasicMode(Rect rect, ref float y)
        {
            Widgets.Label(new Rect(rect.x, y, rect.width, 60f), "Defaults_BasicWorkPrioritiesDesc".Translate());
            y += 60f;

            Listing_StandardHighlight listing = new Listing_StandardHighlight() { maxOneColumn = true };
            listing.Begin(new Rect(rect.x, y, rect.width, rect.height));

            Dictionary<WorkTypeDef, int> basicDefaultWorkPriorities = Settings.Get<Dictionary<WorkTypeDef, int>>(Settings.WORK_PRIORITIES_BASIC);
            foreach (WorkTypeDef def in DefDatabase<WorkTypeDef>.AllDefsListForReading.Where(d => basicDefaultWorkPriorities.ContainsKey(d)).OrderByDescending(d => d.naturalPriority))
            {
                Rect rowRect = listing.GetRect(30f);
                using (new TextBlock(TextAnchor.MiddleLeft)) Widgets.Label(rowRect.LeftPart(0.7f), def.labelShort.CapitalizeFirst());
                Rect buttonRect = rowRect.RightPart(0.3f);
                int priority = basicDefaultWorkPriorities[def];
                Widgets.DraggableResult result = UIUtility.DoImageTextButtonDraggable(buttonRect, WorkPriorityValue.GetIcon(priority), WorkPriorityValue.GetLabel(priority).CapitalizeFirst());
                if (result == Widgets.DraggableResult.Pressed)
                {
                    Find.WindowStack.Add(new FloatMenu(WorkPriorityValue.GetOptions(i => basicDefaultWorkPriorities[def] = i)));
                }
                else if (result == Widgets.DraggableResult.Dragged)
                {
                    dragValue = priority;
                }
                if (dragValue.HasValue && Mouse.IsOver(buttonRect) && priority != dragValue.Value)
                {
                    basicDefaultWorkPriorities[def] = dragValue.Value;
                    SoundDefOf.Click.PlayOneShot(null);
                }
            }

            listing.End();
            y += listing.CurHeight;

            if (!Input.GetMouseButton(0))
            {
                dragValue = null;
            }
        }

        private void DoAdvancedMode(Rect rect, ref float y)
        {
            Listing_Standard listing = new Listing_Standard() { maxOneColumn = true };
            listing.Begin(new Rect(rect.x, y, rect.width, rect.height));

            listing.Label("Defaults_AdvancedWorkPrioritiesDesc".Translate());
            listing.GapLine();

            listing.Label("Defaults_WorkPriorityRulesGlobalDesc".Translate());
            Rect globalButtonRect = listing.GetRect(30f).LeftPart(0.3f);
            if (DoRuleButton(globalButtonRect, "Defaults_WorkPriorityRulesGlobal".Translate()))
            {
                Find.WindowStack.Add(new Dialog_Rules("Defaults_WorkPriorityRulesGlobal".Translate(), Settings.Get<List<Rule>>(Settings.WORK_PRIORITIES_GLOBAL_LOGIC)));
            }

            listing.End();
            y += listing.CurHeight;
        }

        private bool DoRuleButton(Rect rect, string text)
        {
            Widgets.DrawRectFast(rect, Widgets.MenuSectionBGFillColor);
            Widgets.DrawHighlightIfMouseover(rect);
            return Widgets.ButtonText(rect, text, false, overrideTextAnchor: TextAnchor.MiddleCenter);
        }
    }
}

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
    public class Dialog_Rules : Dialog_Common
    {
        private static readonly float uiPadding = 5f;

        private readonly string title;
        private readonly List<Rule> rules;
        private Vector2 scrollPosition;
        private float height;

        public Dialog_Rules(string title, List<Rule> rules)
        {
            this.title = title;
            this.rules = rules;
        }

        public override Vector2 InitialSize => new Vector2(900f, 500f);

        public override void DoWindowContents(Rect inRect)
        {
            base.DoWindowContents(inRect);

            float y = 0f;

            using (new TextBlock(GameFont.Medium)) Widgets.Label(inRect, title.CapitalizeFirst());
            y += 45f;

            Widgets.Label(new Rect(inRect.x, y, inRect.width, 30f), "Defaults_WorkPriorityRulesDesc".Translate());
            y += 30f;

            Rect headerRect = new Rect(inRect.x, y, inRect.width, 30f);
            Widgets.DrawRectFast(headerRect, Widgets.MenuSectionBGFillColor);
            if (Widgets.ButtonText(headerRect.RightPartPixels(100f).ContractedBy(3f), "Defaults_WorkPriorityAddRule".Translate()))
            {
                rules.Add(new Rule());
                SoundDefOf.Click.PlayOneShot(null);
            }
            // TODO paste button?
            headerRect.width -= 20f;
            using (new TextBlock(TextAnchor.MiddleLeft))
            {
                Widgets.Label(headerRect.LeftHalf().ContractedBy(uiPadding), "Defaults_WorkPriorityCondition".Translate());
                Widgets.Label(headerRect.RightHalf().ContractedBy(uiPadding), "Defaults_WorkPriorityEffect".Translate());
            }
            y += 30f;

            Rect viewRect = new Rect(0f, 0f, inRect.width - 20f, height);
            Widgets.BeginScrollView(new Rect(inRect.x, y, inRect.width, inRect.height - y), ref scrollPosition, viewRect);
            height = 0f;
            foreach (Rule rule in rules)
            {
                Rect ruleRect = new Rect(viewRect.x, height, viewRect.width, 45f);

                // TODO Drag icon

                Rect conditionRect = ruleRect.LeftHalf();
                if (UIUtility.DoImageTextButton(conditionRect.LeftHalf().ContractedBy(uiPadding), rule.condition?.def.Icon, rule.condition?.def.LabelCap ?? "None".Translate()))
                {
                    Find.WindowStack.Add(new FloatMenu(DefDatabase<WorkPriorityConditionDef>.AllDefsListForReading.Select(c => new FloatMenuOption(c.LabelCap, () =>
                    {
                        rule.condition = c.Worker.MakeCondition();
                    }, c.Icon, Color.white)).ToList()));
                }
                rule.condition?.def.Worker.DoUI(conditionRect.RightHalf().ContractedBy(uiPadding), rule.condition);

                Rect effectRect = ruleRect.RightHalf();
                if (UIUtility.DoImageTextButton(effectRect.LeftHalf().ContractedBy(uiPadding), rule.effect?.def.Icon, rule.effect?.def.LabelCap ?? "None".Translate()))
                {
                    Find.WindowStack.Add(new FloatMenu(DefDatabase<WorkPriorityEffectDef>.AllDefsListForReading.Select(e => new FloatMenuOption(e.LabelCap, () =>
                    {
                        rule.effect = e.Worker.MakeEffect();
                    }, e.Icon, Color.white)).ToList()));
                }
                rule.effect?.def.Worker.DoUI(effectRect.RightHalf().ContractedBy(uiPadding), rule.effect);

                // TODO copy icon?
                // TODO delete icon

                height += 45f;
            }
            Widgets.EndScrollView();
        }
    }
}

using Defaults.UI;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Defaults.Policies
{
    public class Dialog_DefaultPolicyAssignments : Dialog_Common
    {
        private static readonly string defaultLabel = $"({"Default".Translate().ToLower()})";

        private bool dragDefault;
        private Policy dragValue;

        public override Vector2 InitialSize => new Vector2(1000f, 400f);

        public override void DoWindowContents(Rect inRect)
        {
            base.DoWindowContents(inRect);

            float y = inRect.y;

            DefaultPolicyAssignments assignments = Settings.Get<DefaultPolicyAssignments>(Settings.POLICY_ASSIGNMENTS);
            foreach (PawnType pawnType in assignments.PolicyAssignments.Keys.Where(p => p.IsActive()))
            {
                float x = inRect.x;
                Rect rect = new Rect(x, y, inRect.width, 30f);
                Widgets.DrawLineHorizontal(rect.x, rect.y, rect.width, Widgets.InactiveColor);

                Rect pawnTypeRect = new Rect(x, rect.y, rect.width / 5, rect.height);
                using (new TextBlock(TextAnchor.MiddleLeft)) Widgets.Label(pawnTypeRect, pawnType.GetLabel().CapitalizeFirst());
                x += pawnTypeRect.width;

                x += DoPolicyButton(new Rect(x, rect.y, rect.width / 5, rect.height), ref assignments.PolicyAssignments[pawnType].apparelPolicy, Settings.Get<List<ApparelPolicy>>(Settings.POLICIES_APPAREL), p => assignments.PolicyAssignments[pawnType].apparelPolicy = p);
                x += DoPolicyButton(new Rect(x, rect.y, rect.width / 5, rect.height), ref assignments.PolicyAssignments[pawnType].foodPolicy, Settings.Get<List<FoodPolicy>>(Settings.POLICIES_FOOD), p => assignments.PolicyAssignments[pawnType].foodPolicy = p);
                x += DoPolicyButton(new Rect(x, rect.y, rect.width / 5, rect.height), ref assignments.PolicyAssignments[pawnType].drugPolicy, Settings.Get<List<DrugPolicy>>(Settings.POLICIES_DRUG), p => assignments.PolicyAssignments[pawnType].drugPolicy = p);
                x += DoPolicyButton(new Rect(x, rect.y, rect.width / 5, rect.height), ref assignments.PolicyAssignments[pawnType].readingPolicy, Settings.Get<List<ReadingPolicy>>(Settings.POLICIES_READING), p => assignments.PolicyAssignments[pawnType].readingPolicy = p);

                Widgets.DrawHighlightIfMouseover(rect);

                y += rect.height;
            }

            if (!Input.GetMouseButton(0))
            {
                dragValue = null;
                dragDefault = false;
            }
        }

        private float DoPolicyButton<T>(Rect rect, ref T policy, List<T> options, Action<T> setPolicy) where T : Policy
        {
            Rect buttonRect = rect.ContractedBy(1f);
            Widgets.DraggableResult result = Widgets.ButtonTextDraggable(buttonRect, policy?.RenamableLabel ?? defaultLabel);
            if (result == Widgets.DraggableResult.Pressed)
            {
                Find.WindowStack.Add(new FloatMenu(options.Prepend(null).Select(p => new FloatMenuOption(p?.RenamableLabel ?? defaultLabel, () => setPolicy(p))).ToList()));
            }
            if (result == Widgets.DraggableResult.Dragged)
            {
                dragValue = policy;
                dragDefault = dragValue == null;
            }
            if (Mouse.IsOver(buttonRect) && (dragDefault || dragValue is T) && policy != dragValue)
            {
                policy = dragDefault ? null : dragValue as T;
                SoundDefOf.Click.PlayOneShot(null);
            }
            return rect.width;
        }
    }
}

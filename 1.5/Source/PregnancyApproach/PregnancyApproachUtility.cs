using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Defaults.PregnancyApproach
{
    public static class PregnancyApproachUtility
    {
        public static void DrawPregnancyApproachButton(Rect rect)
        {
            RimWorld.PregnancyApproach approach = Settings.Get<RimWorld.PregnancyApproach>(Settings.PREGNANCY_APPROACH);
            if (Widgets.ButtonImage(rect, approach.GetIcon(), true, string.Concat(new string[]
                {
                    "PregnancyApproach".Translate().Colorize(ColoredText.TipSectionTitleColor),
                    "\n",
                    approach.GetDescription(),
                    "\n\n",
                    "ClickToChangePregnancyApproach".Translate().Colorize(ColoredText.SubtleGrayColor)
                })))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>();
                foreach (RimWorld.PregnancyApproach pregnancyApproach in Enum.GetValues(typeof(RimWorld.PregnancyApproach)))
                {
                    options.Add(new FloatMenuOption(pregnancyApproach.GetDescription(), delegate ()
                    {
                        Settings.Set(Settings.PREGNANCY_APPROACH, pregnancyApproach);
                    }, pregnancyApproach.GetIcon(), Color.white));
                }
                Find.WindowStack.Add(new FloatMenu(options));
            }
        }
    }
}

using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults.MechWorkModes
{
    public static class MechWorkModeUtility
    {
        public static void DrawWorkModeButton(Rect rect, MechWorkModeDef def, Action<MechWorkModeDef> callback)
        {
            if (Widgets.ButtonImage(rect, def.uiIcon, true, string.Concat(new string[]
            {
                ("CurrentMechWorkMode".Translate() + ": " + def.LabelCap).Colorize(ColoredText.TipSectionTitleColor),
                "\n",
                def.description,
            })))
            {
                Find.WindowStack.Add(
                    new FloatMenu(DefDatabase<MechWorkModeDef>.AllDefsListForReading.Select(m =>
                        new FloatMenuOption(m.LabelCap, () => callback(m), m.uiIcon, Color.white)
                        {
                            tooltip = m.description
                        }
                    ).ToList())
                );
            }
        }
    }
}

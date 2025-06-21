using RimWorld;
using UnityEngine;
using Verse;

namespace Defaults.MechWorkModes
{
    public abstract class DefaultSettingWorker_MechWorkMode : DefaultSettingWorker<MechWorkModeDef>
    {
        protected DefaultSettingWorker_MechWorkMode(DefaultSettingDef def) : base(def)
        {
        }

        protected override void DoWidget(Rect rect)
        {
            rect.x += rect.width - 24f;
            rect.width = 24f;
            MechWorkModeUtility.DrawWorkModeButton(rect, setting, m => setting = m);
        }

        protected override void ExposeSetting()
        {
            Scribe_Defs.Look(ref setting, Key);
        }

        protected override MechWorkModeDef Default => MechWorkModeDefOf.Work;
    }
}

using UnityEngine;
using Verse;

namespace Defaults.PregnancyApproach
{
    public class DefaultSettingWorker_PregnancyApproach : DefaultSettingWorker<RimWorld.PregnancyApproach>
    {
        public DefaultSettingWorker_PregnancyApproach(DefaultSettingDef def) : base(def)
        {
        }

        public override string Key => Settings.PREGNANCY_APPROACH;

        protected override RimWorld.PregnancyApproach Default => RimWorld.PregnancyApproach.Normal;

        protected override void DoWidget(Rect rect)
        {
            rect.x += rect.width - 28f;
            rect.width = 32f;
            PregnancyApproachUtility.DrawPregnancyApproachButton(rect.ContractedBy(4f));
        }

        protected override void ExposeSetting()
        {
            Scribe_Values.Look(ref setting, Key);
        }
    }
}

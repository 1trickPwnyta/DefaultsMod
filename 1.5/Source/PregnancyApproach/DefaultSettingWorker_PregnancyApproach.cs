using UnityEngine;
using Verse;

namespace Defaults.PregnancyApproach
{
    public class DefaultSettingWorker_PregnancyApproach : DefaultSettingWorker
    {
        public DefaultSettingWorker_PregnancyApproach(DefaultSettingDef def) : base(def)
        {
        }

        public override void DoSetting(Rect rect)
        {
            rect.x += rect.width - 28f;
            rect.width = 32f;
            PregnancyApproachUtility.DrawPregnancyApproachButton(rect.ContractedBy(4f));
        }
    }
}

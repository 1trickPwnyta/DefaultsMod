using RimWorld;
using UnityEngine;
using Verse;

namespace Defaults.HostilityResponse
{
    public class DefaultSettingWorker_HostilityResponse : DefaultSettingWorker<HostilityResponseMode?>
    {
        protected override HostilityResponseMode? Default => HostilityResponseMode.Flee;

        public override string Key => Settings.HOSTILITY_RESPONSE;

        public DefaultSettingWorker_HostilityResponse(DefaultSettingDef def) : base(def)
        {
        }

        protected override void ExposeSetting()
        {
            Scribe_Values.Look(ref setting, Key);
        }

        protected override void DoWidget(Rect rect)
        {
            rect.x += rect.width - 24f;
            rect.width = 24f;
            HostilityResponseModeUtility.DrawResponseButton(rect);
        }
    }
}

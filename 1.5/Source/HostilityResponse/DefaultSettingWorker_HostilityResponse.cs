using UnityEngine;

namespace Defaults.HostilityResponse
{
    public class DefaultSettingWorker_HostilityResponse : DefaultSettingWorker
    {
        public DefaultSettingWorker_HostilityResponse(DefaultSettingDef def) : base(def)
        {
        }

        public override void DoSetting(Rect rect)
        {
            rect.x += rect.width - 24f;
            rect.width = 24f;
            HostilityResponseModeUtility.DrawResponseButton(rect);
        }
    }
}

using UnityEngine;

namespace Defaults.PlaySettings
{
    public class DefaultSettingWorker_AutoHomeArea : DefaultSettingWorker
    {
        public DefaultSettingWorker_AutoHomeArea(DefaultSettingDef def) : base(def)
        {
        }

        public override void DoSetting(Rect rect)
        {
            PlaySettingsUtility.DrawAutoHomeAreaButton(new Rect(rect.x + rect.width - 24f, rect.y + 3f, 24f, 24f));
        }
    }
}

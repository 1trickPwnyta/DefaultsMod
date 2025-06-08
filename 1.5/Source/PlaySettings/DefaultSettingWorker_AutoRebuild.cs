using UnityEngine;

namespace Defaults.PlaySettings
{
    public class DefaultSettingWorker_AutoRebuild : DefaultSettingWorker
    {
        public DefaultSettingWorker_AutoRebuild(DefaultSettingDef def) : base(def)
        {
        }

        public override void DoSetting(Rect rect)
        {
            PlaySettingsUtility.DrawAutoRebuildButton(new Rect(rect.x + rect.width - 24f, rect.y + 3f, 24f, 24f));
        }
    }
}

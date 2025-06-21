using UnityEngine;
using Verse;

namespace Defaults.PlaySettings
{
    public class DefaultSettingWorker_AutoRebuild : DefaultSettingWorker<bool?>
    {
        public DefaultSettingWorker_AutoRebuild(DefaultSettingDef def) : base(def)
        {
        }

        public override string Key => Settings.AUTO_REBUILD;

        protected override bool? Default => false;

        protected override void DoWidget(Rect rect)
        {
            PlaySettingsUtility.DrawAutoRebuildButton(new Rect(rect.x + rect.width - 24f, rect.y + 3f, 24f, 24f));
        }

        protected override void ExposeSetting()
        {
            Scribe_Values.Look(ref setting, Key);
        }
    }
}

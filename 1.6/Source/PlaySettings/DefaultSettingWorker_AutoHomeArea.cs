using UnityEngine;
using Verse;

namespace Defaults.PlaySettings
{
    public class DefaultSettingWorker_AutoHomeArea : DefaultSettingWorker<bool?>
    {
        protected override bool? Default => true;

        public override string Key => Settings.AUTO_HOME_AREA;

        public DefaultSettingWorker_AutoHomeArea(DefaultSettingDef def) : base(def)
        {
        }

        protected override void ExposeSetting()
        {
            Scribe_Values.Look(ref setting, Key);
        }

        protected override void DoWidget(Rect rect)
        {
            PlaySettingsUtility.DrawAutoHomeAreaButton(new Rect(rect.x + rect.width - 24f, rect.y + 3f, 24f, 24f));
        }
    }
}

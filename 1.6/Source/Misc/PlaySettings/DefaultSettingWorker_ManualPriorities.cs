using Defaults.Defs;
using Defaults.Workers;

namespace Defaults.Misc.PlaySettings
{
    public class DefaultSettingWorker_ManualPriorities : DefaultSettingWorker_Checkbox
    {
        public DefaultSettingWorker_ManualPriorities(DefaultSettingDef def) : base(def)
        {
        }

        public override string Key => Settings.MANUAL_PRIORITIES;

        protected override bool? Default => false;
    }
}

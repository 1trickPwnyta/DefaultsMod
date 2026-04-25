using Defaults.Defs;
using Defaults.Workers;

namespace Defaults.General
{
    public class DefaultSettingWorker_ConfirmSaveDefault : DefaultSettingWorker_Checkbox
    {
        public DefaultSettingWorker_ConfirmSaveDefault(DefaultSettingDef def) : base(def)
        {
        }

        public override string Key => Settings.CONFIRM_SAVE_DEFAULT;

        protected override bool? Default => true;
    }
}

using UnityEngine;
using Verse;

namespace Defaults
{
    public class Dialog_MiscSettings : Dialog_SettingsCategory_List
    {
        public Dialog_MiscSettings(DefaultSettingsCategoryDef category) : base(category)
        {
        }

        public override string Title => "Defaults_MiscSettings".Translate();

        public override Vector2 InitialSize => new Vector2(600f, 550f);

        protected override bool DoResetButton => false;
    }
}

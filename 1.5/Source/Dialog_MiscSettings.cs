using UnityEngine;
using Verse;

namespace Defaults
{
    public class Dialog_MiscSettings : SettingsDialog_List
    {
        public Dialog_MiscSettings() : base(DefDatabase<DefaultSettingsCategoryDef>.GetNamed("Misc"))
        {
        }

        public override string Title => "Defaults_MiscSettings".Translate();

        public override Vector2 InitialSize => new Vector2(600f, 550f);
    }
}

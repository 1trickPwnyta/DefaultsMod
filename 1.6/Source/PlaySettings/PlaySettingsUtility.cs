using RimWorld;
using UnityEngine;
using Verse;

namespace Defaults.PlaySettings
{
    public static class PlaySettingsUtility
    {
        public static void DrawAutoRebuildButton(Rect rect)
        {
            bool setting = Settings.GetValue<bool>(Settings.AUTO_REBUILD);
            UIUtility.DrawCheckButton(rect, TexButton.AutoRebuild, "AutoRebuildButton".Translate(), ref setting);
            Settings.SetValue(Settings.AUTO_REBUILD, setting);
        }

        public static void DrawAutoHomeAreaButton(Rect rect)
        {
            bool setting = Settings.GetValue<bool>(Settings.AUTO_HOME_AREA);
            UIUtility.DrawCheckButton(rect, TexButton.AutoHomeArea, "AutoHomeAreaToggleButton".Translate(), ref setting);
            Settings.SetValue(Settings.AUTO_HOME_AREA, setting);
        }
    }
}

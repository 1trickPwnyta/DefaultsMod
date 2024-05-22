using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Defaults.PlaySettings
{
    public static class PlaySettingsUtility
    {
        private static void DrawButton(Rect rect, Texture2D buttonTex, string tooltip, ref bool enabled)
        {
            bool flag = Widgets.ButtonImage(rect, buttonTex, true, null);
            TooltipHandler.TipRegion(rect, tooltip);
            Rect rect2 = new Rect(rect.x + rect.width / 2f, rect.y, rect.width / 2f, rect.height / 2f);
            Texture2D texture2D = enabled ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex;
            GUI.DrawTexture(rect2, texture2D);
            MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_ButtonToggle);
            if (flag)
            {
                enabled = !enabled;
                if (enabled)
                {
                    SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
                }
                else
                {
                    SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
                }
            }
        }

        public static void DrawAutoRebuildButton(Rect rect)
        {
            DrawButton(rect, TexButton.AutoRebuild, "AutoRebuildButton".Translate(), ref DefaultsSettings.DefaultAutoRebuild);
        }

        public static void DrawAutoHomeAreaButton(Rect rect)
        {
            DrawButton(rect, TexButton.AutoHomeArea, "AutoHomeAreaToggleButton".Translate(), ref DefaultsSettings.DefaultAutoHomeArea);
        }
    }
}

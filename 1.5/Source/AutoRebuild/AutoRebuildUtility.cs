using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Defaults.AutoRebuild
{
    public static class AutoRebuildUtility
    {
        public static void DrawAutoRebuildButton(Rect rect)
        {
            bool flag = Widgets.ButtonImage(rect, TexButton.AutoRebuild, true, null);
            TooltipHandler.TipRegion(rect, "AutoRebuildButton".Translate());
            Rect rect2 = new Rect(rect.x + rect.width / 2f, rect.y, rect.width / 2f, rect.height / 2f);
            Texture2D texture2D = DefaultsSettings.DefaultAutoRebuild ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex;
            GUI.DrawTexture(rect2, texture2D);
            MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_ButtonToggle);
            if (flag)
            {
                DefaultsSettings.DefaultAutoRebuild = !DefaultsSettings.DefaultAutoRebuild;
                if (DefaultsSettings.DefaultAutoRebuild)
                {
                    SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
                }
                else
                {
                    SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
                }
            }
        }
    }
}

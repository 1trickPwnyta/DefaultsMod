using RimWorld;
using UnityEngine;
using Verse.Sound;
using Verse;

namespace Defaults
{
    [StaticConstructorOnStartup]
    public static class ButtonUtility
    {
        public static Texture2D LockIcon = ContentFinder<Texture2D>.Get("UI/Defaults_Lock");

        public static void DrawCheckButton(Rect rect, Texture2D buttonTex, string tooltip, ref bool enabled)
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
    }
}

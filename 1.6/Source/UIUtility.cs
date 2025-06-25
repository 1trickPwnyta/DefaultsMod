using RimWorld;
using UnityEngine;
using Verse.Sound;
using Verse;

namespace Defaults
{
    [StaticConstructorOnStartup]
    public static class UIUtility
    {
        public static Texture2D LockIcon = ContentFinder<Texture2D>.Get("UI/Defaults_Lock");
        public static Texture2D StarIcon = ContentFinder<Texture2D>.Get("UI/Defaults_Star");

        public static void DrawCheckButton(Rect rect, Texture2D buttonTex, string tooltip, ref bool enabled)
        {
            if (Widgets.ButtonImage(rect, buttonTex))
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
            TooltipHandler.TipRegion(rect, tooltip);
            Rect checkRect = new Rect(rect.x + rect.width / 2f, rect.y, rect.width / 2f, rect.height / 2f);
            GUI.DrawTexture(checkRect, enabled ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);
        }

        public static void TemperatureEntry(Rect rect, ref float value, int multiplier = 1, float min = 0f, float max = 1E+09f)
        {
            int num = Mathf.Min(40, (int)rect.width / 5);
            if (Widgets.ButtonText(new Rect(rect.xMin, rect.yMin, num, rect.height), "--"))
            {
                value -= 10 * multiplier * GenUI.CurrentAdjustmentMultiplier();
                SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera();
            }

            if (Widgets.ButtonText(new Rect(rect.xMin + num, rect.yMin, num, rect.height), "-"))
            {
                value -= multiplier * GenUI.CurrentAdjustmentMultiplier();
                SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera();
            }

            if (Widgets.ButtonText(new Rect(rect.xMax - num, rect.yMin, num, rect.height), "++"))
            {
                value += 10 * multiplier * GenUI.CurrentAdjustmentMultiplier();
                SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera();
            }

            if (Widgets.ButtonText(new Rect(rect.xMax - (num * 2), rect.yMin, num, rect.height), "+"))
            {
                value += multiplier * GenUI.CurrentAdjustmentMultiplier();
                SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera();
            }

            value = Mathf.Clamp(value, min, max);

            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(rect.xMin + (num * 2), rect.yMin, rect.width - (num * 4), rect.height), value.ToStringTemperature("F0"));
            Text.Anchor = TextAnchor.UpperLeft;
        }
    }
}

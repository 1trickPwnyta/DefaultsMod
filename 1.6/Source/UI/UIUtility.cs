using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Defaults.UI
{
    [StaticConstructorOnStartup]
    public static class UIUtility
    {
        public static Texture2D LockIcon = ContentFinder<Texture2D>.Get("UI/Defaults_Lock");
        public static Texture2D StarIcon = ContentFinder<Texture2D>.Get("UI/Defaults_Star");

        public static Window TopWindow => Find.WindowStack.Windows.Last(w => !(w is ImmediateWindow) && !(w is FloatMenu));

        public static void DoCheckButton(Rect rect, Texture2D buttonTex, string tooltip, ref bool enabled)
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

        private static void DrawImageTextButton(Rect rect, Texture2D image, string text)
        {
            Widgets.DrawButtonGraphic(rect);
            if (image != null)
            {
                GUI.DrawTexture(rect.LeftPartPixels(rect.height).ContractedBy(6f), image);
            }
            using (new TextBlock(TextAnchor.MiddleLeft)) Widgets.Label(rect.RightPartPixels(rect.width - rect.height).ContractedBy(3f), text);
        }

        public static bool DoImageTextButton(Rect rect, Texture2D image, string text)
        {
            DrawImageTextButton(rect, image, text);
            return Widgets.ButtonInvisible(rect);
        }

        public static Widgets.DraggableResult DoImageTextButtonDraggable(Rect rect, Texture2D image, string text)
        {
            DrawImageTextButton(rect, image, text);
            return Widgets.ButtonInvisibleDraggable(rect, true);
        }

        public static void DoTemperatureEntry(Rect rect, ref float value, int multiplier = 1, float min = 0f, float max = 1E+09f)
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

        public static void DoDraggable(int group, Rect draggableRect, Rect? controlRect = null, Rect? tipRect = null)
        {
            if (controlRect.HasValue)
            {
                using (new TextBlock(Mouse.IsOver(controlRect.Value) && !ReorderableWidget.Dragging ? GenUI.MouseoverColor : Color.white)) GUI.DrawTexture(controlRect.Value, TexButton.DragHash);
            }
            if (tipRect.HasValue)
            {
                TooltipHandler.TipRegion(tipRect.Value, "Defaults_DragToReorder".Translate());
            }
            if ((Event.current.type != EventType.MouseDown || Mouse.IsOver(controlRect ?? draggableRect)) && ReorderableWidget.Reorderable(group, draggableRect))
            {
                Widgets.DrawRectFast(draggableRect, Color.black.WithAlpha(0.5f));
            }
        }

        public static void IntEntry(Rect rect, ref int value, ref string editBuffer, int multiplier = 1, int minimum = 0, int maximum = int.MaxValue)
        {
            Widgets.IntEntry(rect, ref value, ref editBuffer, multiplier);
            value = Mathf.Clamp(value, minimum, maximum);
            editBuffer = value.ToString();
        }
    }
}

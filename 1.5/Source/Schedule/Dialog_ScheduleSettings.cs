using RimWorld;
using UnityEngine;
using Verse.Sound;
using Verse;
using System;

namespace Defaults.Schedule
{
    public class Dialog_ScheduleSettings : Window
    {
        private static Vector2 scrollPos;

        public Dialog_ScheduleSettings()
        {
            this.doCloseX = true;
            this.doCloseButton = true;
            this.optionalTitle = "Defaults_Schedules".Translate();
        }

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(800f, 600f);
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            float selectorHeight = 65f;
            TimeAssignmentSelector.DrawTimeAssignmentSelectorGrid(new Rect(0f, 0f, 191f, selectorHeight));

            float buttonWidth = 160f;
            if (Widgets.ButtonText(new Rect(inRect.x + inRect.width - buttonWidth, 0f, buttonWidth, 30f), "Defaults_AddAlternateSchedule".Translate()))
            {
                DefaultsSettings.DefaultSchedules.Add(new Schedule());
                SoundDefOf.Click.PlayOneShotOnCamera(null);
            }

            Text.Font = GameFont.Tiny;
            Text.Anchor = TextAnchor.LowerCenter;
            float labelWidth = 160f;
            float copyButtonWidth = 18f;
            float x = inRect.x + labelWidth + copyButtonWidth;
            float cellWidth = 540 / 24f;
            float rowHeight = 30f;
            for (int i = 0; i < 24; i++)
            {
                Widgets.Label(new Rect(x, inRect.y + rowHeight, cellWidth, rowHeight), i.ToString());
                x += cellWidth;
            }

            Widgets.BeginScrollView(new Rect(inRect.x, inRect.y + rowHeight * 2, inRect.width, inRect.height - rowHeight * 4), ref scrollPos, new Rect(0f, 0f, inRect.width - 16f, rowHeight * DefaultsSettings.DefaultSchedules.Count));

            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleLeft;
            float y = 0f;
            for (int i = 0; i < DefaultsSettings.DefaultSchedules.Count; i++)
            {
                Widgets.Label(new Rect(0f, y, labelWidth, rowHeight), "Defaults_ScheduleName".Translate(i + 1));

                x = inRect.x + labelWidth;
                CopyPasteUI.DoCopyPasteButtons(new Rect(x, y, copyButtonWidth * 2, rowHeight), delegate
                {
                    DefaultsSettings.DefaultSchedules.Add(new Schedule(DefaultsSettings.DefaultSchedules[i]));
                }, null);

                x += copyButtonWidth;
                for (int j = 0; j < 24; j++)
                {
                    DoTimeAssignment(new Rect(x, y, cellWidth, rowHeight), DefaultsSettings.DefaultSchedules[i], j);
                    x += cellWidth;
                }

                if (i > 0)
                {
                    if (Widgets.ButtonImage(new Rect(labelWidth + cellWidth * 24 + 16f, y + (rowHeight - 24f) / 2, 24f, 24f), TexButton.Delete, Color.white, Color.white * GenUI.SubtleMouseoverColor))
                    {
                        DefaultsSettings.DefaultSchedules.Remove(DefaultsSettings.DefaultSchedules[i--]);
                        SoundDefOf.Click.PlayOneShotOnCamera(null);
                    }
                }

                y += rowHeight;
            }

            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;

            Widgets.EndScrollView();
        }

        private static void DoTimeAssignment(Rect rect, Schedule schedule, int hour)
        {
            rect = rect.ContractedBy(1f);
            bool mouseButton = Input.GetMouseButton(0);
            TimeAssignmentDef assignment = schedule.GetTimeAssignment(hour);
            GUI.DrawTexture(rect, assignment.ColorTexture);
            if (!mouseButton)
            {
                MouseoverSounds.DoRegion(rect);
            }
            if (Mouse.IsOver(rect))
            {
                Widgets.DrawBox(rect, 2, null);
                if (mouseButton && assignment != TimeAssignmentSelector.selectedAssignment && TimeAssignmentSelector.selectedAssignment != null)
                {
                    SoundDefOf.Designate_DragStandard_Changed_NoCam.PlayOneShotOnCamera(null);
                    schedule.SetTimeAssignment(hour, TimeAssignmentSelector.selectedAssignment);
                }
            }
        }
    }
}

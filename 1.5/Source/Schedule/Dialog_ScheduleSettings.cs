using RimWorld;
using UnityEngine;
using Verse.Sound;
using Verse;

namespace Defaults.Schedule
{
    public class Dialog_ScheduleSettings : SettingsDialog
    {
        private static Vector2 scrollPos;

        public override string Title => "Defaults_Schedules".Translate();

        public override Vector2 InitialSize => new Vector2(860f, 600f);

        public override void DoSettings(Rect rect)
        {
            float selectorHeight = 65f;
            TimeAssignmentSelector.DrawTimeAssignmentSelectorGrid(new Rect(rect.x, rect.y, 191f, selectorHeight));

            float buttonWidth = 160f;
            if (Widgets.ButtonText(new Rect(rect.x + rect.width - buttonWidth, rect.y, buttonWidth, 30f), "Defaults_AddAlternateSchedule".Translate()))
            {
                DefaultsSettings.DefaultSchedules.Add(new Schedule("Defaults_ScheduleName".Translate(DefaultsSettings.DefaultSchedules.Count + 1)));
                SoundDefOf.Click.PlayOneShotOnCamera(null);
            }

            Text.Font = GameFont.Tiny;
            Text.Anchor = TextAnchor.LowerCenter;
            float labelWidth = 160f;
            float copyButtonWidth = 18f;
            float x = rect.x + 24f + 24f + labelWidth + copyButtonWidth;
            float cellWidth = 540 / 24f;
            float rowHeight = 30f;
            for (int i = 0; i < 24; i++)
            {
                Widgets.Label(new Rect(x, rect.y + rowHeight, cellWidth, rowHeight), i.ToString());
                x += cellWidth;
            }
            x += 8f;
            Rect useLabelRect = new Rect(x, rect.y + rowHeight, 24f, rowHeight);
            Widgets.Label(useLabelRect, "Defaults_UseSchedule".Translate());
            TooltipHandler.TipRegionByKey(useLabelRect, "Defaults_UseScheduleTip");

            Widgets.BeginScrollView(new Rect(rect.x, rect.y + rowHeight * 2, rect.width, rect.height - rowHeight * 4), ref scrollPos, new Rect(0f, 0f, rect.width - 16f, rowHeight * DefaultsSettings.DefaultSchedules.Count));

            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleLeft;
            float y = 0f;
            for (int i = 0; i < DefaultsSettings.DefaultSchedules.Count; i++)
            {
                Schedule schedule = DefaultsSettings.DefaultSchedules[i];
                x = rect.x;

                if (i < DefaultsSettings.DefaultSchedules.Count - 1)
                {
                    if (Widgets.ButtonImage(new Rect(x + 3f, y + (rowHeight - 18f) / 2, 18f, 18f), TexButton.ReorderDown, Color.white, Color.white * GenUI.SubtleMouseoverColor))
                    {
                        Schedule next = DefaultsSettings.DefaultSchedules[i + 1];
                        DefaultsSettings.DefaultSchedules[i + 1] = schedule;
                        DefaultsSettings.DefaultSchedules[i] = next;
                        SoundDefOf.Click.PlayOneShotOnCamera(null);
                    }
                }
                x += 24f;

                if (i > 0)
                {
                    if (Widgets.ButtonImage(new Rect(x + 3f, y + (rowHeight - 18f) / 2, 18f, 18f), TexButton.ReorderUp, Color.white, Color.white * GenUI.SubtleMouseoverColor))
                    {
                        Schedule prev = DefaultsSettings.DefaultSchedules[i - 1];
                        DefaultsSettings.DefaultSchedules[i - 1] = schedule;
                        DefaultsSettings.DefaultSchedules[i] = prev;
                        SoundDefOf.Click.PlayOneShotOnCamera(null);
                    }
                }
                x += 24f;

                schedule.name = Widgets.TextField(new Rect(x, y, labelWidth, rowHeight), schedule.name);
                x += labelWidth;

                CopyPasteUI.DoCopyPasteButtons(new Rect(x, y, copyButtonWidth * 2, rowHeight), delegate
                {
                    DefaultsSettings.DefaultSchedules.Add(new Schedule("Defaults_ScheduleName".Translate(DefaultsSettings.DefaultSchedules.Count + 1), schedule));
                }, null);
                x += copyButtonWidth;

                for (int j = 0; j < 24; j++)
                {
                    DoTimeAssignment(new Rect(x, y, cellWidth, rowHeight), schedule, j);
                    x += cellWidth;
                }

                x += 8f;
                Widgets.Checkbox(new Vector2(x, y + (rowHeight - 24f) / 2), ref schedule.use);
                x += 24f;
                
                if (DefaultsSettings.DefaultSchedules.Count > 1)
                {
                    if (Widgets.ButtonImage(new Rect(x, y + (rowHeight - 24f) / 2, 24f, 24f), TexButton.Delete, Color.white, Color.white * GenUI.SubtleMouseoverColor))
                    {
                        DefaultsSettings.DefaultSchedules.Remove(schedule);
                        i--;
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

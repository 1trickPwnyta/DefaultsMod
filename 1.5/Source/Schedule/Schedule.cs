using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.Schedule
{
    public class Schedule : IExposable
    {
        private List<string> assignments;

        public Schedule()
        {
            SetToDefaultSchedule();
        }

        private void SetToDefaultSchedule()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                if (assignments == null)
                {
                    assignments = new[]
                    {
                        TimeAssignmentDefOf.Sleep.defName,
                        TimeAssignmentDefOf.Sleep.defName,
                        TimeAssignmentDefOf.Sleep.defName,
                        TimeAssignmentDefOf.Sleep.defName,
                        TimeAssignmentDefOf.Sleep.defName,
                        TimeAssignmentDefOf.Sleep.defName,
                        TimeAssignmentDefOf.Anything.defName,
                        TimeAssignmentDefOf.Anything.defName,
                        TimeAssignmentDefOf.Anything.defName,
                        TimeAssignmentDefOf.Anything.defName,
                        TimeAssignmentDefOf.Anything.defName,
                        TimeAssignmentDefOf.Anything.defName,
                        TimeAssignmentDefOf.Anything.defName,
                        TimeAssignmentDefOf.Anything.defName,
                        TimeAssignmentDefOf.Anything.defName,
                        TimeAssignmentDefOf.Anything.defName,
                        TimeAssignmentDefOf.Anything.defName,
                        TimeAssignmentDefOf.Anything.defName,
                        TimeAssignmentDefOf.Anything.defName,
                        TimeAssignmentDefOf.Anything.defName,
                        TimeAssignmentDefOf.Anything.defName,
                        TimeAssignmentDefOf.Anything.defName,
                        TimeAssignmentDefOf.Sleep.defName,
                        TimeAssignmentDefOf.Sleep.defName
                    }.ToList();
                }
            });
        }

        public void ExposeData()
        {
            Scribe_Collections.Look(ref assignments, "Assignments");
            if (assignments == null)
            {
                SetToDefaultSchedule();
            }
        }

        public TimeAssignmentDef GetTimeAssignment(int hour)
        {
            return DefDatabase<TimeAssignmentDef>.GetNamed(assignments[hour]);
        }

        public void SetTimeAssignment(int hour, TimeAssignmentDef assignment)
        {
            assignments[hour] = assignment.defName;
        }

        public void ApplyToPawnTimetable(Pawn_TimetableTracker timetable)
        {
            if (timetable != null && timetable.times != null)
            {
                timetable.times.Clear();
                for (int i = 0; i < 24; i++)
                {
                    timetable.times.Add(GetTimeAssignment(i));
                }
            }
        }
    }
}

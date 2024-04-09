using RimWorld;
using Verse;

namespace Defaults.Schedule
{
    public static class ScheduleUtility
    {
        public static TimeAssignmentDef GetTimeAssignmentDef(TimeAssignment timeAssignment)
        {
            switch (timeAssignment)
            {
                case TimeAssignment.Anything: return TimeAssignmentDefOf.Anything;
                case TimeAssignment.Sleep: return TimeAssignmentDefOf.Sleep;
                case TimeAssignment.Work: return TimeAssignmentDefOf.Work;
                case TimeAssignment.Meditate:
                    if (ModsConfig.RoyaltyActive)
                    {
                        return TimeAssignmentDefOf.Meditate;
                    }
                    else
                    {
                        return TimeAssignmentDefOf.Anything;
                    }
                case TimeAssignment.Joy: return TimeAssignmentDefOf.Joy;
            }
            return TimeAssignmentDefOf.Anything;
        }

        public static TimeAssignment GetTimeAssignmentEnum(TimeAssignmentDef def)
        {
            if (def == TimeAssignmentDefOf.Anything) return TimeAssignment.Anything;
            if (def == TimeAssignmentDefOf.Sleep) return TimeAssignment.Sleep;
            if (def == TimeAssignmentDefOf.Work) return TimeAssignment.Work;
            if (def == TimeAssignmentDefOf.Meditate) return TimeAssignment.Meditate;
            if (def == TimeAssignmentDefOf.Joy) return TimeAssignment.Joy;
            return TimeAssignment.Anything;
        }

        public static void UpdateTimetableTracker(Pawn_TimetableTracker t)
        {
            Schedule schedule = DefaultsSettings.GetNextDefaultSchedule();
            t.times.Clear();
            for (int i = 0; i < 24; i++)
            {
                t.times.Add(GetTimeAssignmentDef(schedule.GetTimeAssignment(i)));
            }
        }
    }
}

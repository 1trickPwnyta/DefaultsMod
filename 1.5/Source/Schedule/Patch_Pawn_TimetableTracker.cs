using RimWorld;

namespace Defaults.Schedule
{
    // Patched manually in mod constructor
    public static class Patch_Pawn_TimetableTracker_ctor
    {
        public static void Postfix(Pawn_TimetableTracker __instance)
        {
            Schedule schedule = DefaultsSettings.GetNextDefaultSchedule();
            __instance.times.Clear();
            for (int i = 0; i < 24; i++)
            {
                __instance.times.Add(schedule.GetTimeAssignment(i));
            }
        }
    }
}

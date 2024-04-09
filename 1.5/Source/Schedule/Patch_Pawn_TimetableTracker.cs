using RimWorld;

namespace Defaults.Schedule
{
    // Patched manually in mod constructor
    public static class Patch_Pawn_TimetableTracker_ctor
    {
        public static void Postfix(Pawn_TimetableTracker __instance)
        {
            ScheduleUtility.UpdateTimetableTracker(__instance);
        }
    }
}

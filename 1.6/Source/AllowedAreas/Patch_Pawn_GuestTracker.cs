using HarmonyLib;
using RimWorld;
using Verse;

namespace Defaults.AllowedAreas
{
    [HarmonyPatch(typeof(Pawn_GuestTracker))]
    [HarmonyPatch(nameof(Pawn_GuestTracker.SetGuestStatus))]
    public static class Patch_Pawn_GuestTracker
    {
        public static void Prefix(Pawn ___pawn, ref AllowedPawn? __state)
        {
            __state = AllowedPawnUtility.GetAllowedPawnType(___pawn);
        }

        public static void Postfix(Pawn ___pawn, AllowedPawn? __state)
        {
            AllowedAreaUtility.SetDefaultAllowedArea(___pawn, __state);
        }
    }
}

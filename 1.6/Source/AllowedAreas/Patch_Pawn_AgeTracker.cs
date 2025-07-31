using HarmonyLib;
using Verse;

namespace Defaults.AllowedAreas
{
    [HarmonyPatchCategory("AllowedAreas")]
    [HarmonyPatch(typeof(Pawn_AgeTracker))]
    [HarmonyPatch("BirthdayBiological")]
    public static class Patch_Pawn_AgeTracker
    {
        public static void Postfix(Pawn_AgeTracker __instance, Pawn ___pawn, int birthdayAge)
        {
            if (birthdayAge == __instance.AdultMinAge && AllowedPawnUtility.GetAllowedPawnType(___pawn) == AllowedPawn.AdultColonist)
            {
                AllowedAreaUtility.SetDefaultAllowedArea(___pawn, AllowedPawn.ChildColonist);
            }
        }
    }
}

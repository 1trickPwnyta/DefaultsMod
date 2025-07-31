using HarmonyLib;
using RimWorld;
using Verse;

namespace Defaults.AllowedAreas
{
    [HarmonyPatchCategory("AllowedAreas")]
    [HarmonyPatch(typeof(MutantUtility))]
    [HarmonyPatch(nameof(MutantUtility.SetPawnAsMutantInstantly))]
    public static class Patch_MutantUtility
    {
        public static void Prefix(Pawn pawn, ref AllowedPawn? __state)
        {
            __state = AllowedPawnUtility.GetAllowedPawnType(pawn);
        }

        public static void Postfix(Pawn pawn, AllowedPawn? __state)
        {
            AllowedAreaUtility.SetDefaultAllowedArea(pawn, __state);
        }
    }
}

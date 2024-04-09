using RimWorld;
using Verse;

namespace Defaults.HostilityResponse
{
    // Patched manually in mod constructor
    public static class Patch_Pawn_PlayerSettings_ctor
    {
        public static void Postfix(Pawn_PlayerSettings __instance, Pawn pawn)
        {
            HostilityResponseModeUtility.SetHostilityResponseMode(pawn, __instance);
        }
    }
}

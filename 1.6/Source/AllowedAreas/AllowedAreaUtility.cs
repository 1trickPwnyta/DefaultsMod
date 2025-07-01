using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.AllowedAreas
{
    public static class AllowedAreaUtility
    {
        public static void SetDefaultAllowedArea(this Pawn pawn, AllowedPawn? previousAllowedPawn = null)
        {
            if (pawn.Spawned && pawn.playerSettings != null && pawn.playerSettings.SupportsAllowedAreas)
            {
                Dictionary<AllowedPawn, AllowedArea> allowedPawnAreas = Settings.Get<Dictionary<AllowedPawn, AllowedArea>>(Settings.ALLOWED_AREAS_PAWN);
                Dictionary<Map, Area> allowedAreas = pawn.playerSettings.GetType().Field("allowedAreas").GetValue(pawn.playerSettings) as Dictionary<Map, Area>;

                AllowedPawn? allowedPawn = AllowedPawnUtility.GetAllowedPawnType(pawn);
                if (allowedPawn.HasValue)
                {
                    foreach (Map map in Find.Maps)
                    {
                        if (previousAllowedPawn.HasValue)
                        {
                            if (allowedAreas[map]?.Label != allowedPawnAreas[previousAllowedPawn.Value]?.name)
                            {
                                continue;
                            }
                        }
                        allowedAreas[map] = map.areaManager.AllAreas.FirstOrDefault(a => a.Label == allowedPawnAreas[allowedPawn.Value]?.name);
                    }
                }
            }
        }
    }
}

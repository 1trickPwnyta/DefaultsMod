using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.AllowedAreas
{
    public static class AllowedAreaUtility
    {
        public static void SetDefaultAllowedArea(this Pawn pawn, AllowedPawn? previousAllowedPawn = null, Pawn mother = null)
        {
            if (pawn.playerSettings != null && pawn.playerSettings.SupportsAllowedAreas)
            {
                Dictionary<AllowedPawn, AllowedArea> allowedPawnAreas = Settings.Get<Dictionary<AllowedPawn, AllowedArea>>(Settings.ALLOWED_AREAS_PAWN);
                Dictionary<Map, Area> allowedAreas = pawn.playerSettings.GetType().Field("allowedAreas").GetValue(pawn.playerSettings) as Dictionary<Map, Area>;

                AllowedPawn? allowedPawn = AllowedPawnUtility.GetAllowedPawnType(pawn);
                if (allowedPawn.HasValue)
                {
                    Dictionary<Map, Area> inheritedAreas = null;
                    if (mother?.playerSettings != null && mother.playerSettings.SupportsAllowedAreas && (allowedPawn == AllowedPawn.ChildColonist || allowedPawn == AllowedPawn.Animal) && (allowedPawn == AllowedPawn.ChildColonist ? Settings.GetValue<bool>(Settings.CHILDREN_INHERIT_AREA_FROM_PARENT) : Settings.GetValue<bool>(Settings.ANIMALS_INHERIT_AREA_FROM_PARENT)))
                    {
                        inheritedAreas = mother.playerSettings.GetType().Field("allowedAreas").GetValue(mother.playerSettings) as Dictionary<Map, Area>;
                    }
                    foreach (Map map in Find.Maps)
                    {
                        if (previousAllowedPawn.HasValue)
                        {
                            if (allowedAreas.TryGetValue(map)?.Label != allowedPawnAreas.TryGetValue(previousAllowedPawn.Value)?.name)
                            {
                                continue;
                            }
                        }
                        allowedAreas[map] = inheritedAreas == null || !inheritedAreas.ContainsKey(map) || !map.areaManager.AllAreas.Contains(inheritedAreas[map])
                            ? map.areaManager.AllAreas.FirstOrDefault(a => a.Label == allowedPawnAreas.TryGetValue(allowedPawn.Value)?.name)
                            : inheritedAreas[map];
                    }
                }
            }
        }
    }
}

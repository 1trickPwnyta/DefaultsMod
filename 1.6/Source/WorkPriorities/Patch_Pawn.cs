using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.WorkPriorities
{
    [HarmonyPatchCategory("WorkPriorities")]
    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch(nameof(Pawn.Notify_DisabledWorkTypesChanged))]
    public static class Patch_Pawn
    {
        public static void Prefix(Pawn __instance, List<WorkTypeDef> ___cachedDisabledWorkTypes, List<WorkTypeDef> ___cachedDisabledWorkTypesPermanent)
        {
            if (___cachedDisabledWorkTypes != null)
            {
                foreach (WorkTypeDef def in ___cachedDisabledWorkTypes.ListFullCopy().Except(__instance.GetDisabledWorkTypes()))
                {
                    WorkPriorityUtility.SetWorkPrioritiesToDefault(__instance, def);
                }
            }

            if (___cachedDisabledWorkTypesPermanent != null)
            {
                foreach (WorkTypeDef def in ___cachedDisabledWorkTypesPermanent.ListFullCopy().Except(__instance.GetDisabledWorkTypes(true)))
                {
                    WorkPriorityUtility.SetWorkPrioritiesToDefault(__instance, def);
                }
            }
        }

        public static void Postfix(Pawn __instance)
        {
            if (typeof(Pawn_AgeTracker).Field("tmpEnabledWorkTypes").GetValue(null) is List<WorkTypeDef> ageWorkTypes)
            {
                foreach (WorkTypeDef def in ageWorkTypes)
                {
                    WorkPriorityUtility.SetWorkPrioritiesToDefault(__instance, def);
                }
            }
        }
    }
}

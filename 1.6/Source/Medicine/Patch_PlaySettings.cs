using HarmonyLib;

namespace Defaults.Medicine
{
    [HarmonyPatchCategory("Medicine")]
    [HarmonyPatch(typeof(RimWorld.PlaySettings))]
    [HarmonyPatch(MethodType.Constructor)]
    public static class Patch_PlaySettings_ctor
    {
        public static void Postfix(RimWorld.PlaySettings __instance)
        {
            MedicineOptions options = Settings.Get<MedicineOptions>(Settings.MEDICINE);
            __instance.defaultCareForColonist = options.DefaultCareForColonist;
            __instance.defaultCareForPrisoner = options.DefaultCareForPrisoner;
            __instance.defaultCareForSlave = options.DefaultCareForSlave;
            __instance.defaultCareForTamedAnimal = options.DefaultCareForTamedAnimal;
            __instance.defaultCareForFriendlyFaction = options.DefaultCareForFriendlyFaction;
            __instance.defaultCareForNeutralFaction = options.DefaultCareForNeutralFaction;
            __instance.defaultCareForHostileFaction = options.DefaultCareForHostileFaction;
            __instance.defaultCareForNoFaction = options.DefaultCareForNoFaction;
            __instance.defaultCareForWildlife = options.DefaultCareForWildlife;
            __instance.defaultCareForEntities = options.DefaultCareForEntities;
            __instance.defaultCareForGhouls = options.DefaultCareForGhouls;
        }
    }
}

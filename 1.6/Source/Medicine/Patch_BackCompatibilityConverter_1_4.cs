using HarmonyLib;
using Verse;

namespace Defaults.Medicine
{
    [HarmonyPatchCategory("Medicine")]
    [HarmonyPatch(typeof(BackCompatibilityConverter_1_4))]
    [HarmonyPatch(nameof(BackCompatibilityConverter_1_4.PostExposeData))]
    public static class Patch_BackCompatibilityConverter_1_4_PostExposeData
    {
        public static void Postfix(object obj)
        {
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                RimWorld.PlaySettings playSettings;
                if ((playSettings = (obj as RimWorld.PlaySettings)) != null)
                {
                    MedicineOptions options = Settings.Get<MedicineOptions>(Settings.MEDICINE);
                    playSettings.defaultCareForColonist = options.DefaultCareForColonist;
                    playSettings.defaultCareForPrisoner = options.DefaultCareForPrisoner;
                    playSettings.defaultCareForSlave = options.DefaultCareForSlave;
                    playSettings.defaultCareForTamedAnimal = options.DefaultCareForTamedAnimal;
                    playSettings.defaultCareForFriendlyFaction = options.DefaultCareForFriendlyFaction;
                    playSettings.defaultCareForNeutralFaction = options.DefaultCareForNeutralFaction;
                    playSettings.defaultCareForHostileFaction = options.DefaultCareForHostileFaction;
                    playSettings.defaultCareForNoFaction = options.DefaultCareForNoFaction;
                    playSettings.defaultCareForWildlife = options.DefaultCareForWildlife;
                    playSettings.defaultCareForEntities = options.DefaultCareForEntities;
                    playSettings.defaultCareForGhouls = options.DefaultCareForGhouls;
                }
            }
        }
    }
}

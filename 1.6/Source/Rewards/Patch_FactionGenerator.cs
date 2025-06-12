﻿using HarmonyLib;
using RimWorld;
using RimWorld.Planet;

namespace Defaults.Rewards
{
    [HarmonyPatch(typeof(FactionGenerator), nameof(FactionGenerator.NewGeneratedFaction))]
    [HarmonyPatch(new[] { typeof(PlanetLayer), typeof(FactionGeneratorParms) })]
    public static class Patch_FactionGenerator_NewGeneratedFaction
    {
        public static void Postfix(Faction __result)
        {
            if (DefaultsSettings.DefaultRewardPreferences.ContainsKey(__result.def.defName) && DefaultsSettings.DefaultRewardPreferences[__result.def.defName] != null)
            {
                RewardPreference preference = DefaultsSettings.DefaultRewardPreferences[__result.def.defName];
                __result.allowRoyalFavorRewards = preference.allowRoyalFavorRewards;
                __result.allowGoodwillRewards = preference.allowGoodwillRewards;
            }
        }
    }
}

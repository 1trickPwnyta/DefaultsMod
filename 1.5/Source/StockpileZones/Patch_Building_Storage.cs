﻿using HarmonyLib;
using RimWorld;

namespace Defaults.StockpileZones
{
    [HarmonyPatch(typeof(Building_Storage))]
    [HarmonyPatch(nameof(Building_Storage.PostMake))]
    public static class Patch_Building_Storage
    {
        public static void Postfix(Building_Storage __instance)
        {
            __instance.settings.Priority = DefaultsSettings.DefaultShelfSettings.Priority;
            __instance.settings.filter.CopyAllowancesFrom(DefaultsSettings.DefaultShelfSettings.filter);
        }
    }
}

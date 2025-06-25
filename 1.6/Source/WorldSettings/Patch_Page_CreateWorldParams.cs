using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults.WorldSettings
{
    [HarmonyPatch(typeof(Page_CreateWorldParams))]
    [HarmonyPatch(nameof(Page_CreateWorldParams.Reset))]
    public static class Patch_Page_CreateWorldParams_Reset
    {
        public static void Postfix(ref float ___planetCoverage, ref OverallRainfall ___rainfall, ref OverallTemperature ___temperature, ref OverallPopulation ___population, ref float ___pollution)
        {
            PlanetOptions options = Settings.Get<PlanetOptions>(Settings.PLANET);
            ___planetCoverage = options.DefaultPlanetCoverage;
            ___rainfall = options.DefaultOverallRainfall;
            ___temperature = options.DefaultOverallTemperature;
            ___population = options.DefaultOverallPopulation;
            ___pollution = options.DefaultPollution;
        }
    }

    [HarmonyPatch(typeof(Page_CreateWorldParams))]
    [HarmonyPatch("ResetFactionCounts")]
    public static class Patch_Page_CreateWorldParams_ResetFactionCounts
    {
        public static void Postfix(ref List<FactionDef> ___factions)
        {
            ___factions = Settings.Get<List<FactionDef>>(Settings.FACTIONS).Where(f => f != null && f.displayInFactionSelection).Concat(FactionsUtility.GetDefaultNonselectableFactions()).ToList();
        }
    }

    [HarmonyPatch(typeof(Page_CreateWorldParams))]
    [HarmonyPatch(nameof(Page_CreateWorldParams.DoWindowContents))]
    public static class Patch_Page_CreateWorldParams_DoWindowContents
    {
        public static void Postfix(Rect rect, float ___planetCoverage, OverallRainfall ___rainfall, OverallTemperature ___temperature, OverallPopulation ___population, float ___pollution, List<FactionDef> ___factions)
        {
            Rect buttonRect = new Rect(rect.x + rect.width - 150f - 16f, rect.y + 4f, 150f, 40f);
            if (Widgets.ButtonText(buttonRect, "Defaults_SetAsDefault".Translate()))
            {
                PlanetOptions options = Settings.Get<PlanetOptions>(Settings.PLANET);
                options.DefaultPlanetCoverage = ___planetCoverage;
                options.DefaultOverallRainfall = ___rainfall;
                options.DefaultOverallTemperature = ___temperature;
                options.DefaultOverallPopulation = ___population;
                options.DefaultPollution = ___pollution;
                Settings.Set(Settings.FACTIONS, ___factions.Where(f => f.displayInFactionSelection).ToList());
                DefaultsMod.Settings.Write();
                Messages.Message("Defaults_SetAsDefaultConfirmed".Translate(), MessageTypeDefOf.PositiveEvent, false);
            }
        }
    }

    [HarmonyPatch(typeof(Page_CreateWorldParams))]
    [HarmonyPatch(nameof(Page_CreateWorldParams.PostOpen))]
    public static class Patch_Page_CreateWorldParams_PostOpen
    {
        public static void Postfix()
        {
            MapOptions options = Settings.Get<MapOptions>(Settings.MAP);
            Find.GameInitData.mapSize = options.DefaultMapSize;
            Find.GameInitData.startingSeason = options.DefaultStartingSeason;
        }
    }
}

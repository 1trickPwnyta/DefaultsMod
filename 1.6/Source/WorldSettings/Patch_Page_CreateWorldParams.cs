using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults.WorldSettings
{
    [HarmonyPatchCategory("World")]
    [HarmonyPatch(typeof(Page_CreateWorldParams))]
    [HarmonyPatch(nameof(Page_CreateWorldParams.Reset))]
    public static class Patch_Page_CreateWorldParams_Reset
    {
        public static void Postfix(ref float ___planetCoverage, ref OverallRainfall ___rainfall, ref OverallTemperature ___temperature, ref OverallPopulation ___population, ref float ___pollution, ref LandmarkDensity ___landmarkDensity)
        {
            PlanetOptions options = Settings.Get<PlanetOptions>(Settings.PLANET);
            ___planetCoverage = options.DefaultPlanetCoverage;
            ___rainfall = options.DefaultOverallRainfall;
            ___temperature = options.DefaultOverallTemperature;
            ___population = options.DefaultOverallPopulation;
            ___pollution = options.DefaultPollution;
            ___landmarkDensity = options.DefaultLandmarkDensity;
        }
    }

    [HarmonyPatchCategory("World")]
    [HarmonyPatch(typeof(Page_CreateWorldParams))]
    [HarmonyPatch("ResetFactionCounts")]
    public static class Patch_Page_CreateWorldParams_ResetFactionCounts
    {
        public static void Postfix(ref List<FactionDef> ___factions)
        {
            ___factions = Settings.Get<List<FactionDef>>(Settings.FACTIONS).Where(f => f != null && f.displayInFactionSelection).Concat(FactionsUtility.GetDefaultNonselectableFactions()).ToList();
            foreach (FactionDef faction in FactionsUtility.GetDefaultSelectableFactions())
            {
                if (!___factions.Contains(faction) && Current.Game.Scenario.AllParts.Any(p => p.def.preventRemovalOfFaction == faction))
                {
                    ___factions.Add(faction);
                }
            }
        }
    }

    [HarmonyPatchCategory("World")]
    [HarmonyPatch(typeof(Page_CreateWorldParams))]
    [HarmonyPatch(nameof(Page_CreateWorldParams.DoWindowContents))]
    public static class Patch_Page_CreateWorldParams_DoWindowContents
    {
        public static void Postfix(Rect rect, float ___planetCoverage, OverallRainfall ___rainfall, OverallTemperature ___temperature, OverallPopulation ___population, float ___pollution, LandmarkDensity ___landmarkDensity, List<FactionDef> ___factions)
        {
            if (!Settings.GetValue<bool>(Settings.HIDE_SETASDEFAULT))
            {
                Rect buttonRect = new Rect(rect.x + rect.width - 150f - 16f, rect.y + 4f, 150f, 40f);
                if (Widgets.ButtonText(buttonRect, "Defaults_SetAsDefault".Translate()))
                {
                    PlanetOptions planetOptions = Settings.Get<PlanetOptions>(Settings.PLANET);
                    planetOptions.DefaultPlanetCoverage = ___planetCoverage;
                    planetOptions.DefaultOverallRainfall = ___rainfall;
                    planetOptions.DefaultOverallTemperature = ___temperature;
                    planetOptions.DefaultOverallPopulation = ___population;
                    planetOptions.DefaultPollution = ___pollution;
                    planetOptions.DefaultLandmarkDensity = ___landmarkDensity;
                    MapOptions mapOptions = Settings.Get<MapOptions>(Settings.MAP);
                    mapOptions.DefaultMapSize = Find.GameInitData.mapSize;
                    mapOptions.DefaultStartingSeason = Find.GameInitData.startingSeason;
                    Settings.Set(Settings.FACTIONS, ___factions.Where(f => f.displayInFactionSelection).ToList());
                    DefaultsMod.Settings.Write();
                    Messages.Message("Defaults_SetAsDefaultConfirmed".Translate(), MessageTypeDefOf.PositiveEvent, false);
                }
            }
        }
    }

    [HarmonyPatchCategory("World")]
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

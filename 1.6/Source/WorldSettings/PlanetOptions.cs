using RimWorld.Planet;
using Verse;

namespace Defaults.WorldSettings
{
    public class PlanetOptions : IExposable
    {
        public float DefaultPlanetCoverage = ModsConfig.OdysseyActive ? 0.5f : 0.3f;
        public OverallRainfall DefaultOverallRainfall = OverallRainfall.Normal;
        public OverallTemperature DefaultOverallTemperature = OverallTemperature.Normal;
        public OverallPopulation DefaultOverallPopulation = OverallPopulation.Normal;
        public LandmarkDensity DefaultLandmarkDensity = LandmarkDensity.Normal;
        public float DefaultPollution = 0.05f;

        public void ExposeData()
        {
            Scribe_Values.Look(ref DefaultPlanetCoverage, "DefaultPlanetCoverage", ModsConfig.OdysseyActive ? 0.5f : 0.3f);
            Scribe_Values.Look(ref DefaultOverallRainfall, "DefaultOverallRainfall", OverallRainfall.Normal);
            Scribe_Values.Look(ref DefaultOverallTemperature, "DefaultOverallTemperature", OverallTemperature.Normal);
            Scribe_Values.Look(ref DefaultOverallPopulation, "DefaultOverallPopulation", OverallPopulation.Normal);
            Scribe_Values.Look(ref DefaultLandmarkDensity, "DefaultLandmarkDensity", LandmarkDensity.Normal);
            Scribe_Values.Look(ref DefaultPollution, "DefaultPollution", 0.05f);
        }
    }
}

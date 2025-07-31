using Defaults.Defs;
using Defaults.MapSettings;
using Defaults.UI;
using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults.WorldSettings
{
    public class Dialog_WorldSettings : Dialog_SettingsCategory
    {
        private static readonly List<float> PlanetCoverages = new List<float>
        {
            0.3f,
            0.5f,
            1f
        };

        public Dialog_WorldSettings(DefaultSettingsCategoryDef category) : base(category)
        {
        }

        public override Vector2 InitialSize => Page.StandardSize;

        protected override bool DoSettingsWhenDisabled => false;

        public override void DoSettings(Rect rect)
        {
            Rect mainRect = new Rect(rect.x, rect.y, rect.width, rect.height - CloseButSize.y);
            Rect rect2 = new Rect(mainRect.x, mainRect.y, mainRect.width / 2, mainRect.height);
            Widgets.BeginGroup(rect2);
            Text.Font = GameFont.Small;
            float y = 0f;
            float widgetWidth = rect2.width - 200f;
            PlanetOptions options = Settings.Get<PlanetOptions>(Settings.PLANET);

            Widgets.Label(new Rect(0f, y, 200f, 30f), "PlanetCoverage".Translate());
            Rect rect4 = new Rect(200f, y, widgetWidth, 30f);
            if (Widgets.ButtonText(rect4, options.DefaultPlanetCoverage.ToStringPercent()))
            {
                Find.WindowStack.Add(new FloatMenu(PlanetCoverages.Select(c => new FloatMenuOption(c.ToStringPercent(), () =>
                {
                    if (options.DefaultPlanetCoverage != c)
                    {
                        options.DefaultPlanetCoverage = c;
                        if (options.DefaultPlanetCoverage == 1f)
                        {
                            Messages.Message("MessageMaxPlanetCoveragePerformanceWarning".Translate(), MessageTypeDefOf.CautionInput, false);
                        }
                    }
                })).ToList()));
            }
            TooltipHandler.TipRegionByKey(new Rect(0f, y, rect4.xMax, rect4.height), "PlanetCoverageTip");
            y += 40f;

            Widgets.Label(new Rect(0f, y, 200f, 30f), "PlanetRainfall".Translate());
            Rect rect5 = new Rect(200f, y, widgetWidth, 30f);
            options.DefaultOverallRainfall = (OverallRainfall)Mathf.RoundToInt(Widgets.HorizontalSlider(rect5, (float)options.DefaultOverallRainfall, 0f, OverallRainfallUtility.EnumValuesCount - 1, true, "PlanetRainfall_Normal".Translate(), "PlanetRainfall_Low".Translate(), "PlanetRainfall_High".Translate(), 1f));
            y += 40f;

            Widgets.Label(new Rect(0f, y, 200f, 30f), "PlanetTemperature".Translate());
            Rect rect6 = new Rect(200f, y, widgetWidth, 30f);
            options.DefaultOverallTemperature = (OverallTemperature)Mathf.RoundToInt(Widgets.HorizontalSlider(rect6, (float)options.DefaultOverallTemperature, 0f, OverallTemperatureUtility.EnumValuesCount - 1, true, "PlanetTemperature_Normal".Translate(), "PlanetTemperature_Low".Translate(), "PlanetTemperature_High".Translate(), 1f));
            y += 40f;

            Widgets.Label(new Rect(0f, y, 200f, 30f), "PlanetPopulation".Translate());
            Rect rect7 = new Rect(200f, y, widgetWidth, 30f);
            options.DefaultOverallPopulation = (OverallPopulation)Mathf.RoundToInt(Widgets.HorizontalSlider(rect7, (float)options.DefaultOverallPopulation, 0f, OverallPopulationUtility.EnumValuesCount - 1, true, "PlanetPopulation_Normal".Translate(), "PlanetPopulation_Low".Translate(), "PlanetPopulation_High".Translate(), 1f));
            y += 40f;

            if (ModsConfig.OdysseyActive)
            {
                Widgets.Label(new Rect(0f, y, 200f, 30f), "PlanetLandmarkDensity".Translate());
                Rect landmarkRect = new Rect(200f, y, widgetWidth, 30f);
                options.DefaultLandmarkDensity = (LandmarkDensity)Mathf.RoundToInt(Widgets.HorizontalSlider(landmarkRect, (float)options.DefaultLandmarkDensity, 0f, LandmarkDensityUtility.EnumValuesCount - 1, true, "PlanetLandmarkDensity_Normal".Translate(), "PlanetLandmarkDensity_Low".Translate(), "PlanetLandmarkDensity_High".Translate(), 1f));
                y += 40f;
            }

            if (ModsConfig.BiotechActive)
            {
                Widgets.Label(new Rect(0f, y, 200f, 30f), "PlanetPollution".Translate());
                Rect rect8 = new Rect(200f, y, widgetWidth, 30f);
                options.DefaultPollution = Widgets.HorizontalSlider(rect8, options.DefaultPollution, 0f, 1f, true, options.DefaultPollution.ToStringPercent(), null, null, 0.05f);
                y += 40f;
            }

            Widgets.Label(new Rect(0f, y, 200f, 30f), "AdvancedSettings".Translate());
            if (Widgets.ButtonText(new Rect(200f, y, widgetWidth, 30f), "Edit".Translate() + "..."))
            {
                Find.WindowStack.Add(new Dialog_MapSettings());
            }

            Widgets.EndGroup();

            Rect rect9 = mainRect.RightPartPixels(mainRect.width / 2 - 24f);
            WorldFactionsUIUtility.DoWindowContents(rect9, Settings.Get<List<FactionDef>>(Settings.FACTIONS), true);
            Rect rect10 = new Rect(rect9.xMax - 24f, rect9.y, 24f, 24f);
            bool factionsLock = Settings.Get<bool>(Settings.FACTIONS_LOCK);
            UIUtility.DrawCheckButton(rect10, UIUtility.LockIcon, "Defaults_LockSetting".Translate(), ref factionsLock);
            Settings.Set(Settings.FACTIONS_LOCK, factionsLock);
        }
    }
}

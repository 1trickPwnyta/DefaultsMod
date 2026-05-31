using Defaults.Defs;
using Defaults.SaveLoad;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace Defaults
{
    public class DefaultsSettings : ModSettings
    {
        public static List<string> KnownDLCs;
        private static List<FactionDef> PreviousFactionDefs;
        private static List<ThingDef> PreviousThingDefs;
        private static List<SpecialThingFilterDef> PreviousSpecialThingFilterDefs;

        private static readonly List<DefaultSettingsCategoryDef> categories = DefDatabase<DefaultSettingsCategoryDef>.AllDefsListForReading;

        public static void ResetAllSettings()
        {
            foreach (DefaultSettingsCategoryDef def in categories)
            {
                def.Worker.ResetSettings();
            }
            SoundDefOf.GameStartSting.PlayOneShot(null);
            Messages.Message("Defaults_ResetAllSettingsComplete".Translate(), MessageTypeDefOf.SilentInput, false);
        }

        public static void CheckForNewContent()
        {
            HandleNewDefs(ref PreviousFactionDefs);
            HandleNewDefs(ref PreviousThingDefs);
            HandleNewDefs(ref PreviousSpecialThingFilterDefs);
            DefaultsMod.SaveSettings(false);
        }

        private static void HandleNewDefs<T>(ref List<T> previousDefs) where T : Def
        {
            if (previousDefs == null)
            {
                previousDefs = new List<T>();
            }
            List<T> currentDefs = DefDatabase<T>.AllDefsListForReading;
            List<T> newDefs = currentDefs.Except(previousDefs).ToList();
            if (newDefs.Any() && previousDefs.Any())
            {
                foreach (DefaultSettingsCategoryDef def in categories)
                {
                    def.Worker.HandleNewDefs(newDefs);
                }
            }
            previousDefs.AddRange(newDefs);
        }

        public static void PreLoadSettings()
        {
            foreach (DefaultSettingsCategoryDef def in categories)
            {
                def.Worker.PreLoad();
            }
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref KnownDLCs, "KnownDLCs");
            Scribe_Collections_Silent.Look(ref PreviousFactionDefs, "PreviousFactionDefs");
            Scribe_Collections_Silent.Look(ref PreviousThingDefs, "PreviousThingDefs");
            Scribe_Collections_Silent.Look(ref PreviousSpecialThingFilterDefs, "PreviousSpecialThingFilterDefs");

            foreach (DefaultSettingsCategoryDef def in categories)
            {
                def.Worker.ExposeData();
            }
        }
    }
}

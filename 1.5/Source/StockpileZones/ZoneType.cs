using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.StockpileZones
{
    public class ZoneType : IExposable
    {
        public string Name;
        public ThingFilter filter = new ThingFilter();

        public ZoneType()
        {

        }

        public ZoneType(string name, StorageSettingsPreset preset)
        {
            Name = name;
            StorageSettings settings = new StorageSettings();
            settings.SetFromPreset(preset);
            CopyFrom(settings);
        }

        public ZoneType(string name, StorageSettings settings)
        {
            Name = name;
            CopyFrom(settings);
        }

        public void CopyFrom(StorageSettings settings)
        {
            filter.CopyAllowancesFrom(settings.filter);
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref Name, "Name");
            List<string> allowed = filter.AllowedThingDefs.Select(d => d.defName).ToList();
            Scribe_Collections.Look(ref allowed, "Allowed");
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                foreach (string allowedThingDef in allowed)
                {
                    filter.SetAllow(DefDatabase<ThingDef>.GetNamed(allowedThingDef), true);
                }
            });
        }
    }
}

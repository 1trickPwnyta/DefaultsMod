using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Defaults.StockpileZones.Buildings
{
    public static class BuildingUtility
    {
        public static void SetDefaultBuildingStorageSettings(ThingDef def, IStoreSettingsParent parent)
        {
            ZoneType zone = Settings.Get<Dictionary<ThingDef, ZoneType_Building>>(Settings.BUILDING_STORAGE).TryGetValue(def);
            if (zone != null)
            {
                StorageSettings settings = parent.GetStoreSettings();
                if (settings != null)
                {
                    settings.Priority = zone.priority;
                    settings.filter.CopyAllowancesFrom(zone.filter);
                }
            }
        }
    }
}

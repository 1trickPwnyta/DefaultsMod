using RimWorld;
using Verse;
using UnityEngine;

namespace Defaults.StockpileZones.Shelves
{
    public class ZoneType_Shelf : ZoneType
    {
        public ZoneType_Shelf()
        {
            Initialize();
        }

        public ZoneType_Shelf(ZoneType other)
        {
            Initialize();
            Priority = other.Priority;
            filter.CopyAllowancesFrom(other.filter);
        }

        private void Initialize()
        {
            Name = "Defaults_ShelfSettings".Translate();
            IconColor = ThingDefOf.WoodLog.stuffProps.color;
        }

        public override Texture2D Icon => ThingDef.Named("ShelfSmall").uiIcon;

        public override void ExposeData()
        {
            base.ExposeData();
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                Initialize();
            }
        }
    }
}

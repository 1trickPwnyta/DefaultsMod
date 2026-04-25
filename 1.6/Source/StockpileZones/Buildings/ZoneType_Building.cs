using Defaults.Defs;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Defaults.StockpileZones.Buildings
{
    public class ZoneType_Building : ZoneType
    {
        private static readonly List<ThingDef> buildingsUnlockedByDefault = new List<ThingDef>()
        {
            ThingDefOf.Shelf,
            ThingDefOf.ShelfSmall,
            ThingDef.Named("Bookcase"),
            ThingDef.Named("BookcaseSmall"),
            ThingDefOf.Hopper,
            ThingDefOf.GrowthVat,
            ThingDefOf.BiosculpterPod,
            ThingDef.Named("Artillery_Mortar")
        };

        public ThingDef buildingDef;
        private ThingDef iconDef;

        public ZoneType_Building()
        {
        }

        public ZoneType_Building(ThingDef buildingDef)
        {
            this.buildingDef = buildingDef;
            SetToDefault();
            FindIconDef();
            locked = !buildingsUnlockedByDefault.Contains(buildingDef);
        }

        public ZoneType_Building(ThingDef buildingDef, ZoneType other) : this(buildingDef)
        {
            priority = other.priority;
            filter.CopyAllowancesFrom(other.filter);
            locked = other.locked;
        }

        private void SetToDefault()
        {
            priority = buildingDef.building.defaultStorageSettings.Priority;
            filter.CopyAllowancesFrom(buildingDef.building.defaultStorageSettings.filter);
        }

        private void FindIconDef()
        {
            iconDef = DefDatabase<ThingDef>.AllDefsListForReading.FirstOrDefault(d => d.building?.turretGunDef == buildingDef) ?? buildingDef;
        }

        public override string Name => "Defaults_StorageBuildingSettings".Translate(buildingDef.LabelCap);

        public override Texture2D Icon => iconDef.uiIcon;

        public override Color IconColor => GenStuff.DefaultStuffFor(iconDef)?.stuffProps.color ?? Color.white;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs_Silent.Look(ref buildingDef, "buildingDef");
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                FindIconDef();
            }
        }
    }
}

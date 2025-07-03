using Defaults.Defs;
using System.Collections.Generic;
using Verse;

namespace Defaults.AllowedAreas
{
    public class Comp_SpawnTracker : ThingComp
    {
        private HashSet<Map> spawnedOnMapEver = new HashSet<Map>();

        public bool EverSpawnedOnMap(Map map) => spawnedOnMapEver.Contains(map);

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            if (DefaultSettingsCategoryDefOf.AllowedAreas.Enabled)
            {
                if (!EverSpawnedOnMap(parent.Map))
                {
                    AllowedAreaUtility.SetDefaultAllowedArea(parent as Pawn);
                }
                spawnedOnMapEver.Add(parent.Map);
            }
        }

        public override void PostExposeData()
        {
            if (DefaultSettingsCategoryDefOf.AllowedAreas.Enabled)
            {
                if (Scribe.mode == LoadSaveMode.Saving)
                {
                    spawnedOnMapEver.RemoveWhere(m => !Find.Maps.Contains(m));
                }
                Scribe_Collections.Look(ref spawnedOnMapEver, "spawnedOnMapEver", LookMode.Reference);
                if (Scribe.mode == LoadSaveMode.PostLoadInit && spawnedOnMapEver == null)
                {
                    spawnedOnMapEver = new HashSet<Map>();
                }
            }
        }
    }
}

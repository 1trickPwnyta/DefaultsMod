using RimWorld;
using UnityEngine;
using Verse;

namespace Defaults.PlantType
{
    public class DefaultSettingWorker_PlantType : DefaultSettingWorker<ThingDef>
    {
        public DefaultSettingWorker_PlantType(DefaultSettingDef def) : base(def)
        {
        }

        public override string Key => Settings.PLANT_TYPE;

        protected override ThingDef Default => ThingDefOf.Plant_Potato;

        protected override void DoWidget(Rect rect)
        {
            rect.x += rect.width - 24f;
            rect.width = 24f;
            PlantTypeUtility.DrawPlantButton(rect);
        }

        protected override void ExposeSetting()
        {
            Scribe_Defs.Look(ref setting, Key);
        }
    }
}

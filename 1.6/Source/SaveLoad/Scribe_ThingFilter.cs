using RimWorld;
using Verse;

namespace Defaults.SaveLoad
{
    public static class Scribe_ThingFilter
    {
        private static void ScribeFields(ThingFilter value)
        {
            Scribe_Fields.Look_List<SpecialThingFilterDef>(value, "disallowedSpecialFilters");
            Scribe_Fields.Look_HashSet<ThingDef>(value, "allowedDefs");
            Scribe_Fields.Look_Value<FloatRange>(value, "allowedHitPointsPercents");
            Scribe_Fields.Look_Value<FloatRange>(value, "allowedMentalBreakChance");
            Scribe_Fields.Look_Value<QualityRange>(value, "allowedQualities", "allowedQualityLevels");
            Scribe_Fields.Look_Value(value, "onlySpecialFilters", defaultValue: false);
            Scribe_Fields.Look_Def<ThingCategoryDef>(value, "overrideRootDef");
        }

        public static void Look(ref ThingFilter value, string label)
        {
            SaveLoadUtility.Scribe_Custom(ref value, label, ScribeFields);
        }
    }
}

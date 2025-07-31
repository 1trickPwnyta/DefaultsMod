using RimWorld;
using Verse;

namespace Defaults.Medicine
{
    public class MedicineOptions : IExposable
    {
        public MedicalCareCategory DefaultCareForColonist = MedicalCareCategory.Best;
        public MedicalCareCategory DefaultCareForPrisoner = MedicalCareCategory.HerbalOrWorse;
        public MedicalCareCategory DefaultCareForSlave = MedicalCareCategory.HerbalOrWorse;
        public MedicalCareCategory DefaultCareForGhouls = MedicalCareCategory.NoMeds;
        public MedicalCareCategory DefaultCareForTamedAnimal = MedicalCareCategory.HerbalOrWorse;
        public MedicalCareCategory DefaultCareForFriendlyFaction = MedicalCareCategory.HerbalOrWorse;
        public MedicalCareCategory DefaultCareForNeutralFaction = MedicalCareCategory.HerbalOrWorse;
        public MedicalCareCategory DefaultCareForHostileFaction = MedicalCareCategory.HerbalOrWorse;
        public MedicalCareCategory DefaultCareForNoFaction = MedicalCareCategory.HerbalOrWorse;
        public MedicalCareCategory DefaultCareForWildlife = MedicalCareCategory.HerbalOrWorse;
        public MedicalCareCategory DefaultCareForEntities = MedicalCareCategory.NoMeds;

        public void ExposeData()
        {
            Scribe_Values.Look(ref DefaultCareForColonist, "DefaultCareForColonist", MedicalCareCategory.Best);
            Scribe_Values.Look(ref DefaultCareForPrisoner, "DefaultCareForPrisoner", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.Look(ref DefaultCareForSlave, "DefaultCareForSlave", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.Look(ref DefaultCareForGhouls, "DefaultCareForGhouls", MedicalCareCategory.NoMeds);
            Scribe_Values.Look(ref DefaultCareForTamedAnimal, "DefaultCareForTamedAnimal", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.Look(ref DefaultCareForFriendlyFaction, "DefaultCareForFriendlyFaction", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.Look(ref DefaultCareForNeutralFaction, "DefaultCareForNeutralFaction", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.Look(ref DefaultCareForHostileFaction, "DefaultCareForHostileFaction", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.Look(ref DefaultCareForNoFaction, "DefaultCareForNoFaction", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.Look(ref DefaultCareForWildlife, "DefaultCareForWildlife", MedicalCareCategory.HerbalOrWorse);
            Scribe_Values.Look(ref DefaultCareForEntities, "DefaultCareForEntities", MedicalCareCategory.NoMeds);
        }
    }
}

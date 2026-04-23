using RimWorld;
using Verse;

namespace Defaults.Medicine
{
    public static class MedicineUtility
    {
        public static void SetMedicalDefaults(Pawn pawn, Pawn_InventoryStockTracker inventoryStock)
        {
            if (pawn.IsColonist)
            {
                if (inventoryStock != null)
                {
                    inventoryStock.SetThingForGroup(InventoryStockGroupDefOf.Medicine, Settings.Get<ThingDef>(Settings.MEDICINE_TO_CARRY));
                    if (Settings.GetValue<bool>(Settings.GUESTS_CARRY_MEDICINE) || (!pawn.HasExtraMiniFaction() && !pawn.HasExtraHomeFaction()))
                    {
                        inventoryStock.SetCountForGroup(InventoryStockGroupDefOf.Medicine, Settings.GetValue<int>(Settings.MEDICINE_AMOUNT_TO_CARRY));
                    }
                }

                int? selfTendLevel = Settings.Get<int?>(Settings.SELF_TEND_LEVEL);
                if (selfTendLevel != null)
                {
                    if (pawn.skills.GetSkill(SkillDefOf.Medicine).Level >= selfTendLevel)
                    {
                        pawn.playerSettings.selfTend = true;
                    }
                }
            }
        }
    }
}

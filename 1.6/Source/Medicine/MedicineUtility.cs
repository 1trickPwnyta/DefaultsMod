using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Defaults.Medicine
{
    public static class MedicineUtility
    {
        public static void SetMedicineToCarry(Pawn pawn, Pawn_InventoryStockTracker inventoryStock)
        {
            if (inventoryStock != null && pawn.Faction == Faction.OfPlayer && !pawn.IsGhoul)
            {
                inventoryStock.SetThingForGroup(InventoryStockGroupDefOf.Medicine, Settings.Get<ThingDef>(Settings.MEDICINE_TO_CARRY));
                if (Settings.GetValue<bool>(Settings.GUESTS_CARRY_MEDICINE) || (!pawn.HasExtraMiniFaction() && !pawn.HasExtraHomeFaction()))
                {
                    inventoryStock.SetCountForGroup(InventoryStockGroupDefOf.Medicine, Settings.GetValue<int>(Settings.MEDICINE_AMOUNT_TO_CARRY));
                }
            }
        }

        public static void DrawMedicineButton(Rect rect)
        {
            ThingDef currentMedicineDef = Settings.Get<ThingDef>(Settings.MEDICINE_TO_CARRY);
            Widgets.Dropdown(rect, null, new Color(0.84f, 0.84f, 0.84f), new Func<object, ThingDef>(DrawResponseButton_GetResponse), new Func<object, IEnumerable<Widgets.DropdownMenuElement<ThingDef>>>(DrawResponseButton_GenerateMenu), null, currentMedicineDef.uiIcon, null, null, null, true, new float?(4f));
            if (Mouse.IsOver(rect))
            {
                TooltipHandler.TipRegion(rect, "Defaults_MedicineToCarryTip".Translate() + "\n\n" + "Defaults_CurrentMedicineToCarry".Translate() + ": " + currentMedicineDef.LabelCap);
            }
        }

        private static ThingDef DrawResponseButton_GetResponse(object obj)
        {
            return Settings.Get<ThingDef>(Settings.MEDICINE_TO_CARRY);
        }

        private static IEnumerable<Widgets.DropdownMenuElement<ThingDef>> DrawResponseButton_GenerateMenu(object obj)
        {
            IEnumerable<ThingDef> choices = InventoryStockGroupDefOf.Medicine.thingDefs;
            foreach (ThingDef choice in choices)
            {
                string text = choice.LabelCap;
                yield return new Widgets.DropdownMenuElement<ThingDef>
                {
                    option = new FloatMenuOption(text, delegate ()
                    {
                        Settings.Set(Settings.MEDICINE_TO_CARRY, choice);
                    }, choice.uiIcon, Color.white),
                    payload = choice
                };
            }
        }
    }
}

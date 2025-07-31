﻿using HarmonyLib;
using Verse;

namespace Defaults.Medicine
{
    [HarmonyPatchCategory("Medicine")]
    [HarmonyPatch(typeof(PawnGenerator))]
    [HarmonyPatch("GenerateNewPawnInternal")]
    public static class Patch_PawnGenerator_GenerateNewPawnInternal
    {
        public static void Postfix(Pawn __result)
        {
            MedicineUtility.SetMedicineToCarry(__result, __result.inventoryStock);
        }
    }
}

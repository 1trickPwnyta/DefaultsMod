﻿using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults.WorkbenchBills
{
    [HarmonyPatchCategory("WorkbenchBills")]
    [HarmonyPatch(typeof(BillStack))]
    [HarmonyPatch(nameof(BillStack.DoListing))]
    public static class Patch_BillStack
    {
        public static void Postfix(BillStack __instance, Rect rect)
        {
            if (!Settings.GetValue<bool>(Settings.HIDE_LOADDEFAULT))
            {
                IEnumerable<BillTemplate> defaultBills = Settings.Get<List<WorkbenchBillStore>>(Settings.WORKBENCH_BILLS).Where(s => s.workbenchGroup.Contains(((Thing)__instance.billGiver).def)).SelectMany(s => s.bills);
                if ((__instance.Count < 15 || !Settings.Get<GlobalBillOptions>(Settings.GLOBAL_BILL_OPTIONS).LimitBillsTo15) && defaultBills.Count() > 0)
                {
                    Rect loadDefaultRect = new Rect(rect.x + 150f, rect.y, 150f, 29f);
                    if (Widgets.ButtonText(loadDefaultRect, "Defaults_LoadDefaultBill".Translate()))
                    {
                        Find.WindowStack.Add(new FloatMenu(defaultBills.Select(b => new FloatMenuOption(b.name, () =>
                        {
                            __instance.AddBill(b.ToBill());
                        }, b.recipe.UIIconThing, b.recipe.UIIcon, null, true, MenuOptionPriority.Default, null, null, 29f, r => Widgets.InfoCardButton(r.x + 5f, r.y + (r.height - 24f) / 2f, b.recipe))).ToList()));
                    }
                }
            }
        }
    }
}

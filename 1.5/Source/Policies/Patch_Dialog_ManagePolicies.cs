using Defaults.Policies.ApparelPolicies;
using Defaults.Policies.DrugPolicies;
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace Defaults.Policies
{
    [HarmonyPatch(typeof(Dialog_ManagePolicies<Policy>))]
    [HarmonyPatch(nameof(Dialog_ManagePolicies<Policy>.DoWindowContents))]
    public static class Patch_Dialog_ManagePolicies_DoWindowContents
    {
        public static void Postfix(Dialog_ManagePolicies<Policy> __instance, Rect inRect, Policy ___policyInt)
        {
            if (___policyInt != null && __instance.GetType() != typeof(Dialog_ApparelPolicies) && __instance.GetType() != typeof(Dialog_DrugPolicies))
            {
                Rect saveAsDefaultRect = new Rect(inRect.xMax - 158f, inRect.y + 74f, 32f, 32f);
                if (Widgets.ButtonImage(saveAsDefaultRect, TexButton.Save))
                {
                    string name = ___policyInt.label;

                    if (___policyInt is RimWorld.ApparelPolicy)
                    {
                        int i = DefaultsSettings.DefaultApparelPolicies.Count + 1;
                        while (DefaultsSettings.DefaultApparelPolicies.Any(p => p.label == name))
                        {
                            name = "ApparelPolicy".Translate() + " " + i++;
                        }
                        ApparelPolicies.ApparelPolicy policy = new ApparelPolicies.ApparelPolicy(0, name);
                        policy.filter.CopyAllowancesFrom(((RimWorld.ApparelPolicy)___policyInt).filter);
                        DefaultsSettings.DefaultApparelPolicies.Add(policy);
                    }

                    if (___policyInt is DrugPolicy)
                    {
                        int i = DefaultsSettings.DefaultDrugPolicies.Count + 1;
                        while (DefaultsSettings.DefaultDrugPolicies.Any(p => p.label == name))
                        {
                            name = "DrugPolicy".Translate() + " " + i++;
                        }
                        DrugPolicy policy = new DrugPolicy(0, name);
                        policy.CopyFrom(___policyInt);
                        DefaultsSettings.DefaultDrugPolicies.Add(policy);
                    }

                    LongEventHandler.ExecuteWhenFinished(DefaultsMod.Settings.Write);
                    Messages.Message("Defaults_PolicySavedAs".Translate(name), MessageTypeDefOf.PositiveEvent, false);
                }
                TooltipHandler.TipRegionByKey(saveAsDefaultRect, "Defaults_SaveNewDefaultPolicy");
            }
        }
    }

    [HarmonyPatch(typeof(Dialog_ManagePolicies<Policy>))]
    [HarmonyPatch("DoPolicyListing")]
    public static class Patch_Dialog_ManagePolicies_DoPolicyListing
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool foundQuickSearch = false;
            CodeInstruction targetInstruction = null;

            foreach (CodeInstruction instruction in instructions.Reverse())
            {
                if (instruction.opcode == OpCodes.Callvirt && (MethodInfo)instruction.operand == DefaultsRefs.m_QuickSearchWidget_OnGUI)
                {
                    foundQuickSearch = true;
                }
                if (foundQuickSearch && instruction.opcode == OpCodes.Ldc_R4)
                {
                    targetInstruction = instruction;
                    break;
                }
            }

            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction == targetInstruction)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, DefaultsRefs.m_Dialog_ManagePolicies_GetDefaultPolicy);
                    yield return new CodeInstruction(OpCodes.Call, DefaultsRefs.m_PolicyUtility_GetNewPolicyButtonPaddingTop);
                    continue;
                }

                yield return instruction;
            }
        }

        public static void Postfix(Dialog_ManagePolicies<Policy> __instance, Rect leftRect)
        {
            object policy = __instance.GetType().Method("GetDefaultPolicy").Invoke(__instance, new object[] { });
            if (policy != null && __instance.GetType() != typeof(Dialog_ApparelPolicies) && __instance.GetType() != typeof(Dialog_DrugPolicies))
            {
                Rect loadDefaultRect = new Rect(leftRect.x + 10f, leftRect.yMax - 24f - 10f - Window.CloseButSize.y * 2 - 10f, leftRect.width - 20f, Window.CloseButSize.y);
                if (Widgets.ButtonText(loadDefaultRect, "Defaults_LoadDefaultPolicy".Translate()))
                {
                    if (policy is RimWorld.ApparelPolicy)
                    {
                        Find.WindowStack.Add(new FloatMenu(DefaultsSettings.DefaultApparelPolicies.Select(p => new FloatMenuOption(p.label, delegate
                        {
                            RimWorld.ApparelPolicy apparelPolicy = (RimWorld.ApparelPolicy)__instance.GetType().Method("CreateNewPolicy").Invoke(__instance, new object[] { });
                            apparelPolicy.label = p.label;
                            apparelPolicy.filter.CopyAllowancesFrom(p.filter);
                            __instance.GetType().Method("set_SelectedPolicy").Invoke(__instance, new[] { apparelPolicy });
                        })).ToList()));
                    }
                    if (policy is DrugPolicy)
                    {
                        Find.WindowStack.Add(new FloatMenu(DefaultsSettings.DefaultDrugPolicies.Select(p => new FloatMenuOption(p.label, delegate
                        {
                            DrugPolicy drugPolicy = (DrugPolicy)__instance.GetType().Method("CreateNewPolicy").Invoke(__instance, new object[] { });
                            drugPolicy.label = p.label;
                            drugPolicy.CopyFrom(p);
                            __instance.GetType().Method("set_SelectedPolicy").Invoke(__instance, new[] { drugPolicy });
                        })).ToList()));
                    }
                }
            }
        }
    }
}

using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace Defaults.ApparelPolicies
{
    [HarmonyPatch(typeof(Dialog_ManagePolicies<Policy>))]
    [HarmonyPatch(nameof(Dialog_ManagePolicies<Policy>.DoWindowContents))]
    public static class Patch_Dialog_ManagePolicies_DoWindowContents
    {
        public static void Postfix(Rect inRect, Policy ___policyInt)
        {
            if (___policyInt != null && ___policyInt is RimWorld.ApparelPolicy)
            {
                Rect saveAsDefaultRect = new Rect(inRect.xMax - 158f, inRect.y + 74f, 32f, 32f);
                if (Widgets.ButtonImage(saveAsDefaultRect, TexButton.Save))
                {
                    string name = ___policyInt.label;
                    int i = DefaultsSettings.DefaultApparelPolicies.Count + 1;
                    while (DefaultsSettings.DefaultApparelPolicies.Any(p => p.label == name))
                    {
                        name = "ApparelPolicy".Translate() + " " + i++;
                    }
                    ApparelPolicy policy = new ApparelPolicy(0, name);
                    policy.filter.CopyAllowancesFrom(((RimWorld.ApparelPolicy)___policyInt).filter);
                    DefaultsSettings.DefaultApparelPolicies.Add(policy);
                    LongEventHandler.ExecuteWhenFinished(DefaultsMod.Settings.Write);
                    Messages.Message("Defaults_PolicySavedAs".Translate(policy.label), MessageTypeDefOf.PositiveEvent, false);
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
            if (__instance.GetType().Method("GetDefaultPolicy").Invoke(__instance, new object[] { }) is RimWorld.ApparelPolicy)
            {
                Rect loadDefaultRect = new Rect(leftRect.x + 10f, leftRect.yMax - 24f - 10f - Window.CloseButSize.y * 2 - 10f, leftRect.width - 20f, Window.CloseButSize.y);
                if (Widgets.ButtonText(loadDefaultRect, "Defaults_LoadDefaultPolicy".Translate()))
                {
                    Find.WindowStack.Add(new FloatMenu(DefaultsSettings.DefaultApparelPolicies.Select(p => new FloatMenuOption(p.label, delegate
                    {
                        RimWorld.ApparelPolicy policy = (RimWorld.ApparelPolicy)__instance.GetType().Method("CreateNewPolicy").Invoke(__instance, new object[] { });
                        policy.label = p.label;
                        policy.filter.CopyAllowancesFrom(p.filter);
                        __instance.GetType().Method("set_SelectedPolicy").Invoke(__instance, new[] { policy });
                    })).ToList()));
                }
            }
        }
    }
}

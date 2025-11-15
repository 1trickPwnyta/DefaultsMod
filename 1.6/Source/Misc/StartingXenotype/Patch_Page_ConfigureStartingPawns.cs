using HarmonyLib;
using RimWorld;
using System;
using System.Linq;
using Verse;

namespace Defaults.Misc.StartingXenotype
{
    [HarmonyPatchCategory("Misc")]
    [HarmonyPatch(typeof(Page_ConfigureStartingPawns))]
    [HarmonyPatch(nameof(Page_ConfigureStartingPawns.PostOpen))]
    [HarmonyPatchMod("Ludeon.RimWorld.Biotech")]
    public static class Patch_Page_ConfigureStartingPawns
    {
        public static void Postfix()
        {
            StartingXenotypeOptions options = Settings.Get<StartingXenotypeOptions>(Settings.STARTING_XENOTYPE_OPTIONS);
            for (int i = 0; i < Find.GameInitData.startingAndOptionalPawns.Count; i++)
            {
                object[] args;
                switch (options.Option)
                {
                    case StartingXenotypeOption.AnyNonArchite:
                        args = new object[]
                        {
                            i,
                            null,
                            null,
                            DefDatabase<XenotypeDef>.AllDefsListForReading.Where(x => !x.Archite && x != XenotypeDefOf.Baseliner).ToList(),
                            0.5f,
                            (Func<PawnGenerationRequest, bool>)(r => r.ForcedXenotype != null || r.ForcedCustomXenotype != null),
                            null,
                            false
                        };
                        break;
                    case StartingXenotypeOption.XenotypeDef:
                        args = new object[]
                        {
                            i,
                            options.XenotypeDef,
                            null,
                            null,
                            0f,
                            (Func<PawnGenerationRequest, bool>)((PawnGenerationRequest r) => r.ForcedXenotype != options.XenotypeDef),
                            null,
                            false
                        };
                        break;
                    case StartingXenotypeOption.CustomXenotype:
                        args = new object[]
                        {
                            i,
                            null,
                            options.CustomXenotype,
                            null,
                            0f,
                            (Func<PawnGenerationRequest, bool>)(r => r.ForcedCustomXenotype != options.CustomXenotype),
                            null,
                            false
                        };
                        break;
                    default:
                        throw new Exception("Invalid starting xenotype option: " + options.Option);
                }
                typeof(CharacterCardUtility).Method("SetupGenerationRequest").Invoke(null, args);
                StartingPawnUtility.RandomizePawn(i);
            }
        }
    }
}

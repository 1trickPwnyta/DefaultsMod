using HarmonyLib;
using RimWorld;
using Verse;

namespace Defaults.BabyFeeding
{
    [HarmonyPatch(typeof(PregnancyUtility))]
    [HarmonyPatch(nameof(PregnancyUtility.ApplyBirthOutcome_NewTemp))]
    public static class Patch_PregnancyUtility
    {
        public static void Postfix(Thing birtherThing, Thing __result)
        {
            if (__result is Pawn baby)
            {
                foreach (Pawn feeder in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction)
                {
                    if (feeder != baby && feeder.RaceProps.Humanlike && !ChildcareUtility.CanSuckle(feeder, out _) && !feeder.IsWorkTypeDisabledByAge(WorkTypeDefOf.Childcare, out _))
                    {
                        AutofeedMode mode;
                        if (feeder == birtherThing)
                        {
                            mode = DefaultsSettings.DefaultBabyFeedingOptions.BirtherParent;
                        }
                        else
                        {
                            bool isParent = baby.GetMother() == feeder || baby.GetFather() == feeder;
                            bool isLactating = feeder.health.hediffSet.HasHediff(HediffDefOf.Lactating);
                            mode = PatchUtility_PregnancyUtility.GetMode(isParent, isLactating);
                        } 
                        if (mode != AutofeedMode.Childcare)
                        {
                            baby.mindState.SetAutofeeder(feeder, mode);
                        }
                    }
                }
            }
        }
    }

    public static class PatchUtility_PregnancyUtility
    {
        public static AutofeedMode GetMode(bool isParent, bool isLactating)
        {
            if (isParent)
            {
                if (isLactating)
                {
                    return DefaultsSettings.DefaultBabyFeedingOptions.ParentLactating;
                }
                else
                {
                    return DefaultsSettings.DefaultBabyFeedingOptions.ParentNonlactating;
                }
            }
            else
            {
                if (isLactating)
                {
                    return DefaultsSettings.DefaultBabyFeedingOptions.NonparentLactating;
                }
                else
                {
                    return DefaultsSettings.DefaultBabyFeedingOptions.NonparentNonlactating;
                }
            }
        }
    }
}

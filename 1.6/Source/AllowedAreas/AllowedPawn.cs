using RimWorld;
using System;
using Verse;

namespace Defaults.AllowedAreas
{
    public enum AllowedPawn
    {
        AdultColonist,
        ChildColonist,
        Slave,
        Guest,
        Mech,
        Animal,
        Ghoul
    }

    public static class AllowedPawnUtility
    {
        public static string GetLabel(this AllowedPawn allowedPawn)
        {
            switch (allowedPawn)
            {
                case AllowedPawn.AdultColonist: return "Defaults_AdultColonists".Translate();
                case AllowedPawn.ChildColonist: return "Defaults_ChildColonists".Translate();
                case AllowedPawn.Slave: return "Defaults_Slaves".Translate();
                case AllowedPawn.Guest: return "Defaults_Guests".Translate();
                case AllowedPawn.Mech: return "Defaults_Mechs".Translate();
                case AllowedPawn.Animal: return "Defaults_Animals".Translate();
                case AllowedPawn.Ghoul: return "Defaults_Ghouls".Translate();
                default: throw new ArgumentException("Unknown pawn type: " + allowedPawn);
            }
        }

        public static bool IsActive(this AllowedPawn allowedPawn)
        {
            switch (allowedPawn)
            {
                case AllowedPawn.ChildColonist: return ModsConfig.BiotechActive;
                case AllowedPawn.Slave: return ModsConfig.IdeologyActive;
                case AllowedPawn.Mech: return ModsConfig.BiotechActive;
                case AllowedPawn.Ghoul: return ModsConfig.AnomalyActive;
                default: return true;
            }
        }

        public static AllowedPawn? GetAllowedPawnType(Pawn pawn)
        {
            if (pawn.IsFreeNonSlaveColonist && !pawn.IsQuestLodger() && pawn.ageTracker.Adult)
            {
                return AllowedPawn.AdultColonist;
            }
            else if (pawn.IsFreeNonSlaveColonist && !pawn.IsQuestLodger() && !pawn.ageTracker.Adult)
            {
                return AllowedPawn.ChildColonist;
            }
            else if (pawn.IsSlaveOfColony)
            {
                return AllowedPawn.Slave;
            }
            else if (pawn.IsQuestLodger())
            {
                return AllowedPawn.Guest;
            }
            else if (pawn.IsColonyMech)
            {
                return AllowedPawn.Mech;
            }
            else if (pawn.IsAnimal && ((pawn.Faction != null && pawn.Faction.IsPlayer) || (pawn.HostFaction != null && pawn.HostFaction.IsPlayer)))
            {
                return AllowedPawn.Animal;
            }
            else if (pawn.IsColonySubhuman)
            {
                return AllowedPawn.Ghoul;
            }
            return null;
        }
    }
}

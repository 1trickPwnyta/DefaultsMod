using RimWorld;
using System.Collections.Generic;

namespace Defaults.WorldSettings
{
    public static class FactionsUtility
    {
        public static List<FactionDef> GetDefaultSelectableFactions()
        {
            List<FactionDef> factions = new List<FactionDef>();
            foreach (FactionDef factionDef in FactionGenerator.ConfigurableFactions)
            {
                if (factionDef.displayInFactionSelection && factionDef.startingCountAtWorldCreation > 0)
                {
                    for (int i = 0; i < factionDef.startingCountAtWorldCreation; i++)
                    {
                        factions.Add(factionDef);
                    }
                }
            }
            using (IEnumerator<FactionDef> enumerator = FactionGenerator.ConfigurableFactions.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    FactionDef faction = enumerator.Current;
                    if (faction.replacesFaction != null)
                    {
                        factions.RemoveAll(f => f == faction.replacesFaction);
                    }
                }
            }
            return factions;
        }
    }
}

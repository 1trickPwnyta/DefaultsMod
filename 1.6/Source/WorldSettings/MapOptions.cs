using RimWorld;
using Verse;

namespace Defaults.WorldSettings
{
    public class MapOptions : IExposable
    {
        public int DefaultMapSize = 250;
        public Season DefaultStartingSeason = Season.Undefined;

        public void ExposeData()
        {
            Scribe_Values.Look(ref DefaultMapSize, "DefaultMapSize", 250);
            Scribe_Values.Look(ref DefaultStartingSeason, "DefaultStartingSeason", Season.Undefined);
        }
    }
}

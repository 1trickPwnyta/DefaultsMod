using Verse;

namespace Defaults.StockpileZones
{
    public class DefaultSettingsCategoryWorker_Storage : DefaultSettingsCategoryWorker
    {
        public DefaultSettingsCategoryWorker_Storage(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_StockpileZones(def));
        }
    }
}

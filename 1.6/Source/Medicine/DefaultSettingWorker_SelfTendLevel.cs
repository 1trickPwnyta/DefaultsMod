using Defaults.Defs;
using Defaults.Workers;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.Medicine
{
    public class DefaultSettingWorker_SelfTendLevel : DefaultSettingWorker_Dropdown<int?>
    {
        public DefaultSettingWorker_SelfTendLevel(DefaultSettingDef def) : base(def)
        {
        }

        public override string Key => Settings.SELF_TEND_LEVEL;

        protected override int? Default => null;

        protected override IEnumerable<int?> Options => new List<int?>() { null }.Concat(Enumerable.Range(0, 21).Cast<int?>());

        protected override TaggedString GetText(int? option) => option?.ToString() ?? "Never".Translate();

        protected override float Width => 60f;

        protected override void ExposeSetting()
        {
            Scribe_Values.Look(ref setting, Key, Default);
        }
    }
}

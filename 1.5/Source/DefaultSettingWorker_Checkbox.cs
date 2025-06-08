using UnityEngine;
using Verse;

namespace Defaults
{
    public abstract class DefaultSettingWorker_Checkbox : DefaultSettingWorker
    {
        protected DefaultSettingWorker_Checkbox(DefaultSettingDef def) : base(def)
        {
        }

        public abstract bool Enabled { get; set; }

        public override void DoSetting(Rect rect)
        {
            rect.x += rect.width - 24f;
            rect.width = 24f;
            bool enabled = Enabled;
            Widgets.Checkbox(rect.xMax - 24f, rect.y + (rect.height - 24f) / 2, ref enabled);
            Enabled = enabled;
        }
    }
}

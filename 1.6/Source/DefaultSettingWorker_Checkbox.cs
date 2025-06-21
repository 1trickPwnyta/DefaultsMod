using UnityEngine;
using Verse;

namespace Defaults
{
    public abstract class DefaultSettingWorker_Checkbox : DefaultSettingWorker<bool?>
    {
        protected DefaultSettingWorker_Checkbox(DefaultSettingDef def) : base(def)
        {
        }

        protected override void DoWidget(Rect rect)
        {
            bool enabled = setting.Value;
            Widgets.ToggleInvisibleDraggable(rect, ref enabled, true);
            setting = enabled;
            rect.x += rect.width - 24f;
            rect.width = 24f;
            Widgets.CheckboxDraw(rect.xMax - 24f, rect.y + (rect.height - 24f) / 2, setting.Value, false);
        }

        protected override void ExposeSetting()
        {
            Scribe_Values.Look(ref setting, Key);
        }
    }
}

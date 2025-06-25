using Defaults.UI;
using RimWorld;
using UnityEngine;
using Verse;

namespace Defaults.Workers
{
    public abstract class DefaultSettingWorker_Checkbox : DefaultSettingWorker<bool?>
    {
        protected DefaultSettingWorker_Checkbox(DefaultSettingDef def) : base(def)
        {
        }

        protected virtual Texture2D Icon => null;

        protected virtual TaggedString Tip => null;

        protected override void DoWidget(Rect rect)
        {
            Rect checkRect = rect;
            checkRect.x += checkRect.width - checkRect.height;
            checkRect.width = checkRect.height;
            bool enabled = setting.Value;
            if (Icon != null)
            {
                UIUtility.DrawCheckButton(checkRect.ContractedBy(3f), Icon, Tip, ref enabled);
            }
            else
            {
                Widgets.ToggleInvisibleDraggable(rect, ref enabled, true, true);
                Widgets.CheckboxDraw(checkRect.x + 3f, checkRect.y + 3f, setting.Value, false);
            }
            setting = enabled;
        }

        protected override void ExposeSetting()
        {
            Scribe_Values.Look(ref setting, Key);
        }
    }
}

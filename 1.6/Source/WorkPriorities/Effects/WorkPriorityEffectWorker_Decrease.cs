using Defaults.Defs;
using Defaults.UI;
using Defaults.Workers;
using RimWorld;
using UnityEngine;
using Verse;

namespace Defaults.WorkPriorities.Effects
{
    [StaticConstructorOnStartup]
    public class WorkPriorityEffectWorker_Decrease : WorkPriorityEffectWorker<Effect_Decrease>
    {
        private static readonly Texture2D allowZeroTex = WidgetsWork.WorkBoxBGTex_Mid;

        private string editBuffer;

        public WorkPriorityEffectWorker_Decrease(WorkPriorityEffectDef def) : base(def)
        {
        }

        protected override void DoUI(Rect rect, Effect_Decrease effect)
        {
            UIUtility.IntEntry(rect.LeftPartPixels(rect.width - rect.height), ref effect.amount, ref editBuffer, minimum: WorkPriorityValue.Max, maximum: WorkPriorityValue.Max);
            UIUtility.DoCheckButton(rect.RightPartPixels(rect.height), allowZeroTex, "Defaults_WorkPriorityAllowZero".Translate(WorkPriorityValue.Max), ref effect.allowZero);
        }
    }
}

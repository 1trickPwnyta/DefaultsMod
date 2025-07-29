using Defaults.Defs;
using Defaults.UI;
using Defaults.Workers;
using UnityEngine;

namespace Defaults.WorkPriorities.Effects
{
    public class WorkPriorityEffectWorker_Increase : WorkPriorityEffectWorker<Effect_Increase>
    {
        private string editBuffer;

        public WorkPriorityEffectWorker_Increase(WorkPriorityEffectDef def) : base(def)
        {
        }

        protected override void DoUI(Rect rect, Effect_Increase effect)
        {
            UIUtility.IntEntry(rect, ref effect.amount, ref editBuffer, minimum: WorkPriorityValue.Max, maximum: WorkPriorityValue.Max);
        }
    }
}

using Defaults.Defs;
using Defaults.Workers;
using UnityEngine;

namespace Defaults.WorkPriorities.Effects
{
    public class WorkPriorityEffectWorker_Logic : WorkPriorityEffectWorker<Effect_Logic>
    {
        public WorkPriorityEffectWorker_Logic(WorkPriorityEffectDef def) : base(def)
        {
        }

        protected override void DoUI(Rect rect, Effect_Logic effect)
        {
            
        }
    }
}

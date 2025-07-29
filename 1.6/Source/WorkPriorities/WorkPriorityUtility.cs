﻿using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.WorkPriorities
{
    public static class WorkPriorityUtility
    {
        public static void SetWorkPrioritiesToDefault(Pawn pawn)
        {
            if (Settings.Get<bool>(Settings.WORK_PRIORITIES_ADVANCED_MODE))
            {
                List<Rule> advancedGlobalWorkPriorityLogic = Settings.Get<List<Rule>>(Settings.WORK_PRIORITIES_GLOBAL_LOGIC);
                foreach (WorkTypeDef def in DefDatabase<WorkTypeDef>.AllDefsListForReading.Where(d => !pawn.WorkTypeIsDisabled(d)))
                {
                    int priority = pawn.workSettings.GetPriority(def);
                    bool? applied = ApplyRules(advancedGlobalWorkPriorityLogic, def, pawn);
                    // TODO do specific logic
                    if (applied.HasValue && !applied.Value)
                    {
                        pawn.workSettings.SetPriority(def, priority);
                    }
                }
            }
            else
            {
                Dictionary<WorkTypeDef, int> basicDefaultWorkPriorities = Settings.Get<Dictionary<WorkTypeDef, int>>(Settings.WORK_PRIORITIES_BASIC);
                foreach (WorkTypeDef def in DefDatabase<WorkTypeDef>.AllDefsListForReading.Where(d => !pawn.WorkTypeIsDisabled(d) && basicDefaultWorkPriorities.ContainsKey(d)))
                {
                    int priority = basicDefaultWorkPriorities[def];
                    if (priority == WorkPriorityValue.DoNotDo)
                    {
                        pawn.workSettings.Disable(def);
                    }
                    else if (priority > 0)
                    {
                        pawn.workSettings.SetPriority(def, priority);
                    }
                }
            }
        }

        public static bool? ApplyRules(List<Rule> rules, WorkTypeDef def, Pawn pawn)
        {
            bool? applied = null;
            foreach (Rule rule in rules)
            {
                bool? appliedRule = rule.Apply(def, pawn);
                if (appliedRule.HasValue)
                {
                    applied = appliedRule.Value;
                }
            }
            return applied;
        }
    }
}

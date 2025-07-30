using Defaults.Defs;
using Verse;

namespace Defaults.WorkPriorities.Effects
{
    public class Effect_Value : Effect
    {
        public int value = 3;

        public Effect_Value()
        {
        }

        public Effect_Value(WorkPriorityEffectDef def) : base(def)
        {
        }

        public override bool? Apply(WorkTypeDef def, Pawn pawn)
        {
            switch (value)
            {
                case WorkPriorityValue.TynansChoice:
                    return false;
                case WorkPriorityValue.DoNotDo:
                    pawn.workSettings.Disable(def);
                    return true;
                default:
                    pawn.workSettings.SetPriority(def, value);
                    return true;
            }
        }

        public override Effect MakeCopy() => new Effect_Value(def)
        {
            value = value
        };

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref value, "value", 3);
        }
    }
}

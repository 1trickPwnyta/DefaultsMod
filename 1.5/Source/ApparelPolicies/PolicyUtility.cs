using RimWorld;
using Verse;

namespace Defaults.ApparelPolicies
{
    public static class PolicyUtility
    {
        public static float GetNewPolicyButtonPaddingTop(Policy policy)
        {
            if (policy != null && policy is RimWorld.ApparelPolicy)
            {
                return 10f + Window.CloseButSize.y;
            }
            else
            {
                return 10f;
            }
        }
    }
}

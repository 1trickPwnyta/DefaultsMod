using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.Policies.ReadingPolicies
{
    public class Dialog_ReadingPolicies : Dialog_ManageReadingPolicies
    {
        public Dialog_ReadingPolicies(ReadingPolicy policy) : base(policy)
        {
            optionalTitle = TitleKey.Translate();
        }

        protected override ReadingPolicy CreateNewPolicy() => PolicyUtility.NewDefaultPolicy<ReadingPolicy>();

        protected override ReadingPolicy GetDefaultPolicy() => DefaultsSettings.DefaultReadingPolicies.First();

        protected override List<ReadingPolicy> GetPolicies() => DefaultsSettings.DefaultReadingPolicies;

        protected override void SetDefaultPolicy(ReadingPolicy policy)
        {
            List<ReadingPolicy> policies = DefaultsSettings.DefaultReadingPolicies;
            int currentIndex = policies.IndexOf(policy);
            policies[currentIndex] = policies[0];
            policies[0] = policy;
        }

        protected override AcceptanceReport TryDeletePolicy(ReadingPolicy policy)
        {
            return policy == GetDefaultPolicy()
                ? (AcceptanceReport)"Defaults_CantDeleteDefaultPolicy".Translate()
                : (AcceptanceReport)DefaultsSettings.DefaultReadingPolicies.Remove(policy);
        }
    }
}

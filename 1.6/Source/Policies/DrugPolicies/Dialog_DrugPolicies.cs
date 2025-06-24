using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.Policies.DrugPolicies
{
    public class Dialog_DrugPolicies : Dialog_ManageDrugPolicies
    {
        public Dialog_DrugPolicies(DrugPolicy policy) : base(policy)
        {
            optionalTitle = TitleKey.Translate();
        }

        protected override DrugPolicy CreateNewPolicy() => PolicyUtility.NewDefaultPolicy<DrugPolicy>();

        protected override DrugPolicy GetDefaultPolicy() => DefaultsSettings.DefaultDrugPolicies.First();

        protected override List<DrugPolicy> GetPolicies() => DefaultsSettings.DefaultDrugPolicies;

        protected override void SetDefaultPolicy(DrugPolicy policy)
        {
            List<DrugPolicy> policies = DefaultsSettings.DefaultDrugPolicies;
            int currentIndex = policies.IndexOf(policy);
            policies[currentIndex] = policies[0];
            policies[0] = policy;
        }

        protected override AcceptanceReport TryDeletePolicy(DrugPolicy policy)
        {
            return policy == GetDefaultPolicy()
                ? (AcceptanceReport)"Defaults_CantDeleteDefaultPolicy".Translate()
                : (AcceptanceReport)DefaultsSettings.DefaultDrugPolicies.Remove(policy);
        }
    }
}

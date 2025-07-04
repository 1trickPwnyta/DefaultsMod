﻿using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.Policies.DrugPolicies
{
    public class Dialog_DrugPolicies : Dialog_ManageDrugPolicies
    {
        public Dialog_DrugPolicies(DrugPolicy policy) : base(policy)
        {
        }

        protected override DrugPolicy CreateNewPolicy()
        {
            return PolicyUtility.NewDrugPolicy();
        }

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
            if (policy == GetDefaultPolicy())
            {
                return "Defaults_CantDeleteDefaultPolicy".Translate();
            }
            return DefaultsSettings.DefaultDrugPolicies.Remove(policy);
        }
    }
}

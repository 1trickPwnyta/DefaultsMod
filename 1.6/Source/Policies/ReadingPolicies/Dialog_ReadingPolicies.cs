﻿using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.Policies.ReadingPolicies
{
    public class Dialog_ReadingPolicies : Dialog_ManageReadingPolicies
    {
        public Dialog_ReadingPolicies(ReadingPolicy policy) : base(policy)
        {
        }

        protected override RimWorld.ReadingPolicy CreateNewPolicy()
        {
            return PolicyUtility.NewReadingPolicy();
        }

        protected override RimWorld.ReadingPolicy GetDefaultPolicy() => DefaultsSettings.DefaultReadingPolicies.First();

        protected override List<RimWorld.ReadingPolicy> GetPolicies() => DefaultsSettings.DefaultReadingPolicies.Select(p => (RimWorld.ReadingPolicy)p).ToList();

        protected override void SetDefaultPolicy(RimWorld.ReadingPolicy policy)
        {
            List<ReadingPolicy> policies = DefaultsSettings.DefaultReadingPolicies;
            int currentIndex = policies.IndexOf((ReadingPolicy)policy);
            policies[currentIndex] = policies[0];
            policies[0] = (ReadingPolicy)policy;
        }

        protected override AcceptanceReport TryDeletePolicy(RimWorld.ReadingPolicy policy)
        {
            if (policy == GetDefaultPolicy())
            {
                return "Defaults_CantDeleteDefaultPolicy".Translate();
            }
            return DefaultsSettings.DefaultReadingPolicies.Remove((ReadingPolicy)policy);
        }
    }
}

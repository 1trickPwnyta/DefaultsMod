using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults.Policies.ApparelPolicies
{
    public class Dialog_ApparelPolicies : Dialog_ManagePolicies<ApparelPolicy>
    {
        private static readonly ThingFilter apparelGlobalFilter = new ThingFilter();

        private readonly ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

        static Dialog_ApparelPolicies()
        {
            apparelGlobalFilter.SetAllow(ThingCategoryDefOf.Apparel, true);
        }

        public Dialog_ApparelPolicies(ApparelPolicy policy) : base(policy)
        {
            optionalTitle = "ApparelPolicyTitle".Translate();
        }

        public override Vector2 InitialSize => new Vector2(700f, 700f);

        protected override string TitleKey => "ApparelPolicyTitle";

        protected override string TipKey => "ApparelPolicyTip";

        protected override ApparelPolicy CreateNewPolicy() => PolicyUtility.NewDefaultPolicy<ApparelPolicy>();

        protected override void DoContentsRect(Rect rect)
        {
            ThingFilterUI.DoThingFilterConfigWindow(rect, thingFilterState, SelectedPolicy.filter, apparelGlobalFilter, 16, null, HiddenSpecialThingFilters());
        }

        protected override ApparelPolicy GetDefaultPolicy() => DefaultsSettings.DefaultApparelPolicies.First();

        protected override List<ApparelPolicy> GetPolicies() => DefaultsSettings.DefaultApparelPolicies;

        protected override void SetDefaultPolicy(ApparelPolicy policy)
        {
            List<ApparelPolicy> policies = DefaultsSettings.DefaultApparelPolicies;
            int currentIndex = policies.IndexOf(policy);
            policies[currentIndex] = policies[0];
            policies[0] = policy;
        }

        protected override AcceptanceReport TryDeletePolicy(ApparelPolicy policy)
        {
            return policy == GetDefaultPolicy()
                ? (AcceptanceReport)"Defaults_CantDeleteDefaultPolicy".Translate()
                : (AcceptanceReport)DefaultsSettings.DefaultApparelPolicies.Remove(policy);
        }

        private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
        {
            yield return SpecialThingFilterDefOf.AllowNonDeadmansApparel;
            if (ModsConfig.IdeologyActive)
            {
                yield return SpecialThingFilterDefOf.AllowVegetarian;
                yield return SpecialThingFilterDefOf.AllowCarnivore;
                yield return SpecialThingFilterDefOf.AllowCannibal;
                yield return SpecialThingFilterDefOf.AllowInsectMeat;
            }
        }
    }
}

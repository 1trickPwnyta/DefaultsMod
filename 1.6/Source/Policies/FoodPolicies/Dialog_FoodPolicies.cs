using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults.Policies.FoodPolicies
{
    public class Dialog_FoodPolicies : Dialog_ManagePolicies<FoodPolicy>
    {
        private static readonly ThingFilter foodGlobalFilter = new ThingFilter();

        private readonly ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

        static Dialog_FoodPolicies()
        {
            foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.Where(x => x.GetStatValueAbstract(StatDefOf.Nutrition, null) > 0f))
            {
                foodGlobalFilter.SetAllow(thingDef, true);
            }
        }

        public Dialog_FoodPolicies(FoodPolicy policy) : base(policy)
        {
            optionalTitle = "FoodPolicyTitle".Translate();
        }

        public override Vector2 InitialSize => new Vector2(700f, 700f);

        protected override string TitleKey => "FoodPolicyTitle";

        protected override string TipKey => "FoodPolicyTip";

        protected override FoodPolicy CreateNewPolicy() => PolicyUtility.NewDefaultPolicy<FoodPolicy>();

        protected override void DoContentsRect(Rect rect)
        {
            ThingFilterUI.DoThingFilterConfigWindow(rect, thingFilterState, SelectedPolicy.filter, foodGlobalFilter, forceHiddenFilters: HiddenSpecialThingFilters(), forceHideHitPointsConfig: true);
        }

        protected override FoodPolicy GetDefaultPolicy() => DefaultsSettings.DefaultFoodPolicies.First();

        protected override List<FoodPolicy> GetPolicies() => DefaultsSettings.DefaultFoodPolicies;

        protected override void SetDefaultPolicy(FoodPolicy policy)
        {
            List<FoodPolicy> policies = DefaultsSettings.DefaultFoodPolicies;
            int currentIndex = policies.IndexOf(policy);
            policies[currentIndex] = policies[0];
            policies[0] = policy;
        }

        protected override AcceptanceReport TryDeletePolicy(FoodPolicy policy)
        {
            return policy == GetDefaultPolicy()
                ? (AcceptanceReport)"Defaults_CantDeleteDefaultPolicy".Translate()
                : (AcceptanceReport)DefaultsSettings.DefaultFoodPolicies.Remove(policy);
        }

        private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
        {
            yield return SpecialThingFilterDefOf.AllowFresh;
        }
    }
}

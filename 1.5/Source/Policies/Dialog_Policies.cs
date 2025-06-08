using Defaults.Policies.ApparelPolicies;
using Defaults.Policies.DrugPolicies;
using Defaults.Policies.FoodPolicies;
using Defaults.Policies.ReadingPolicies;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Defaults.Policies
{
    public class Dialog_Policies : Window
    {
        private static Window currentWindow;

        private readonly List<PolicyTab> tabs = new List<PolicyTab>();

        public Dialog_Policies()
        {
            doCloseX = true;
            doCloseButton = true;
            optionalTitle = "Defaults_Policies".Translate();

            tabs.AddRange(new[]
            {
                new PolicyTab(new Dialog_ApparelPolicies(DefaultsSettings.DefaultApparelPolicies[0])),
                new PolicyTab(new Dialog_FoodPolicies(DefaultsSettings.DefaultFoodPolicies[0])),
                new PolicyTab(new Dialog_DrugPolicies(DefaultsSettings.DefaultDrugPolicies[0])),
                new PolicyTab(new Dialog_ReadingPolicies(DefaultsSettings.DefaultReadingPolicies[0]))
            });
            currentWindow = tabs[0].window;
        }

        public override Vector2 InitialSize => new Vector2(1320f, 807f);

        public override void DoWindowContents(Rect inRect)
        {
            Rect tabsRect = new Rect(inRect.x, inRect.y + 32f, inRect.width, 1f);
            TabDrawer.DrawTabs(tabsRect, tabs);
            Rect contentRect = new Rect(inRect.x, tabsRect.yMax, currentWindow.InitialSize.x - 36f, inRect.height - CloseButSize.y);
            currentWindow.DoWindowContents(contentRect);
        }

        private class PolicyTab : TabRecord
        {
            public Window window;

            public PolicyTab(Window window) : base(window.optionalTitle, () => currentWindow = window, () => currentWindow == window)
            {
                this.window = window;
            }
        }
    }
}

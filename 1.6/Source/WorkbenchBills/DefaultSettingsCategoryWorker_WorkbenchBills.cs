using Defaults.Defs;
using Defaults.Workers;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.WorkbenchBills
{
    public class DefaultSettingsCategoryWorker_WorkbenchBills : DefaultSettingsCategoryWorker
    {
        private List<WorkbenchBillStore> defaultWorkbenchBills;
        private GlobalBillOptions defaultGlobalBillOptions;

        public DefaultSettingsCategoryWorker_WorkbenchBills(DefaultSettingsCategoryDef def) : base(def)
        {
        }

        public override void OpenSettings()
        {
            Find.WindowStack.Add(new Dialog_WorkbenchBills(def));
        }

        protected override bool GetCategorySetting(string key, out object value)
        {
            switch (key)
            {
                case Settings.WORKBENCH_BILLS:
                    value = defaultWorkbenchBills;
                    return true;
                case Settings.GLOBAL_BILL_OPTIONS:
                    value = defaultGlobalBillOptions;
                    return true;
                default:
                    return base.GetCategorySetting(key, out value);
            }
        }

        protected override bool SetCategorySetting(string key, object value)
        {
            switch (key)
            {
                case Settings.WORKBENCH_BILLS:
                    defaultWorkbenchBills = value as List<WorkbenchBillStore>;
                    return true;
                case Settings.GLOBAL_BILL_OPTIONS:
                    defaultGlobalBillOptions = value as GlobalBillOptions;
                    return true;
                default:
                    return base.SetCategorySetting(key, value);
            }
        }

        public override void HandleNewDefs(IEnumerable<Def> defs)
        {
            foreach (BillTemplate bill in defaultWorkbenchBills.SelectMany(s => s.bills))
            {
                if (!bill.locked)
                {
                    foreach (ThingDef def in defs.OfType<ThingDef>())
                    {
                        bill.ingredientFilter.SetAllow(def, true);
                    }
                    foreach (SpecialThingFilterDef def in defs.OfType<SpecialThingFilterDef>())
                    {
                        bill.ingredientFilter.SetAllow(def, false);
                    }
                }
            }
        }

        protected override void ResetCategorySettings(bool forced)
        {
            if (forced || defaultWorkbenchBills == null)
            {
                defaultWorkbenchBills = new List<WorkbenchBillStore>();
            }
            if (forced || defaultGlobalBillOptions == null)
            {
                defaultGlobalBillOptions = new GlobalBillOptions();
            }
        }

        protected override void ExposeCategorySettings()
        {
            Scribe_Collections.Look(ref defaultWorkbenchBills, Settings.WORKBENCH_BILLS);
            Scribe_Deep.Look(ref defaultGlobalBillOptions, Settings.GLOBAL_BILL_OPTIONS);
            BackwardCompatibilityUtility.MigrateGlobalBillOptions(ref defaultGlobalBillOptions);
        }
    }
}

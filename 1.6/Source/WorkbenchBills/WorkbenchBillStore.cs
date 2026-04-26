using Defaults.Defs;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.WorkbenchBills
{
    public class WorkbenchBillStore : IExposable
    {
        public HashSet<ThingDef> workbenchGroup;
        public List<BillTemplate> bills = new List<BillTemplate>();

        public static bool StoreExists(HashSet<ThingDef> workbenchGroup)
        {
            List<WorkbenchBillStore> workbenchBills = Settings.Get<List<WorkbenchBillStore>>(Settings.WORKBENCH_BILLS);
            return workbenchBills.Any(s => s.workbenchGroup.Any(w => workbenchGroup.Contains(w)));
        }

        public static WorkbenchBillStore GetCreateIfNotExist(HashSet<ThingDef> workbenchGroup)
        {
            List<WorkbenchBillStore> workbenchBills = Settings.Get<List<WorkbenchBillStore>>(Settings.WORKBENCH_BILLS);
            WorkbenchBillStore store = workbenchBills.FirstOrDefault(s => s.workbenchGroup.Any(w => workbenchGroup.Contains(w)));
            if (store == null)
            {
                store = new WorkbenchBillStore(workbenchGroup);
                workbenchBills.Add(store);
            }
            store.workbenchGroup = workbenchGroup;
            return store;
        }

        public static void Delete(HashSet<ThingDef> workbenchGroup)
        {
            List<WorkbenchBillStore> workbenchBills = Settings.Get<List<WorkbenchBillStore>>(Settings.WORKBENCH_BILLS);
            workbenchBills.RemoveWhere(s => s.workbenchGroup.Any(w => workbenchGroup.Contains(w)));
        }

        private WorkbenchBillStore() { }

        public WorkbenchBillStore(HashSet<ThingDef> workbenchGroup)
        {
            this.workbenchGroup = workbenchGroup;
        }

        public void ExposeData()
        {
            Scribe_Collections_Silent.Look(ref workbenchGroup, "workbenchGroup");
            workbenchGroup.RemoveWhere(t => t == null);
            Scribe_Collections.Look(ref bills, "bills");
            bills.RemoveWhere(b => b.recipe == null);
        }
    }
}

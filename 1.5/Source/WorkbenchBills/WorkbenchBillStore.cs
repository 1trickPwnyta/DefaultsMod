using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Defaults.WorkbenchBills
{
    public class WorkbenchBillStore : IExposable
    {
        public HashSet<ThingDef> workbenchGroup;
        public List<BillTemplate> bills = new List<BillTemplate>();

        public static WorkbenchBillStore Get(HashSet<ThingDef> workbenchGroup)
        {
            WorkbenchBillStore store = DefaultsSettings.DefaultWorkbenchBills.FirstOrDefault(s => s.workbenchGroup.SetEquals(workbenchGroup));
            if (store == null)
            {
                store = new WorkbenchBillStore(workbenchGroup);
                DefaultsSettings.DefaultWorkbenchBills.Add(store);
            }
            return store;
        }

        private WorkbenchBillStore() { }

        public WorkbenchBillStore(HashSet<ThingDef> workbenchGroup)
        {
            this.workbenchGroup = workbenchGroup;
        }

        public void ExposeData()
        {
            Scribe_Collections.Look(ref workbenchGroup, "workbenchGroup");
            Scribe_Collections.Look(ref bills, "bills");
        }
    }
}

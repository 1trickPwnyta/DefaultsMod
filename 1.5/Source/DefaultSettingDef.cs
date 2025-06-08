using System;
using Verse;

namespace Defaults
{
    public class DefaultSettingDef : Def
    {
        private DefaultSettingWorker worker;

        public DefaultSettingsCategoryDef category;
        public int uiOrder;
        public Type workerClass;

        public DefaultSettingWorker Worker
        {
            get
            {
                if (worker == null)
                {
                    worker = (DefaultSettingWorker)Activator.CreateInstance(workerClass, new[] { this });
                }
                return worker;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Verse;

namespace Defaults
{
    public class DefaultSettingDef : Def
    {
        private DefaultSettingWorker worker;

        public DefaultSettingsCategoryDef category;
        public int uiOrder;
        public Type workerClass;
        public List<string> keywords = new List<string>();

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

using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace Defaults
{
    public class DefaultSettingDef : Def
    {
        private IDefaultSettingWorker worker;

        public DefaultSettingsCategoryDef category;
        public int uiOrder;
        public Type workerClass;
        public List<string> keywords = new List<string>();

        public IDefaultSettingWorker Worker
        {
            get
            {
                if (worker == null)
                {
                    worker = (IDefaultSettingWorker)Activator.CreateInstance(workerClass, new[] { this });
                }
                return worker;
            }
        }

        public bool Matches(QuickSearchFilter filter) => filter.Matches(label) || keywords.Any(k => filter.Matches(k));
    }
}

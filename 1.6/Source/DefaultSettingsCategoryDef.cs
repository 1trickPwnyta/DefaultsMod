using Defaults.Workers;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults
{
    public class DefaultSettingsCategoryDef : Def
    {
        private Texture2D icon;
        private DefaultSettingsCategoryWorker worker;

        public string iconPath;
        public Type workerClass;
        public List<string> keywords = new List<string>();

        public Texture2D Icon
        {
            get
            {
                if (icon == null)
                {
                    if (iconPath != null)
                    {
                        icon = ContentFinder<Texture2D>.Get(iconPath);
                    }
                }
                return icon;
            }
        }

        public DefaultSettingsCategoryWorker Worker
        {
            get
            {
                if (worker == null)
                {
                    worker = (DefaultSettingsCategoryWorker)Activator.CreateInstance(workerClass, new[] { this });
                }
                return worker;
            }
        }

        public IEnumerable<DefaultSettingDef> DefaultSettings => DefDatabase<DefaultSettingDef>.AllDefsListForReading.Where(d => d.category == this);

        public bool Matches(QuickSearchFilter filter) => filter.Matches(label)
            || keywords.Any(k => filter.Matches(k))
            || DefaultSettings.Any(s => s.Matches(filter)
        );
    }
}

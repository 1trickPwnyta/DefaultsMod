using System;
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
    }
}

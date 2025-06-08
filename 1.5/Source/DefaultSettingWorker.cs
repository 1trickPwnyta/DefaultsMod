using UnityEngine;

namespace Defaults
{
    public abstract class DefaultSettingWorker
    {
        public readonly DefaultSettingDef def;

        public DefaultSettingWorker(DefaultSettingDef def)
        {
            this.def = def;
        }

        public abstract void DoSetting(Rect rect);
    }
}

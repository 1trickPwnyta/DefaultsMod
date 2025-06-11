using UnityEngine;
using Verse;

namespace Defaults
{
    public abstract class DefaultSettingWorker
    {
        public abstract string Key { get; }

        public abstract void ExposeData();

        public abstract void SetDefault();

        public abstract void DoSetting(Rect rect);
    }

    public abstract class DefaultSettingWorker<T> : DefaultSettingWorker
    {
        public readonly DefaultSettingDef def;
        public T setting;

        public DefaultSettingWorker(DefaultSettingDef def)
        {
            this.def = def;
        }

        protected abstract void DoWidget(Rect rect);

        public override void DoSetting(Rect rect)
        {
            using (new TextBlock(TextAnchor.MiddleLeft))
            {
                Widgets.Label(rect, def.LabelCap);
            }
            DoWidget(rect);
        }

        protected abstract void ExposeSetting();

        public override void ExposeData()
        {
            ExposeSetting();
            if (Scribe.mode == LoadSaveMode.PostLoadInit && setting == null)
            {
                SetDefault();
            }
        }

        protected abstract T Default { get; }

        public override void SetDefault()
        {
            setting = Default;
        }
    }
}

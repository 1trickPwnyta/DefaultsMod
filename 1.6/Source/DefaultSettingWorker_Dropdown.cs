using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Defaults
{
    public abstract class DefaultSettingWorker_Dropdown<T> : DefaultSettingWorker<T>
    {
        protected DefaultSettingWorker_Dropdown(DefaultSettingDef def) : base(def)
        {
        }

        public abstract IEnumerable<T> Options { get; }

        public virtual Texture2D GetIcon(T option) => null;

        public virtual TaggedString GetText(T option) => null;

        public virtual float Width => 0f;

        protected override void DoWidget(Rect rect)
        {
            Texture2D icon = GetIcon(setting);
            TaggedString text = GetText(setting);
            if (icon == null && text.NullOrEmpty())
            {
                throw new Exception("At least one of icon or text must be specified for option " + setting + ".");
            }
            if (!text.NullOrEmpty() && Width <= 0f)
            {
                throw new Exception("If text is specified for option " + setting + ", width must also be specified.");
            }

            float width = Width;
            if (icon != null)
            {
                width += 24f;
            }
            rect.x += rect.width - width;
            rect.width = width;
        }
    }
}

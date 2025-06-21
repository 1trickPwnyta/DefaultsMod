using UnityEngine;
using Verse;

namespace Defaults
{
    public abstract class SettingsDialog : Window
    {
        protected SettingsDialog()
        {
            doCloseX = true;
            doCloseButton = true;
            absorbInputAroundWindow = true;
            closeOnClickedOutside = true;
        }

        public virtual string Title { get; }

        public abstract void DoSettings(Rect rect);

        public override void DoWindowContents(Rect inRect)
        {
            float y = 0f;
            if (!Title.NullOrEmpty())
            {
                using (new TextBlock(GameFont.Medium))
                {
                    Widgets.Label(inRect, Title);
                    y = Text.LineHeight + Margin - 4f;
                }
            }
            DoSettings(new Rect(inRect.x, inRect.y + y, inRect.width, inRect.height - y));
        }
    }
}

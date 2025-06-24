using UnityEngine;
using Verse;

namespace Defaults
{
    public abstract class Dialog_SettingsCategory : Window
    {
        private readonly DefaultSettingsCategoryDef category;

        protected Dialog_SettingsCategory(DefaultSettingsCategoryDef category)
        {
            this.category = category;
            doCloseX = true;
            doCloseButton = true;
            absorbInputAroundWindow = true;
            closeOnClickedOutside = true;
        }

        public virtual string Title { get; }

        public Vector2 ResetButtonSize => new Vector2(250f, 30f);

        protected virtual bool DoResetButton => true;

        protected virtual Vector2 ResetButtonPosition(Rect rect) => new Vector2(rect.x + rect.width / 2 - ResetButtonSize.x / 2, rect.yMax - CloseButSize.y - 10f - ResetButtonSize.y);

        protected virtual void OnResetButtonClicked()
        {
            Find.WindowStack.Add(new Dialog_MessageBox(ResetButtonWarning, "Confirm".Translate(), category.Worker.ResetSettings, "GoBack".Translate(), null, null, true, category.Worker.ResetSettings));
        }

        protected virtual TaggedString ResetButtonWarning => "Defaults_ConfirmResetTheseSettings".Translate(category.label);

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

            if (Widgets.ButtonText(new Rect(ResetButtonPosition(inRect), ResetButtonSize), "Defaults_ResetTheseSettings".Translate()))
            {
                OnResetButtonClicked();
            }
        }
    }
}

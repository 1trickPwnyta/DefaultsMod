using Defaults.Defs;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Defaults.UI
{
    public abstract class Dialog_SettingsCategory : Dialog_Common
    {
        private readonly DefaultSettingsCategoryDef category;

        public Dialog_SettingsCategory(DefaultSettingsCategoryDef category)
        {
            this.category = category;
        }

        public virtual string Title => category.LabelCap;

        protected virtual bool DoResetOption => true;

        protected virtual string ResetOptionText => "Defaults_ResetTheseSettings".Translate();

        protected virtual void OnResetOptionClicked()
        {
            Find.WindowStack.Add(new Dialog_MessageBox(ResetOptionWarning, "Confirm".Translate(), category.Worker.ResetSettings, "GoBack".Translate(), null, null, true, category.Worker.ResetSettings));
        }

        protected virtual TaggedString ResetOptionWarning => "Defaults_ConfirmResetTheseSettings".Translate(category.label);

        protected override IEnumerable<FloatMenuOption> QuickOptions
        {
            get
            {
                foreach (FloatMenuOption option in category.Worker.FloatMenuOptions)
                {
                    yield return option;
                }

                if (DoResetOption)
                {
                    yield return new FloatMenuOption(ResetOptionText, OnResetOptionClicked);
                }
            }
        }

        public abstract void DoSettings(Rect rect);

        public override void DoWindowContents(Rect inRect)
        {
            base.DoWindowContents(inRect);
            float y = 0f;

            if (!Title.NullOrEmpty())
            {
                using (new TextBlock(GameFont.Medium))
                {
                    Widgets.Label(inRect, Title);
                    y += Text.LineHeight + Margin - 4f;
                }
            }

            if (!category.Enabled)
            {
                TaggedString message = "Defaults_SettingsCategoryDisabled".Translate(category.LabelCap);
                Rect disabledRect = new Rect(inRect.x, y, inRect.width, Mathf.Max(Text.CalcHeight(message, inRect.width - 100f) + 6f, 30f));
                Widgets.DrawRectFast(disabledRect, Color.red.WithAlpha(0.25f));
                using (new TextBlock(TextAnchor.MiddleLeft))
                {
                    Widgets.Label(disabledRect.LeftPartPixels(disabledRect.width - 100f).ContractedBy(3f), message);
                }
                if (Widgets.ButtonText(disabledRect.RightPartPixels(100f).MiddlePartPixels(100f, 30f).ContractedBy(3f), "Defaults_Enable".Translate()))
                {
                    category.Enabled = true;
                    SoundDefOf.Click.PlayOneShot(null);
                }
                y += disabledRect.height + Margin;
            }

            DoSettings(new Rect(inRect.x, inRect.y + y, inRect.width, inRect.height - CloseButSize.y - 10f - y));
        }
    }
}

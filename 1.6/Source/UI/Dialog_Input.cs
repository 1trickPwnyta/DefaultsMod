using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace Defaults.UI
{
    public class Dialog_Input<T> : Dialog_Common
    {
        private readonly string title;
        private readonly string prompt;
        private string input;
        private readonly Action<T> confirmAction;
        private readonly Func<string, T> parser;
        private readonly Func<T, AcceptanceReport> validator;

        public Dialog_Input(Action<T> confirmAction, Func<string, T> parser, string title = null, string prompt = null, T defaultValue = default, Func<T, AcceptanceReport> validator = null)
        {
            this.title = title;
            this.prompt = prompt;
            this.confirmAction = confirmAction;
            this.parser = parser;
            this.validator = validator;
            input = defaultValue.ToString();
            doCloseX = true;
            doCloseButton = false;
            forcePause = true;
            closeOnAccept = false;
            closeOnClickedOutside = true;
            absorbInputAroundWindow = true;
        }

        public override Vector2 InitialSize => new Vector2(480f, 225f);

        public override void DoWindowContents(Rect inRect)
        {
            float y = 0f;

            if (!title.NullOrEmpty())
            {
                using (new TextBlock(GameFont.Medium))
                {
                    Rect titleRect = new Rect(inRect.x, inRect.y + y, inRect.width, Text.LineHeight + 10f);
                    Widgets.Label(titleRect, title);
                    y += titleRect.height;
                }
            }

            if (!prompt.NullOrEmpty())
            {
                Rect promptRect = new Rect(inRect.x, inRect.y + y, inRect.width, Text.LineHeight + 10f);
                Widgets.Label(promptRect, prompt);
                y += promptRect.height;
            }

            GUI.SetNextControlName("InputField");
            Rect textRect = new Rect(0f, y, inRect.width, 35f);
            input = Widgets.TextField(textRect, input);
            Verse.UI.FocusControl("InputField", this);

            Rect buttonRect = new Rect(15f, inRect.height - 35f - 10f, inRect.width - 15f - 15f, 35f);
            if (Widgets.ButtonText(buttonRect, "OK"))
            {
                T result = parser(input);
                AcceptanceReport acceptanceReport = validator != null ? validator(result) : AcceptanceReport.WasAccepted;
                if (!acceptanceReport.Accepted)
                {
                    if (acceptanceReport.Reason.NullOrEmpty())
                    {
                        Messages.Message("Defaults_InputIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
                        return;
                    }
                    Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
                    return;
                }
                else
                {
                    Find.WindowStack.TryRemove(this);
                    confirmAction(result);
                }
            }
        }
    }

    public class Dialog_InputText : Dialog_Input<string>
    {
        public Dialog_InputText(Action<string> confirmAction, string title = null, string prompt = null, string defaultValue = null, Func<string, AcceptanceReport> validator = null) : base(confirmAction, s => s, title, prompt, defaultValue, validator)
        {
        }
    }
}

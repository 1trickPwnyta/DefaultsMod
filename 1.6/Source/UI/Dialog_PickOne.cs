using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Defaults.UI
{
    public class Dialog_PickOne<T> : Dialog_InputBox
    {
        private readonly HashSet<T> choices;
        private readonly Func<T, TaggedString> toString;
        private readonly Action<T> callback;
        private T selectedChoice;

        public Dialog_PickOne(string title, TaggedString text, IEnumerable<T> choices, Action<T> acceptAction, bool forceInput = false, bool destructive = false, Func<T, TaggedString> toString = null) : base(text, "Confirm".Translate(), title: title)
        {
            doCloseX = !forceInput;
            this.choices = choices.ToHashSet();
            buttonAAction = Accept;
            buttonADestructive = destructive;
            if (!forceInput)
            {
                buttonBText = "Cancel".Translate();
            }
            this.acceptAction = Accept;
            callback = acceptAction;
            if (toString != null)
            {
                this.toString = toString;
            }
            else
            {
                this.toString = t => t.ToString();
            }
            selectedChoice = choices.First();
        }

        private void Accept()
        {
            callback?.Invoke(selectedChoice);
        }

        public override float DoInput(Rect rect)
        {
            Listing_Standard listing = new Listing_Standard() { maxOneColumn = true };
            listing.Begin(rect);
            foreach (T choice in choices)
            {
                Rect choiceRect = listing.GetRect(30f);
                GUI.DrawTexture(choiceRect.LeftPartPixels(30f).ContractedBy(3f), selectedChoice.Equals(choice) ? Widgets.RadioButOnTex : typeof(Widgets).Field("RadioButOffTex").GetValue(null) as Texture2D);
                using (new TextBlock(TextAnchor.MiddleLeft)) Widgets.Label(choiceRect.RightPartPixels(choiceRect.width - 35f), toString(choice));
                if (Widgets.ButtonInvisible(choiceRect))
                {
                    selectedChoice = choice;
                    SoundDefOf.Click.PlayOneShot(null);
                }
            }
            listing.End();
            return listing.CurHeight;
        }
    }
}

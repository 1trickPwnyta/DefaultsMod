using UnityEngine;
using Verse;

namespace Defaults.UI
{
    public class Listing_StandardHighlight
    {
        private readonly Listing_Standard listing;
        private bool highlight = true;

        public bool Highlight
        {
            get
            {
                highlight = !highlight;
                return highlight;
            }
        }

        public float CurHeight => listing.CurHeight;

        public Listing_StandardHighlight(bool maxOneColumn)
        {
            listing = new Listing_Standard() { maxOneColumn = maxOneColumn };
        }

        public void Begin(Rect rect) => listing.Begin(rect);

        public void End() => listing.End();

        public Rect GetRect(float height)
        {
            Rect rect = listing.GetRect(height);
            if (Highlight)
            {
                Widgets.DrawLightHighlight(rect);
            }
            return rect;
        }
    }
}

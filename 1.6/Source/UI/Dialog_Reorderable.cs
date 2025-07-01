using System.Collections;
using UnityEngine;
using Verse;

namespace Defaults.UI
{
    public abstract class Dialog_Reorderable : Window
    {
        protected Rect reorderableRect;

        protected int ReorderableGroup { get; private set; }

        protected virtual IList ReorderableItems => null;

        public override void DoWindowContents(Rect inRect)
        {
            IList reorderableItems = ReorderableItems;
            if (reorderableItems != null && reorderableRect != null && Event.current.type == EventType.Repaint)
            {
                ReorderableGroup = ReorderableWidget.NewGroup((from, to) =>
                {
                    object o = reorderableItems[from];
                    reorderableItems.Insert(to, o);
                    reorderableItems.RemoveAt(from < to ? from : from + 1);
                }, ReorderableDirection.Vertical, reorderableRect);
            }
        }
    }
}

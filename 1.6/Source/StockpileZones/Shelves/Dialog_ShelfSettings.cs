using RimWorld;
using System.Collections.Generic;
using System;
using UnityEngine;
using Verse;
using Defaults.UI;

namespace Defaults.StockpileZones.Shelves
{
    public class Dialog_ShelfSettings : Window
    {
        private readonly ThingFilterUI.UIState state = new ThingFilterUI.UIState();

        public Dialog_ShelfSettings()
        {
            doCloseX = true;
            doCloseButton = true;
            optionalTitle = "Defaults_ShelfSettings".Translate();
        }

        public override Vector2 InitialSize => new Vector2(400f, 600f);

        public override void DoWindowContents(Rect inRect)
        {
            float buttonHeight = 30f;
            ZoneType shelfSettings = Settings.Get<ZoneType>(Settings.SHELF_SETTINGS);

            if (Widgets.ButtonText(new Rect(inRect.x, inRect.y, 160f, buttonHeight), "Priority".Translate() + ": " + shelfSettings.Priority.Label().CapitalizeFirst()))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (object obj in Enum.GetValues(typeof(StoragePriority)))
                {
                    StoragePriority storagePriority = (StoragePriority)obj;
                    if (storagePriority != StoragePriority.Unstored)
                    {
                        StoragePriority localPr = storagePriority;
                        list.Add(new FloatMenuOption(localPr.Label().CapitalizeFirst(), () =>
                        {
                            shelfSettings.Priority = localPr;
                        }));
                    }
                }
                Find.WindowStack.Add(new FloatMenu(list));
            }

            Rect lockRect = new Rect(inRect.width - 24f, inRect.y, 24f, 24f);
            UIUtility.DrawCheckButton(lockRect, UIUtility.LockIcon, "Defaults_LockSetting".Translate(), ref shelfSettings.locked);

            Rect filterRect = new Rect(inRect.x, inRect.y + buttonHeight, inRect.width, inRect.height - buttonHeight - CloseButSize.y);
            ThingFilterUI.DoThingFilterConfigWindow(filterRect, state, shelfSettings.filter, DefDatabase<ThingDef>.GetNamed("Shelf").building.fixedStorageSettings.filter, 8);
        }
    }
}

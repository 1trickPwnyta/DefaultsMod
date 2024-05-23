﻿using Defaults.StockpileZones;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Defaults.Schedule
{
    public class Dialog_StockpileZones : Window
    {
        private static Vector2 scrollPos;
        private static Texture2D stockpileIcon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true);
        private static ZoneType selectedZoneType;
        private ThingFilterUI.UIState state = new ThingFilterUI.UIState();

        public Dialog_StockpileZones()
        {
            this.doCloseX = true;
            this.doCloseButton = true;
            this.optionalTitle = "Defaults_StockpileZones".Translate();
        }

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(860f, 600f);
            }
        }

        public override void PreOpen()
        {
            base.PreOpen();
            selectedZoneType = null;
        }

        public override void DoWindowContents(Rect inRect)
        {
            float buttonWidth = 80f;
            float buttonHeight = 30f;
            if (Widgets.ButtonText(new Rect(inRect.x + inRect.width - buttonWidth, 0f, buttonWidth, buttonHeight), "Defaults_AddNew".Translate()))
            {
                DefaultsSettings.DefaultStockpileZones.Add(new ZoneType("Defaults_StockpileZoneTypeName".Translate(DefaultsSettings.DefaultStockpileZones.Count + 1), StorageSettingsPreset.DefaultStockpile));
                SoundDefOf.Click.PlayOneShotOnCamera(null);
            }

            float rowWidth = inRect.width - 300f - 16f;
            float rowHeight = 64f;
            Widgets.BeginScrollView(new Rect(inRect.x, inRect.y + buttonHeight, inRect.width - 300f, inRect.height - buttonHeight - Window.CloseButSize.y), ref scrollPos, new Rect(0f, 0f, rowWidth, rowHeight * DefaultsSettings.DefaultStockpileZones.Count));
            float y = 0f;

            Text.Anchor = TextAnchor.MiddleLeft;
            for (int i = 0; i < DefaultsSettings.DefaultStockpileZones.Count; i++)
            {
                ZoneType type = DefaultsSettings.DefaultStockpileZones[i];

                GUI.DrawTexture(new Rect(0f, y, rowHeight, rowHeight), stockpileIcon);

                Widgets.Label(new Rect(rowHeight + 8f, y, rowWidth - rowHeight - 8f - 24f - 8f, rowHeight), type.Name);

                if (Widgets.ButtonImage(new Rect(rowWidth - 24f - 8f, y + (rowHeight - 24f) / 2, 24f, 24f), TexButton.Delete, Color.white, Color.white * GenUI.SubtleMouseoverColor))
                {
                    DefaultsSettings.DefaultStockpileZones.Remove(type);
                    i--;
                    SoundDefOf.Click.PlayOneShotOnCamera(null);
                }

                Rect rowRect = new Rect(0f, y, rowWidth, rowHeight);
                if (Widgets.ButtonInvisible(rowRect))
                {
                    selectedZoneType = type;
                    SoundDefOf.Click.PlayOneShotOnCamera(null);
                }
                Widgets.DrawHighlightIfMouseover(rowRect);
                if (type == selectedZoneType)
                {
                    Widgets.DrawHighlightSelected(rowRect);
                }

                y += rowHeight;
            }
            Text.Anchor = TextAnchor.UpperLeft;

            Widgets.EndScrollView();

            if (DefaultsSettings.DefaultStockpileZones.Contains(selectedZoneType))
            {
                Rect filterRect = new Rect(inRect.width  - 300f, inRect.y + buttonHeight, 300f, inRect.height - buttonHeight - Window.CloseButSize.y);
                ThingFilterUI.DoThingFilterConfigWindow(filterRect, state, selectedZoneType.filter, null, 8);
            }
        }
    }
}
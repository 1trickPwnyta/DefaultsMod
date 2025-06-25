using RimWorld;
using System.Collections.Generic;
using System;
using UnityEngine;
using Verse;
using Verse.Sound;
using Defaults.UI;

namespace Defaults.StockpileZones
{
    [StaticConstructorOnStartup]
    public class Dialog_StockpileZones : Dialog_SettingsCategory
    {
        public static Texture2D DefaultStockpileIcon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true);
        public static Texture2D DumpingStockpileIcon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_DumpingStockpile", true);
        public static Texture2D CorpseStockpileIcon = ContentFinder<Texture2D>.Get(ModsConfig.AnomalyActive ? "UI/Icons/CorpseStockpileZone" : "UI/Designators/ZoneCreate_DumpingStockpile", true);

        private static Vector2 scrollPos;
        private static ZoneType selectedZoneType;
        private readonly ThingFilterUI.UIState state = new ThingFilterUI.UIState();

        public Dialog_StockpileZones(DefaultSettingsCategoryDef category) : base(category)
        {
        }

        public override Vector2 InitialSize => new Vector2(860f, 640f);

        public override void PreOpen()
        {
            base.PreOpen();
            selectedZoneType = Settings.Get<ZoneType>(Settings.SHELF_SETTINGS);
        }

        public override void DoSettings(Rect rect)
        {
            List<ZoneType> stockpileZones = Settings.Get<List<ZoneType>>(Settings.STOCKPILE_ZONES);
            ZoneType shelfSettings = Settings.Get<ZoneType>(Settings.SHELF_SETTINGS);

            float buttonWidth = 80f;
            float buttonHeight = 30f;
            if (Widgets.ButtonText(new Rect(rect.x + rect.width - buttonWidth, 0f, buttonWidth, buttonHeight), "Defaults_AddNew".Translate()))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (object obj in Enum.GetValues(typeof(StorageSettingsPreset)))
                {
                    StorageSettingsPreset preset = (StorageSettingsPreset)obj;
                    list.Add(new FloatMenuOption(preset.PresetName().CapitalizeFirst(), () =>
                    {
                        ZoneType newZoneType = new ZoneType(preset.PresetName().CapitalizeFirst() + " " + (stockpileZones.Count + 1), preset);
                        stockpileZones.Add(newZoneType);
                        selectedZoneType = newZoneType;
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            }

            float rowWidth = rect.width - 300f - 16f;
            float rowHeight = 64f;
            Widgets.BeginScrollView(new Rect(rect.x, rect.y + buttonHeight, rect.width - 300f, rect.height - buttonHeight - CloseButSize.y - 10f - ResetButtonSize.y - 10f), ref scrollPos, new Rect(0f, 0f, rowWidth, rowHeight * stockpileZones.Count));
            float y = 0f;

            Text.Anchor = TextAnchor.MiddleLeft;
            DoZoneTypeButton(shelfSettings, y, rowWidth, rowHeight);
            y += rowHeight;
            for (int i = 0; i < stockpileZones.Count; i++)
            {
                ZoneType type = stockpileZones[i];

                if (i < stockpileZones.Count - 1)
                {
                    if (Widgets.ButtonImage(new Rect(3f, y + (rowHeight - 18f) / 2, 18f, 18f), TexButton.ReorderDown, Color.white, Color.white * GenUI.SubtleMouseoverColor))
                    {
                        ZoneType next = stockpileZones[i + 1];
                        stockpileZones[i + 1] = type;
                        stockpileZones[i] = next;
                        SoundDefOf.Click.PlayOneShotOnCamera(null);
                    }
                }
                if (i > 0)
                {
                    if (Widgets.ButtonImage(new Rect(24f + 3f, y + (rowHeight - 18f) / 2, 18f, 18f), TexButton.ReorderUp, Color.white, Color.white * GenUI.SubtleMouseoverColor))
                    {
                        ZoneType prev = stockpileZones[i - 1];
                        stockpileZones[i - 1] = type;
                        stockpileZones[i] = prev;
                        SoundDefOf.Click.PlayOneShotOnCamera(null);
                    }
                }

                if (Widgets.ButtonImage(new Rect(rowWidth - 24f - 24f - 24f - 8f, y + (rowHeight - 24f) / 2, 24f, 24f), TexButton.Copy, Color.white, Color.white * GenUI.SubtleMouseoverColor))
                {
                    stockpileZones.Add(new ZoneType(type.Preset.PresetName().CapitalizeFirst() + " " + (stockpileZones.Count + 1), type));
                    SoundDefOf.Click.PlayOneShotOnCamera(null);
                }

                if (type.DesignatorType == typeof(Designator_ZoneAddStockpile_Custom))
                {
                    if (Widgets.ButtonImage(new Rect(rowWidth - 24f - 24f - 8f, y + (rowHeight - 24f) / 2, 24f, 24f), TexButton.Rename, Color.white, Color.white * GenUI.SubtleMouseoverColor))
                    {
                        Find.WindowStack.Add(new Dialog_RenameZoneType(type));
                    }
                }

                if (type.DesignatorType == typeof(Designator_ZoneAddStockpile_Custom))
                {
                    if (Widgets.ButtonImage(new Rect(rowWidth - 24f - 8f, y + (rowHeight - 24f) / 2, 24f, 24f), TexButton.Delete, Color.white, Color.white * GenUI.SubtleMouseoverColor))
                    {
                        stockpileZones.Remove(type);
                        i--;
                        SoundDefOf.Click.PlayOneShotOnCamera(null);
                    }
                }

                DoZoneTypeButton(type, y, rowWidth, rowHeight);

                y += rowHeight;
            }
            Text.Anchor = TextAnchor.UpperLeft;

            Widgets.EndScrollView();

            if (shelfSettings == selectedZoneType || stockpileZones.Contains(selectedZoneType))
            {
                if (Widgets.ButtonText(new Rect(rect.width - 300f, rect.y + buttonHeight, 160f, buttonHeight), "Priority".Translate() + ": " + selectedZoneType.Priority.Label().CapitalizeFirst()))
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
                                selectedZoneType.Priority = localPr;
                            }));
                        }
                    }
                    Find.WindowStack.Add(new FloatMenu(list));
                }

                Rect lockRect = new Rect(rect.width - 24f, rect.y + buttonHeight, 24f, 24f);
                UIUtility.DrawCheckButton(lockRect, UIUtility.LockIcon, "Defaults_LockSetting".Translate(), ref selectedZoneType.locked);

                Rect filterRect = new Rect(rect.width - 300f, rect.y + buttonHeight * 2, 300f, rect.height - buttonHeight * 2 - CloseButSize.y - 10f - ResetButtonSize.y - 10f);
                ThingFilterUI.DoThingFilterConfigWindow(filterRect, state, selectedZoneType.filter, StorageSettings.EverStorableFixedSettings().filter, 8);
            }
            else
            {
                selectedZoneType = Settings.Get<ZoneType>(Settings.SHELF_SETTINGS);
            }
        }

        private void DoZoneTypeButton(ZoneType type, float y, float rowWidth, float rowHeight)
        {
            GUI.color = type.IconColor;
            Widgets.DrawTextureFitted(new Rect(48f, y, rowHeight, rowHeight), type.Icon, 1f);
            GUI.color = Color.white;
            Widgets.Label(new Rect(48f + rowHeight + 8f, y, rowWidth - rowHeight - 8f - 24f - 24f - 24f - 8f, rowHeight), type.Name);

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
        }
    }
}

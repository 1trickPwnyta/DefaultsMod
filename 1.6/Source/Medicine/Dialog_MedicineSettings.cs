using Defaults.Defs;
using Defaults.UI;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Defaults.Medicine
{
    [StaticConstructorOnStartup]
    public class Dialog_MedicineSettings : Dialog_SettingsCategory_List
    {
        private static bool medicalCarePainting = false;
        private static readonly Texture2D[] careTextures = new[]
        {
            ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoCare"),
            ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoMeds"),
            ThingDefOf.MedicineHerbal.uiIcon,
            ThingDefOf.MedicineIndustrial.uiIcon,
            ThingDefOf.MedicineUltratech.uiIcon,
        };

        public Dialog_MedicineSettings() : base(DefDatabase<DefaultSettingsCategoryDef>.GetNamed("Medicine"))
        {
        }

        public override Vector2 InitialSize => new Vector2(426f, 680f);

        public override float DoPostSettings(Rect rect)
        {
            float y = rect.y;
            Widgets.Label(rect, ref y, "Defaults_MedicineAssignments".Translate());
            y += 10f;
            using (new TextBlock(TextAnchor.MiddleLeft))
            {
                MedicineOptions options = Settings.Get<MedicineOptions>(Settings.MEDICINE);
                DoRow(rect, ref y, ref options.DefaultCareForColonist, "MedGroupColonists", "MedGroupColonistsDesc");
                DoRow(rect, ref y, ref options.DefaultCareForPrisoner, "MedGroupPrisoners", "MedGroupPrisonersDesc");
                if (ModsConfig.IdeologyActive)
                {
                    DoRow(rect, ref y, ref options.DefaultCareForSlave, "MedGroupSlaves", "MedGroupSlavesDesc");
                }
                if (ModsConfig.AnomalyActive)
                {
                    DoRow(rect, ref y, ref options.DefaultCareForGhouls, "MedGroupGhouls", "MedGroupGhoulsDesc");
                }
                DoRow(rect, ref y, ref options.DefaultCareForTamedAnimal, "MedGroupTamedAnimals", "MedGroupTamedAnimalsDesc");
                y += 17f;
                DoRow(rect, ref y, ref options.DefaultCareForFriendlyFaction, "MedGroupFriendlyFaction", "MedGroupFriendlyFactionDesc");
                DoRow(rect, ref y, ref options.DefaultCareForNeutralFaction, "MedGroupNeutralFaction", "MedGroupNeutralFactionDesc");
                DoRow(rect, ref y, ref options.DefaultCareForHostileFaction, "MedGroupHostileFaction", "MedGroupHostileFactionDesc");
                y += 17f;
                DoRow(rect, ref y, ref options.DefaultCareForNoFaction, "MedGroupNoFaction", "MedGroupNoFactionDesc");
                DoRow(rect, ref y, ref options.DefaultCareForWildlife, "MedGroupWildlife", "MedGroupWildlifeDesc");
                if (ModsConfig.AnomalyActive)
                {
                    DoRow(rect, ref y, ref options.DefaultCareForEntities, "MedGroupEntities", "MedGroupEntitiesDesc");
                }
            }
            return y - rect.y;
        }

        private void DoRow(Rect rect, ref float y, ref MedicalCareCategory category, string labelKey, string tipKey)
        {
            Rect rect2 = new Rect(rect.x, y, rect.width, 28f);
            Rect rect3 = new Rect(rect.x, y, 230f, 28f);
            Rect rect4 = new Rect(230f, y, 140f, 28f);
            if (Mouse.IsOver(rect2))
            {
                Widgets.DrawLightHighlight(rect2);
            }
            TooltipHandler.TipRegionByKey(rect2, tipKey);
            Widgets.LabelFit(rect3, labelKey.Translate());
            MedicalCareSetter(rect4, ref category);
            y += 34f;
        }

        public void MedicalCareSetter(Rect rect, ref MedicalCareCategory medCare)
        {
            Rect rect2 = new Rect(rect.x, rect.y, rect.width / 5f, rect.height);
            for (int i = 0; i < 5; i++)
            {
                MedicalCareCategory mc = (MedicalCareCategory)i;
                Widgets.DrawHighlightIfMouseover(rect2);
                MouseoverSounds.DoRegion(rect2);
                GUI.DrawTexture(rect2, careTextures[i]);
                Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect2, false);
                if (draggableResult == Widgets.DraggableResult.Dragged)
                {
                    medicalCarePainting = true;
                }
                if ((medicalCarePainting && Mouse.IsOver(rect2) && medCare != mc) || draggableResult.AnyPressed())
                {
                    medCare = mc;
                    SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
                }
                if (medCare == mc)
                {
                    Widgets.DrawBox(rect2, 2, null);
                }
                if (Mouse.IsOver(rect2))
                {
                    TooltipHandler.TipRegion(rect2, () => mc.GetLabel().CapitalizeFirst(), 632165 + i * 17);
                }
                rect2.x += rect2.width;
            }
            if (!Input.GetMouseButton(0))
            {
                medicalCarePainting = false;
            }
        }
    }
}

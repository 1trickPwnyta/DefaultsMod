using RimWorld;
using UnityEngine;
using Verse;

namespace Defaults.Storyteller
{
    public class Dialog_Storyteller : Dialog_SettingsCategory
    {
        public Dialog_Storyteller(DefaultSettingsCategoryDef category) : base(category)
        {
        }

        public override string Title => "Defaults_Storyteller".Translate();

        public override Vector2 InitialSize => new Vector2(Page.StandardSize.x, Page.StandardSize.y);

        protected override Vector2 ResetButtonPosition(Rect rect) => new Vector2(rect.xMax - ResetButtonSize.x, rect.yMax - CloseButSize.y - 10f - ResetButtonSize.y);

        public override void PreOpen()
        {
            base.PreOpen();
            StorytellerUI.ResetStorytellerSelectionInterface();
        }

        public override void DoSettings(Rect rect)
        {
            Rect interfaceRect = new Rect(rect.x, rect.y, rect.width, rect.height - CloseButSize.y);
            StorytellerDef storyteller = DefDatabase<StorytellerDef>.GetNamed(DefaultsSettings.DefaultStoryteller);
            DifficultyDef difficulty = DefDatabase<DifficultyDef>.GetNamed(DefaultsSettings.DefaultDifficulty);
            Difficulty difficultyValues = DefaultsSettings.DefaultDifficultyValues;
            StorytellerUI.DrawStorytellerSelectionInterface(interfaceRect, ref storyteller, ref difficulty, ref difficultyValues, new Listing_Standard());
            DefaultsSettings.DefaultStoryteller = storyteller.defName;
            DefaultsSettings.DefaultDifficulty = difficulty.defName;
        }

        public static bool ShouldNotDoPermadeathSelection(Difficulty difficulty)
        {
            return !(difficulty is DifficultySub) && Current.ProgramState != ProgramState.Entry;
        }

        public static void DoPermadeathSelection(Listing_Standard infoListing, Difficulty difficulty)
        {
            bool settingsOpen = difficulty is DifficultySub;
            bool active = settingsOpen ? DefaultsSettings.DefaultPermadeath : Find.GameInitData.permadeathChosen && Find.GameInitData.permadeath;
            bool active2 = settingsOpen ? !DefaultsSettings.DefaultPermadeath : Find.GameInitData.permadeathChosen && !Find.GameInitData.permadeath;
            if (infoListing.RadioButton("ReloadAnytimeMode".Translate(), active2, 0f, "ReloadAnytimeModeInfo".Translate(), null))
            {
                if (settingsOpen)
                {
                    DefaultsSettings.DefaultPermadeath = false;
                }
                else
                {
                    Find.GameInitData.permadeathChosen = true;
                    Find.GameInitData.permadeath = false;
                }
            }
            infoListing.Gap(3f);
            if (infoListing.RadioButton("CommitmentMode".TranslateWithBackup("PermadeathMode"), active, 0f, "PermadeathModeInfo".Translate(), null))
            {
                if (settingsOpen)
                {
                    DefaultsSettings.DefaultPermadeath = true;
                }
                else
                {
                    Find.GameInitData.permadeathChosen = true;
                    Find.GameInitData.permadeath = true;
                }
            }
        }

        public static bool NonStandardAnomalyPlaystylesAllowed(AnomalyPlaystyleDef def, Difficulty difficulty)
        {
            return difficulty is DifficultySub || !Find.Scenario.standardAnomalyPlaystyleOnly || def == AnomalyPlaystyleDefOf.Standard;
        }

        public static AnomalyPlaystyleDef GetAnomalyPlaystyleDef(Difficulty difficulty)
        {
            return difficulty is DifficultySub
                ? DefDatabase<AnomalyPlaystyleDef>.GetNamed(DefaultsSettings.DefaultAnomalyPlaystyle)
                : difficulty.AnomalyPlaystyleDef;
        }

        public static void SetAnomalyPlaystyleDef(Difficulty difficulty, AnomalyPlaystyleDef def)
        {
            if (difficulty is DifficultySub)
            {
                DefaultsSettings.DefaultAnomalyPlaystyle = def.defName;
            }
            else
            {
                difficulty.AnomalyPlaystyleDef = def;
            }
        }
    }
}

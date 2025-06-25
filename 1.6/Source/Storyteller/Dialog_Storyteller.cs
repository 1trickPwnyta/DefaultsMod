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

        public override Vector2 InitialSize => new Vector2(Page.StandardSize.x, Page.StandardSize.y + 40f);

        public override void PreOpen()
        {
            base.PreOpen();
            StorytellerUI.ResetStorytellerSelectionInterface();
        }

        public override void DoSettings(Rect rect)
        {
            Rect interfaceRect = new Rect(rect.x, rect.y, rect.width, rect.height - CloseButSize.y - 10f - ResetButtonSize.y - 10f);
            StorytellerDef storyteller = Settings.Get<StorytellerDef>(Settings.STORYTELLER);
            DifficultyDef difficulty = Settings.Get<DifficultyDef>(Settings.DIFFICULTY);
            Difficulty difficultyValues = Settings.Get<Difficulty>(Settings.DIFFICULTY_VALUES);
            StorytellerUI.DrawStorytellerSelectionInterface(interfaceRect, ref storyteller, ref difficulty, ref difficultyValues, new Listing_Standard());
            Settings.Set(Settings.STORYTELLER, storyteller);
            Settings.Set(Settings.DIFFICULTY, difficulty);
        }
    }
}

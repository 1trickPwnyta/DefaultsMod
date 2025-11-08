using Defaults.UI;
using RimWorld;
using System.IO;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Defaults.General
{
    public class Dialog_SettingsBackupOptions : Dialog_Common
    {
        public override Vector2 InitialSize => new Vector2(750f, 400f);

        public override void DoWindowContents(Rect inRect)
        {
            base.DoWindowContents(inRect);

            SettingsBackupOptions options = Settings.Get<SettingsBackupOptions>(Settings.SETTINGS_BACKUP_OPTIONS);
            float y = 0f;

            using (new TextBlock(GameFont.Medium)) Widgets.Label(inRect, "Defaults_SettingsBackupOptionsTitle".Translate());
            y += 45f;

            using (new TextBlock(TextAnchor.MiddleLeft)) Widgets.Label(new Rect(inRect.x, inRect.y + y, inRect.width, 30f), "Defaults_SettingsBackupPath".Translate());
            options.BackupPath = Widgets.TextField(new Rect(inRect.x + 150f, inRect.y + y, inRect.width - 250f, 30f), options.BackupPath);
            if (Widgets.ButtonText(new Rect(inRect.xMax - 100f, inRect.y + y, 100f, 30f), "OpenFolder".Translate()))
            {
                DirectoryInfo directory = new DirectoryInfo(options.BackupPath);
                if (!directory.Exists)
                {
                    try
                    {
                        directory.Create();
                    }
                    catch (IOException)
                    {
                        Messages.Message("Defaults_InvalidFolder".Translate(options.BackupPath), MessageTypeDefOf.RejectInput, false);
                    }
                }
                if (directory.Exists)
                {
                    Application.OpenURL(directory.FullName);
                    SoundDefOf.Click.PlayOneShotOnCamera(null);
                }
            }
            y += 30f;
        }
    }
}

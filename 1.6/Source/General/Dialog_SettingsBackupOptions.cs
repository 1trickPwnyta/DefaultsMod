using Defaults.UI;
using RimWorld;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;

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
            string backupPathSafe = options.BackupPath;
            options.BackupPath = Widgets.TextField(new Rect(inRect.x + 150f, inRect.y + y, inRect.width - 250f, 30f), options.BackupPath);
            try
            {
                Path.GetFullPath(Path.Combine(options.BackupPath, SettingsBackupUtility.BackupName));
            }
            catch
            {
                options.BackupPath = backupPathSafe;
            }

            if (Widgets.ButtonText(new Rect(inRect.xMax - 100f, inRect.y + y, 100f, 30f), "OpenFolder".Translate()))
            {
                SettingsBackupUtility.OpenBackupFolder();
            }
            y += 35f;

            if (Widgets.ButtonText(new Rect(inRect.x, inRect.y + y, inRect.width / 2 - 5f, 30f), "Defaults_BackupNow".Translate()))
            {
                Find.WindowStack.Add(new Dialog_InputText(filename =>
                {
                    if (SettingsBackupUtility.BackUpNow(filename))
                    {
                        Messages.Message("Defaults_SettingsBackupComplete".Translate(filename), MessageTypeDefOf.PositiveEvent, false);
                    }
                }, title: "Defaults_BackupNow".Translate(), prompt: "Defaults_EnterBackupName".Translate(), defaultValue: SettingsBackupUtility.BackupName, validator: filename =>
                {
                    try
                    {
                        if (filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                        {
                            return "Defaults_InvalidFilename".Translate();
                        }
                        if (File.Exists(Path.GetFullPath(Path.Combine(options.BackupPath, filename))))
                        {
                            return "Defaults_FileExists".Translate();
                        }
                    }
                    catch (PathTooLongException)
                    {
                        return "Default_PathTooLong".Translate();
                    }
                    catch
                    {
                        return "Defaults_InvalidFilename".Translate();
                    }
                    return AcceptanceReport.WasAccepted;
                }));
            }
            if (Widgets.ButtonText(new Rect(inRect.x + inRect.width / 2 + 5f, inRect.y + y, inRect.width / 2 - 5f, 30f), "Defaults_RestoreFromBackup".Translate()))
            {
                try
                {
                    DirectoryInfo directory = new DirectoryInfo(options.BackupPath);
                    Find.WindowStack.Add(new Dialog_PickOne<FileInfo>("Defaults_RestoreFromBackup".Translate(), "Defaults_SelectBackupFile".Translate(), directory.GetFiles(), f =>
                    {
                        SettingsBackupUtility.RestoreBackup(f.Name);
                    }, destructive: true, toString: f => f.Name + "    " + f.LastWriteTime.ToString(CultureInfo.InstalledUICulture).Colorize(ColoredText.DateTimeColor)));
                }
                catch (Exception e)
                {
                    Messages.Message("Defaults_SettingsRestoreFailed".Translate(e.ToString()), MessageTypeDefOf.RejectInput, false);
                }
            }
            y += 35f;
        }
    }
}

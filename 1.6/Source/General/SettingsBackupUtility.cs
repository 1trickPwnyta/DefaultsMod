using HarmonyLib;
using RimWorld;
using System;
using System.IO;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Defaults.General
{
    public static class SettingsBackupUtility
    {
        private static SettingsBackupOptions Options => Settings.Get<SettingsBackupOptions>(Settings.SETTINGS_BACKUP_OPTIONS);
        private static string SettingsPath = typeof(LoadedModManager).Method("GetSettingsFilename").Invoke(null, new object[] { DefaultsMod.Mod.Content.FolderName, DefaultsMod.Mod.GetType().Name }).ToString();

        public static string BackupName => "Defaults_BackupName".Translate(new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds());

        private static bool CreateBackupFolder()
        {
            DirectoryInfo directory = new DirectoryInfo(Options.BackupPath);
            if (!directory.Exists)
            {
                try
                {
                    directory.Create();
                }
                catch
                {
                    Messages.Message("Defaults_InvalidFolder".Translate(Options.BackupPath), MessageTypeDefOf.RejectInput, false);
                    return false;
                }
            }
            return true;
        }

        public static bool BackUpNow(string name = null)
        {
            if (CreateBackupFolder())
            {
                try
                {
                    if (name.NullOrEmpty())
                    {
                        name = BackupName;
                    }
                    string backupPath = Path.Combine(Options.BackupPath, name);
                    File.Copy(SettingsPath, backupPath);
                    return true;
                }
                catch (Exception e)
                {
                    Messages.Message("Defaults_SettingsBackupFailed".Translate(e.ToString()), MessageTypeDefOf.RejectInput, false);
                }
            }
            return false;
        }

        public static void RestoreBackup(string name)
        {
            try
            {
                string backupPath = Path.Combine(Options.BackupPath, name);
                File.Copy(backupPath, SettingsPath, true);
                LoadedModManager.ReadModSettings<DefaultsSettings>(DefaultsMod.Mod.Content.FolderName, DefaultsMod.Mod.GetType().Name);
                SoundDefOf.GameStartSting.PlayOneShot(null);
                Find.WindowStack.Add(new Dialog_MessageBox("Defaults_SettingsRestoreComplete".Translate(name), "Defaults_Restart".Translate(), GenCommandLine.Restart, "Defaults_NotNow".Translate(), null, "Defaults_RestartRequired".Translate(), false, GenCommandLine.Restart));
            }
            catch (Exception e)
            {
                Messages.Message("Defaults_SettingsRestoreFailed".Translate(e.ToString()), MessageTypeDefOf.RejectInput, false);
            }
        }

        public static void OpenBackupFolder()
        {
            if (CreateBackupFolder())
            {
                try
                {
                    Application.OpenURL(Options.BackupPath);
                    SoundDefOf.Click.PlayOneShotOnCamera(null);
                }
                catch
                {
                    Messages.Message("Defaults_CantOpenFolder".Translate(Options.BackupPath), MessageTypeDefOf.RejectInput, false);
                }
            }
        }
    }
}

using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Defaults.General
{
    public static class SettingsBackupUtility
    {
        public static readonly string settingsExt = ".1tpdsb";
        private static readonly string settingsPath = typeof(LoadedModManager).Method("GetSettingsFilename").Invoke(null, new object[] { DefaultsMod.Mod.Content.FolderName, DefaultsMod.Mod.GetType().Name }).ToString();

        private static SettingsBackupOptions Options => Settings.Get<SettingsBackupOptions>(Settings.SETTINGS_BACKUP_OPTIONS);

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
                    name += settingsExt;
                    string backupPath = Path.Combine(Options.BackupPath, name);
                    File.Copy(settingsPath, backupPath, true);
                    PurgeBackups();
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
                File.Copy(backupPath, settingsPath, true);
                LoadedModManager.ReadModSettings<DefaultsSettings>(DefaultsMod.Mod.Content.FolderName, DefaultsMod.Mod.GetType().Name);
                SoundDefOf.GameStartSting.PlayOneShot(null);
                Messages.Message("Defaults_SettingsRestoreComplete".Translate(name), MessageTypeDefOf.SilentInput, false);
            }
            catch (Exception e)
            {
                Messages.Message("Defaults_SettingsRestoreFailed".Translate(e.ToString()), MessageTypeDefOf.RejectInput, false);
            }
        }

        public static void PurgeBackups()
        {
            SettingsBackupOptions options = Options;
            try
            {
                IEnumerable<FileInfo> files = GetBackupFiles().Where(f => !options.PinnedBackups.Contains(f.Name));
                if (options.Duration == SettingsBackupDuration.Count)
                {
                    foreach (FileInfo file in files.OrderByDescending(f => f.LastWriteTime).Skip(options.DurationCount))
                    {
                        file.Delete();
                    }
                }
                if (options.Duration == SettingsBackupDuration.TimeInterval)
                {
                    foreach (FileInfo file in files.Where(f => f.LastWriteTime < DateTime.Now.AddDays(-options.DurationDays)))
                    {
                        file.Delete();
                    }
                }
            }
            catch (Exception e)
            {
                Messages.Message("Defaults_SettingsBackupPurgeFailed".Translate(e.ToString()), MessageTypeDefOf.RejectInput, false);
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

        public static IEnumerable<FileInfo> GetBackupFiles()
        {
            if (CreateBackupFolder())
            {
                try
                {
                    DirectoryInfo directory = new DirectoryInfo(Options.BackupPath);
                    return directory.GetFiles().Where(f => IsBackupFile(f));
                }
                catch (Exception e)
                {
                    Messages.Message("Defaults_GetBackupFilesFailed".Translate(Options.BackupPath, e.ToString()), MessageTypeDefOf.RejectInput, false);
                }
            }
            return new FileInfo[] { };
        }

        private static bool IsBackupFile(FileInfo file)
        {
            IEnumerator<string> lines = null;
            try
            {
                if (file.Exists)
                {
                    return file.Name.EndsWith(settingsExt);
                }
            }
            catch
            {
                Log.Warning("Couldn't determine whether " + file + " is a settings backup file. Assuming no.");
            }
            finally
            {
                lines?.Dispose();
            }
            return false;
        }

        public static string GetLabel(this SettingsBackupFrequency frequency)
        {
            switch (frequency)
            {
                case SettingsBackupFrequency.Never: return "Defaults_SettingsBackupFrequency_Never".Translate();
                case SettingsBackupFrequency.Startup: return "Defaults_SettingsBackupFrequency_Startup".Translate();
                case SettingsBackupFrequency.OnChange: return "Defaults_SettingsBackupFrequency_OnChange".Translate();
                default: throw new ArgumentException("Invalid settings backup frequency: " + frequency);
            }
        }

        public static string GetLabel(this SettingsBackupDuration duration)
        {
            switch (duration)
            {
                case SettingsBackupDuration.Forever: return "Defaults_SettingsBackupDuration_Forever".Translate();
                case SettingsBackupDuration.Count: return "Defaults_SettingsBackupDuration_Count".Translate();
                case SettingsBackupDuration.TimeInterval: return "Defaults_SettingsBackupDuration_TimeInterval".Translate();
                default: throw new ArgumentException("Invalid settings backup duration: " + duration);
            }
        }
    }
}

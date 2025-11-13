using System.Collections.Generic;
using System.IO;
using Verse;

namespace Defaults.General
{
    public class SettingsBackupOptions : IExposable
    {
        private static readonly string defaultBackupPath = Path.Combine(GenFilePaths.SaveDataFolderPath, "DefaultSettingsBackup");

        public string BackupPath = defaultBackupPath;
        public SettingsBackupFrequency Frequency = SettingsBackupFrequency.Startup;
        public SettingsBackupDuration Duration = SettingsBackupDuration.Count;
        public int DurationCount = 5;
        public int DurationDays = 7;
        public HashSet<string> PinnedBackups = new HashSet<string>();

        public void ExposeData()
        {
            Scribe_Values.Look(ref BackupPath, "BackupPath", defaultBackupPath);
            Scribe_Values.Look(ref Frequency, "Frequency", SettingsBackupFrequency.Startup);
            Scribe_Values.Look(ref Duration, "Duration", SettingsBackupDuration.Count);
            Scribe_Values.Look(ref DurationCount, "DurationCount", 5);
            Scribe_Values.Look(ref DurationDays, "DurationDays", 7);
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                try
                {
                    PinnedBackups.RemoveWhere(b => !File.Exists(Path.Combine(BackupPath, b)));
                }
                catch
                {
                    Log.Warning("Failed to purge non-existent pinned backups.");
                }
            }
            Scribe_Collections.Look(ref PinnedBackups, "PinnedBackups", LookMode.Value);
            if (Scribe.mode == LoadSaveMode.PostLoadInit && PinnedBackups == null)
            {
                PinnedBackups = new HashSet<string>();
            }
        }
    }

    public enum SettingsBackupFrequency
    {
        Never,
        Startup,
        OnChange
    }

    public enum SettingsBackupDuration
    {
        Forever,
        Count,
        TimeInterval
    }
}

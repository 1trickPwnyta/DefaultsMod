using System.IO;
using Verse;

namespace Defaults.General
{
    public class SettingsBackupOptions : IExposable
    {
        private static readonly string defaultBackupPath = Path.Combine(GenFilePaths.SaveDataFolderPath, "DefaultSettingsBackup");

        public string BackupPath = defaultBackupPath;

        public void ExposeData()
        {
            Scribe_Values.Look(ref BackupPath, "BackupPath", defaultBackupPath);
        }
    }
}

using System;
using Verse;

namespace Defaults
{
    public static class Settings
    {
        public const string SCHEDULES = "DefaultSchedules";
        public const string NEXT_SCHEDULE = "NextScheduleIndex";
        public const string MEDICINE = "DefaultMedicineOptions";
        public const string BABY_FEEDING = "DefaultBabyFeedingOptions";

        public const string HOSTILITY_RESPONSE = "DefaultHostilityResponse";
        public const string MEDICINE_TO_CARRY = "DefaultMedicineToCarry";
        public const string MEDICINE_AMOUNT_TO_CARRY = "DefaultMedicineAmountToCarry";
        public const string GUESTS_CARRY_MEDICINE = "GuestsCarryMedicine";
        public const string PLANT_TYPE = "DefaultPlantType";
        public const string AUTO_HOME_AREA = "DefaultAutoHomeArea";
        public const string AUTO_REBUILD = "DefaultAutoRebuild";
        public const string MANUAL_PRIORITIES = "DefaultManualPriorities";
        public const string PREGNANCY_APPROACH = "DefaultPregnancyApproach";
        public const string TARGET_TEMP_COOLER = "DefaultTargetTemperatureCooler";
        public const string TARGET_TEMP_HEATER = "DefaultTargetTemperatureHeater";
        public const string MECH_WORK_MODE_ADDITIONAL = "DefaultWorkModeAdditional";
        public const string MECH_WORK_MODE_FIRST = "DefaultWorkModeFirst";
        public const string MECH_WORK_MODE_SECOND = "DefaultWorkModeSecond";

        public static T Get<T>(string key)
        {
            foreach (DefaultSettingsCategoryDef category in DefDatabase<DefaultSettingsCategoryDef>.AllDefsListForReading)
            {
                if (category.Worker.GetSetting(key, out T value))
                {
                    return value;
                }
            }
            throw new Exception("No setting for " + key);
        }

        public static T GetValue<T>(string key) where T : struct => Get<T?>(key).Value;

        public static void Set<T>(string key, T value)
        {
            foreach (DefaultSettingsCategoryDef category in DefDatabase<DefaultSettingsCategoryDef>.AllDefsListForReading)
            {
                if (category.Worker.SetSetting(key, value))
                {
                    return;
                }
            }
            throw new Exception("No setting for " + key);
        }

        public static void SetValue<T>(string key, T value) where T : struct => Set<T?>(key, value);
    }
}

using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults.WorkPriorities
{
    [StaticConstructorOnStartup]
    public static class WorkPriorityValue
    {
        public const int TynansChoice = -1;
        public const int DoNotDo = 0;

        private static readonly Texture2D iconTynansChoice = Widgets.CheckboxPartialTex;
        private static readonly Texture2D iconOn = Widgets.CheckboxOnTex;
        private static readonly Texture2D iconOff = Widgets.CheckboxOffTex;
        private static readonly Texture2D iconNumber = WidgetsWork.WorkBoxBGTex_Mid;
        private static readonly Type workTabSettings = AccessTools.TypeByName("WorkTab.Settings");

        public static int Max =>
                // Support for Work Tab mod
                workTabSettings != null ? (int)workTabSettings.Field("maxPriority").GetValue(null) : 4;

        private static bool ManualPriorities => Settings.GetValue<bool>(Settings.MANUAL_PRIORITIES);

        public static string GetLabel(int value)
        {
            switch (value)
            {
                case TynansChoice: return "Defaults_WorkPriorityTynansChoice".Translate();
                case DoNotDo: return "Defaults_WorkPriorityDoNotDo".Translate();
                default: return ManualPriorities ? value.ToString() : "Defaults_WorkPriorityDo".Translate().ToString();
            }
        }

        public static Texture2D GetIcon(int value)
        {
            switch (value)
            {
                case TynansChoice: return iconTynansChoice;
                case DoNotDo: return iconOff;
                default: return ManualPriorities ? iconNumber : iconOn;
            }
        }

        public static List<FloatMenuOption> GetOptions(Action<int> optionSelected) => Enumerable.Range(TynansChoice, Max + 2).Where(i => ManualPriorities || i == 3 || i < 1).Select(i => new FloatMenuOption(GetLabel(i).CapitalizeFirst(), () => optionSelected(i), GetIcon(i), Color.white)).ToList();
    }
}

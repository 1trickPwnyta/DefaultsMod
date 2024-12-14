﻿using RimWorld;
using UnityEngine;
using Verse;

namespace Defaults.PlaySettings
{
    public static class PlaySettingsUtility
    {
        public static void DrawAutoRebuildButton(Rect rect)
        {
            ButtonUtility.DrawCheckButton(rect, TexButton.AutoRebuild, "AutoRebuildButton".Translate(), ref DefaultsSettings.DefaultAutoRebuild);
        }

        public static void DrawAutoHomeAreaButton(Rect rect)
        {
            ButtonUtility.DrawCheckButton(rect, TexButton.AutoHomeArea, "AutoHomeAreaToggleButton".Translate(), ref DefaultsSettings.DefaultAutoHomeArea);
        }
    }
}

﻿using HarmonyLib;
using UnityEngine;
using Verse;

namespace Defaults
{
    public class Listing_StandardHighlight : Listing_Standard
    {
        private bool highlight = true;

        public bool Highlight
        {
            get
            {
                highlight = !highlight;
                return highlight;
            }
        }
    }

    [HarmonyPatch(typeof(Listing))]
    [HarmonyPatch(nameof(Listing.GetRect))]
    public static class Patch_Listing_GetRect
    {
        public static void Postfix(Listing __instance, Rect __result)
        {
            if (__instance is Listing_StandardHighlight)
            {
                Listing_StandardHighlight listing = (Listing_StandardHighlight)__instance;
                if (listing.Highlight)
                {
                    Widgets.DrawLightHighlight(__result);
                }
            }
        }
    }
}

using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Defaults.HostilityResponse
{
    public static class HostilityResponseModeUtility
    {
        public static void SetHostilityResponseMode(Pawn pawn, Pawn_PlayerSettings settings)
        {
            if (settings != null && pawn.Faction == Faction.OfPlayer && !pawn.IsGhoul)
            {
                settings.hostilityResponse = Settings.GetValue<HostilityResponseMode>(Settings.HOSTILITY_RESPONSE);
                if (pawn.WorkTagIsDisabled(WorkTags.Violent) && settings.hostilityResponse == HostilityResponseMode.Attack)
                {
                    settings.hostilityResponse = HostilityResponseMode.Flee;
                }
            }
        }

        public static void DrawResponseButton(Rect rect)
        {
            HostilityResponseMode mode = Settings.GetValue<HostilityResponseMode>(Settings.HOSTILITY_RESPONSE);
            Widgets.Dropdown(rect, null, new Color(0.84f, 0.84f, 0.84f), new Func<object, HostilityResponseMode>(DrawResponseButton_GetResponse), new Func<object, IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>>(DrawResponseButton_GenerateMenu), null, mode.GetIcon(), null, null, null, true, new float?(4f));
            if (Mouse.IsOver(rect))
            {
                TooltipHandler.TipRegion(rect, "HostilityReponseTip".Translate() + "\n\n" + "HostilityResponseCurrentMode".Translate() + ": " + mode.GetLabel());
            }
        }

        private static HostilityResponseMode DrawResponseButton_GetResponse(object obj)
        {
            return Settings.GetValue<HostilityResponseMode>(Settings.HOSTILITY_RESPONSE);
        }

        private static IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>> DrawResponseButton_GenerateMenu(object obj)
        {
            IEnumerator enumerator = Enum.GetValues(typeof(HostilityResponseMode)).GetEnumerator();
            while (enumerator.MoveNext())
            {
                HostilityResponseMode response = (HostilityResponseMode)enumerator.Current;
                yield return new Widgets.DropdownMenuElement<HostilityResponseMode>
                {
                    option = new FloatMenuOption(response.GetLabel(), delegate ()
                    {
                        Settings.SetValue(Settings.HOSTILITY_RESPONSE, response);
                    }, response.GetIcon(), Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false),
                    payload = response
                };
            }
        }
    }
}

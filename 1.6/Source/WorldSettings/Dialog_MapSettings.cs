using Defaults.WorldSettings;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults.MapSettings
{
    public class Dialog_MapSettings : Window
    {
        private static readonly int[] MapSizes = new int[]
        {
            200,
            225,
            250,
            275,
            300,
            325
        };

        private static readonly int[] TestMapSizes = new int[]
        {
            350,
            400
        };

        public Dialog_MapSettings()
        {
            doCloseX = true;
            doCloseButton = true;
            absorbInputAroundWindow = true;
            closeOnClickedOutside = true;
        }

        public override Vector2 InitialSize => new Vector2(483f, 500f);

        public override void DoWindowContents(Rect inRect)
        {
            MapOptions options = Settings.Get<MapOptions>(Settings.MAP);

            Listing_Standard listing = new Listing_Standard { ColumnWidth = 200f };
            listing.Begin(inRect.AtZero());

            Text.Font = GameFont.Medium;
            listing.Label("MapSize".Translate());
            Text.Font = GameFont.Small;
            IEnumerable<int> enumerable = MapSizes.AsEnumerable();
            if (Prefs.TestMapSizes)
            {
                enumerable = enumerable.Concat(TestMapSizes);
            }
            foreach (int num in enumerable)
            {
                if (num == 200)
                {
                    listing.Label("MapSizeSmall".Translate());
                }
                else if (num == 250)
                {
                    listing.Gap(10f);
                    listing.Label("MapSizeMedium".Translate());
                }
                else if (num == 300)
                {
                    listing.Gap(10f);
                    listing.Label("MapSizeLarge".Translate());
                }
                else if (num == 350)
                {
                    listing.Gap(10f);
                    listing.Label("MapSizeExtreme".Translate());
                }
                string label = "MapSizeDesc".Translate(num, num * num);
                if (listing.RadioButton(label, options.DefaultMapSize == num, 0f, null, null))
                {
                    options.DefaultMapSize = num;
                }
            }
            listing.NewColumn();

            Text.Font = GameFont.Medium;
            listing.Label("MapStartSeason".Translate());
            Text.Font = GameFont.Small;
            listing.Label("");
            if (listing.RadioButton("MapStartSeasonDefault".Translate(), options.DefaultStartingSeason == Season.Undefined, 0f, null, null))
            {
                options.DefaultStartingSeason = Season.Undefined;
            }
            if (listing.RadioButton(Season.Spring.LabelCap(), options.DefaultStartingSeason == Season.Spring, 0f, null, null))
            {
                options.DefaultStartingSeason = Season.Spring;
            }
            if (listing.RadioButton(Season.Summer.LabelCap(), options.DefaultStartingSeason == Season.Summer, 0f, null, null))
            {
                options.DefaultStartingSeason = Season.Summer;
            }
            if (listing.RadioButton(Season.Fall.LabelCap(), options.DefaultStartingSeason == Season.Fall, 0f, null, null))
            {
                options.DefaultStartingSeason = Season.Fall;
            }
            if (listing.RadioButton(Season.Winter.LabelCap(), options.DefaultStartingSeason == Season.Winter, 0f, null, null))
            {
                options.DefaultStartingSeason = Season.Winter;
            }
            listing.End();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Defaults.ResourceCategories
{
    public class Dialog_ResourceCategories : Dialog_SettingsCategory
    {
        private static readonly List<ThingCategoryDef> rootThingCategories = DefDatabase<ThingCategoryDef>.AllDefs.Where(c => c.resourceReadoutRoot && CountAsResource(c)).ToList();

        public Dialog_ResourceCategories(DefaultSettingsCategoryDef category) : base(category)
        {
        }

        private static bool CountAsResource(ThingCategoryDef def)
        {
            return def.childThingDefs.Any(d => d.CountAsResource) || def.childCategories.Any(c => CountAsResource(c));
        }

        public override string Title => "Defaults_ResourceCategories".Translate();

        public override Vector2 InitialSize => new Vector2(406f, 640f);

        public override void DoSettings(Rect rect)
        {
            Listing_ResourceCategories listing_ResourceCategories = new Listing_ResourceCategories();
            listing_ResourceCategories.Begin(rect);
            listing_ResourceCategories.nestIndentWidth = 7f;
            listing_ResourceCategories.lineHeight = 24f;
            listing_ResourceCategories.verticalSpacing = 0f;
            foreach (ThingCategoryDef def in rootThingCategories)
            {
                listing_ResourceCategories.DoCategory(def.treeNode, 0);
            }
            listing_ResourceCategories.End();
        }
    }
}

using Verse;

namespace Defaults.MechWorkModes
{
    // Patched manually in mod constructor
    public static class Patch_MechanitorControlGroup
    {
        public static void Postfix(ref MechWorkModeDef ___workMode)
        {
            ___workMode = DefaultsSettings.DefaultWorkModeAdditional;
        }
    }
}

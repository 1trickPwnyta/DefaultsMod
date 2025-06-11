using Verse;

namespace Defaults.MechWorkModes
{
    // Patched manually in mod constructor
    public static class Patch_MechanitorControlGroup
    {
        public static void Postfix(ref MechWorkModeDef ___workMode)
        {
            ___workMode = Settings.Get<MechWorkModeDef>(Settings.MECH_WORK_MODE_ADDITIONAL);
        }
    }
}

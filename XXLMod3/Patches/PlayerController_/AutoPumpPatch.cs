using HarmonyLib;

namespace XXLModCV.Patches.PlayerController_
{
    [HarmonyPatch(typeof(PlayerController), "AutoPump")]
    class AutoPumpPatch
    {
        static bool Prefix()
        {
            if (!Main.settings.AutoPump && Main.enabled)
            {
                return false;
            }
            return true;
        }
    }
}
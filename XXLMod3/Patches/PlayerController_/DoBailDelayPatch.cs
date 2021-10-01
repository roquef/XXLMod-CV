using HarmonyLib;

namespace XXLModCV.Patches.PlayerController_
{
    [HarmonyPatch(typeof(PlayerController), "DoBailDelay")]

    class RespawnCancel
    {
        private static bool Prefix()
        {
            if (Main.enabled && !Main.settings.AutoRespawn)
            {
                PlayerController.Instance.Invoke("DoBail", 9999f);
                return false;
            }
            return true;
        }
    }
}
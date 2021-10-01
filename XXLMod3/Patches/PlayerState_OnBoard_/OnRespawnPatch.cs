using HarmonyLib;
using XXLModCV.PlayerStates;

namespace XXLModCV.Patches.PlayerState_OnBoard_
{
    [HarmonyPatch(typeof(PlayerState_OnBoard), "OnRespawn")]
    class OnRespawnPatch
    {
        static bool Prefix(PlayerState_OnBoard __instance)
        {
            if (Main.enabled)
            {
                EventManager.Instance.OnRespawn();
                PlayerController.Instance.ResetAllAnimations();
                PlayerController.Instance.AnimGrindTransition(false);
                PlayerController.Instance.AnimOllieTransition(false);
                PlayerController.Instance.AnimSetupTransition(false);
                __instance.DoTransition(typeof(XXLModCV.PlayerStates.Custom_Riding), null);
                return false;
            }
            return true;
        }
    }
}
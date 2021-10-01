using HarmonyLib;
using XXLModCV.PlayerStates;

namespace XXLModCV.Patches.PlayerState_OnBoard_
{
    [HarmonyPatch(typeof(PlayerState_OnBoard), "OnBailed")]
    class OnBailedPatch
    {
        static bool Prefix(PlayerState_OnBoard __instance)
        {
            if (Main.enabled)
            {
                PlayerController.Instance.ResetAllAnimations();
                PlayerController.Instance.AnimGrindTransition(false);
                PlayerController.Instance.AnimOllieTransition(false);
                PlayerController.Instance.AnimSetupTransition(false);
                __instance.DoTransition(typeof(XXLModCV.PlayerStates.Custom_Bailed), null);
                return false;
            }
            return true;
        }
    }
}
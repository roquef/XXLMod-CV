using HarmonyLib;
using UnityEngine;
using XXLModCV.Controller;

namespace XXLModCV.Patches.BoardController_
{
    [HarmonyPatch(typeof(BoardController), "UpdateReferenceBoardTargetRotation")]
    class UpdateReferenceBoardTargetRotationPatch
    {
        static void Prefix(BoardController __instance, ref Quaternion ____bufferedRotation)
        {
            if (XXLController.Instance.IsPrimoFlip)
            {
                ____bufferedRotation = __instance.boardTransform.rotation;
                Vector3 view = ____bufferedRotation * Vector3.forward;
                ____bufferedRotation.SetLookRotation(view, Vector3.ProjectOnPlane(PlayerController.Instance.skaterController.skaterTargetTransform.up,__instance.boardTransform.forward));
                return;
            }
        }
    }
}
﻿using HarmonyLib;
using ReplayEditor;

namespace XXLModCV.Patches.ReplayEditorController_
{
    [HarmonyPatch(typeof(ReplayEditorController), "OnEnable")]
    class OnEnablePrefixPatch
    {
        static void Prefix(ReplayEditorController __instance)
        {
            if (Main.enabled)
            {
                __instance.startAtLastRespawn = !Main.settings.StartReplayAtLastFrame;
            }
        }
    }
}
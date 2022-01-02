using HarmonyLib;
using Sandbox.Game;
using Sandbox.Game.World;
using System;
using System.Reflection;

namespace RDR2DeathScreen.Patches
{
    [HarmonyPatch]
    internal class BloodOverlayPatch
    {
        private static readonly Type BloodOverlay = Type.GetType("Sandbox.Game.Components.MyRenderComponentCharacter, Sandbox.Game", true);
        private static MethodBase TargetMethod()
        {
            return AccessTools.Method(BloodOverlay, "GetHUDBloodAlpha");
        }

        private static void Postfix(ref float __result)
        {
            if (MyVisualScriptLogicProvider.IsPlayerDead(MySession.Static.LocalPlayerId))
            {
                __result = 0f;
            }
        }
    }
}

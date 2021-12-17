using HarmonyLib;
using Sandbox;
using Sandbox.Engine.Platform;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using System.Threading;
using VRageRender;

namespace RDR2DeathScreen.Patches
{
    [HarmonyPatch(typeof(MyCharacter), "DieInternal")]
    internal class ExamplePatch
    {
        public static MyCharacter Instance = null;

        public static void Prefix(MyCharacter __instance)
        {
            Instance = __instance;
        }
    }
}

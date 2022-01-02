using HarmonyLib;
using Sandbox.Game.Entities.Character;

namespace RDR2DeathScreen.Patches
{
    [HarmonyPatch(typeof(MyCharacter), "StartRespawn")]
    internal class RespawnPatch
    {
        public static void Prefix(ref float respawnTime)
        {
            respawnTime += 8f;
        }
    }
}

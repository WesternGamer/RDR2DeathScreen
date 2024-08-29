using HarmonyLib;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;

namespace RDR2DeathScreen.Patches
{
    [HarmonyPatch(typeof(MyGuiScreenGamePlay), "HandleUnhandledInput")]
    internal class InputPatch
    {
        private static bool Prefix()
        {
            if (MyGuiScreenGamePlay.DisableInput == true && MyScreenManager.IsScreenOfTypeOpen(typeof(DeathScreen)))
            {
                return false;
            }
                
            return true;
        }
    }
}

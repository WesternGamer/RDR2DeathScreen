using HarmonyLib;
using Sandbox.Game.Gui;

namespace RDR2DeathScreen.Patches
{
    [HarmonyPatch(typeof(MyGuiScreenGamePlay), "HandleUnhandledInput")]
    internal class InputPatch
    {
        private static bool Prefix()
        {
            if (MyGuiScreenGamePlay.DisableInput == true)
            {
                return false;
            }
                
            return true;
        }
    }
}

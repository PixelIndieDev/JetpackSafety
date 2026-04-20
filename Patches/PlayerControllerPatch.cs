using GameNetcodeStuff;
using HarmonyLib;

namespace JetpackSafety.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal static class PlayerControllerPatch
    {
        [HarmonyPatch("KillPlayer")]
        [HarmonyPrefix]
        [HarmonyPriority(Priority.Last)]
        static bool PreventDeath(PlayerControllerB __instance, CauseOfDeath causeOfDeath)
        {
            if (causeOfDeath == CauseOfDeath.Gravity && IsUsingJetpack(__instance))
            {
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(PlayerControllerB), "DamagePlayer")]
        [HarmonyPrefix]
        [HarmonyPriority(Priority.Last)]
        static bool PreventDamage(PlayerControllerB __instance, CauseOfDeath causeOfDeath)
        {
            if (causeOfDeath == CauseOfDeath.Gravity && IsUsingJetpack(__instance)) {
                return false;
            }
            return true;
        }

        private static bool IsUsingJetpack(PlayerControllerB player)
        {
            return player.jetpackControls && !player.disablingJetpackControls;
        }
    }
}

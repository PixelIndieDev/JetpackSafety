using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace JetpackSafety.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal static class PlayerControllerPatch
    {
        private static bool isFlying = false;
        private static float gracePeriodDuration = 2.5f;
        private static float graceTimer = 0f;

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

        [HarmonyPatch("DamagePlayer")]
        [HarmonyPrefix]
        [HarmonyPriority(Priority.Last)]
        static bool PreventDamage(PlayerControllerB __instance, CauseOfDeath causeOfDeath)
        {
            if (causeOfDeath == CauseOfDeath.Gravity && IsUsingJetpack(__instance)) {
                return false;
            }
            return true;
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        [HarmonyPriority(Priority.Last)]
        static void CheckIfFlying(PlayerControllerB __instance)
        {
            bool jetpackActive = IsJetpackActive(__instance);
            if (jetpackActive)
            {
                graceTimer = 0f;
                isFlying = true;
            }
            else if (isFlying) //just stopped flying
            {
                isFlying = false;
                graceTimer = gracePeriodDuration;
            }

            //tick timer down
            if (graceTimer > 0f)
            {
                graceTimer -= Time.deltaTime;
                if (graceTimer < 0f) graceTimer = 0f;
            }
        }

        private static bool IsJetpackActive(PlayerControllerB player)
        {
            return player.jetpackControls && !player.disablingJetpackControls;
        }
        private static bool IsUsingJetpack(PlayerControllerB player)
        {
            return isFlying || graceTimer > 0f;
        }
    }
}

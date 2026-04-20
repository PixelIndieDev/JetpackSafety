using HarmonyLib;
using UnityEngine;

namespace JetpackSafety.Patches
{
    [HarmonyPatch(typeof(JetpackItem))]
    internal static class JetpackPatch
    {
        [HarmonyPatch("ExplodeJetpackServerRpc")]
        [HarmonyPrefix]
        [HarmonyPriority(Priority.Last)] //priority above other mods
        static bool PreventExplosionsOnServer()
        {
            return false;
        }

        [HarmonyPatch("ExplodeJetpackClientRpc")]
        [HarmonyPrefix]
        [HarmonyPriority(Priority.Last)]
        static bool PreventExplosionsOnClient()
        {
            return false;
        }

        [HarmonyPatch("SetJetpackAudios")]
        [HarmonyPostfix]
        [HarmonyPriority(Priority.First)]
        static void PreventBeeps(JetpackItem __instance, ref bool ___jetpackPlayingWarningBeep, AudioSource ___jetpackBeepsAudio)
        {
            if (___jetpackPlayingWarningBeep)
            {
                ___jetpackPlayingWarningBeep = false;
                ___jetpackBeepsAudio.Stop();
            }
        }
    }
}

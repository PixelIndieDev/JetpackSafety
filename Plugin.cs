using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using JetpackSafety.Patches;

namespace JetpackSafety
{
    [BepInPlugin(ModInfo.modGUID, ModInfo.modName, ModInfo.modVersion)]
    public class JetpackPlayerDamagePatchBase : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(ModInfo.modGUID);
        private static JetpackPlayerDamagePatchBase instance;

        internal ManualLogSource logSource;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            logSource = BepInEx.Logging.Logger.CreateLogSource(ModInfo.modGUID);

            harmony.PatchAll(typeof(JetpackPlayerDamagePatchBase));
            harmony.PatchAll(typeof(JetpackPatch));
            harmony.PatchAll(typeof(PlayerControllerPatch));
            harmony.PatchAll(typeof(NetworkPatch));

            logSource.LogInfo(ModInfo.modName + " (version - " + ModInfo.modVersion + ")" + ": patches applied successfully");
        }
    }
}

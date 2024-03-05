using BepInEx;
using CardChoiceSpawnUniqueCardPatch;
using HarmonyLib;
using System;

namespace ModdingTools {
    [BepInDependency("com.willis.rounds.unbound")]
    [BepInPlugin("TODO", "ModdingTools", "1.0.0")]
    [BepInProcess("Rounds.exe")]
    public class Main:BaseUnityPlugin {
        internal static bool HasDrawN = false;
        void Awake() {
            new Harmony(Info.Metadata.GUID).PatchAll();
            HasDrawN = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("pykess.rounds.plugins.pickncards");
        }

        void Start() {
            Func<Player, CardInfo, bool> old = (Func<Player, CardInfo, bool>)CardChoicePatchSpawnUniqueCard.PlayerIsAllowedCard.Clone();
            CardChoicePatchSpawnUniqueCard.PlayerIsAllowedCard = (Player p, CardInfo c) => old(p, c) && CardUtilities.PlayerAllowedFunction(p, c);
        }
    }
}

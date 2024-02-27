using BepInEx;
using CardChoiceSpawnUniqueCardPatch;
using HarmonyLib;
using System;
using System.Reflection;
using Unbound.Core;
using UnityEngine;

namespace ModdingTools {
    [BepInDependency("com.willis.rounds.unbound")] 
    [BepInPlugin("TODO", "ModdingTools", "1.0.0")]
    [BepInProcess("Rounds.exe")]
    public class Main : BaseUnityPlugin {
        
        void Awake() {
            new Harmony(Info.Metadata.GUID).PatchAll();

        }

        void Start() {
            Func<Player, CardInfo, bool> old = (Func<Player, CardInfo, bool>)CardChoicePatchSpawnUniqueCard.PlayerIsAllowedCard.Clone();
            CardChoicePatchSpawnUniqueCard.PlayerIsAllowedCard = (Player p, CardInfo c) => old(p,c) && true;
        }
    }
}

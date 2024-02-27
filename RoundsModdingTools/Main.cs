using BepInEx;
using HarmonyLib;
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

        }
    }
}

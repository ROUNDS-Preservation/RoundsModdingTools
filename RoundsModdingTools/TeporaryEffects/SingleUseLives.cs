using HarmonyLib;
using System.Linq;
using Unbound.Core;
using UnityEngine;

namespace ModdingTools.TeporaryEffects {
    [HarmonyPatch]
    public class SingleUseLives : MonoBehaviour {
        [SerializeField] private int extraLives;
        public int ExtraLives { get => extraLives; private set { extraLives = value; if(extraLives == 0) Destroy(this); } }

        public SingleUseLives(int lives) { ExtraLives = lives; }

        public static int GetExtraTotalLives(Player player) {
            return player.GetComponentsInChildren<SingleUseLives>().Sum(l => l.ExtraLives);
        }

        [HarmonyPatch(typeof(HealthHandler), nameof(HealthHandler.Revive))]
        [HarmonyPostfix]
        static void AddLives(HealthHandler __instance, bool isFullRevive) {
            if(!isFullRevive) return;
            var templives = __instance.gameObject.GetComponentsInChildren<SingleUseLives>().ToList();
            __instance.GetFieldValue<CharacterData>("data").stats.remainingRespawns += GetExtraTotalLives(__instance.GetFieldValue<Player>("player"));
        }

        [HarmonyPatch(typeof(HealthHandler),"RPCA_Die_Phoenix")]
        [HarmonyPrefix]
        static void UseLife(HealthHandler __instance) {
            var data = __instance.GetFieldValue<CharacterData>("data");
            if(!data.isPlaying || data.dead || __instance.isRespawning || GetExtraTotalLives(data.player) < data.stats.remainingRespawns - 1) return;
            data.player.GetComponentInChildren<SingleUseLives>().ExtraLives--;  
        }
        

    }
}

using HarmonyLib;
using ModdingTools.Handler;
using UnityEngine;

namespace ModdingTools.Patches
{
    [HarmonyPatch(typeof(HealthHandler))]
    public class HealthHandlerPatch
    {
        [HarmonyPatch("DoDamage")]
        [HarmonyPrefix]
        [HarmonyBefore("com.aalund13.rounds.jarl")]
        public static void DoDamage(HealthHandler __instance, ref Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon, Player damagingPlayer, bool healthRemoval, bool lethal, bool ignoreBlock)
        {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            DeathHandler.PlayerTakeDamage(data.player, damagingPlayer);
        }

        [HarmonyPatch("RPCA_Die")]
        [HarmonyPrefix]
        public static void RPCA_Die(Player ___player, Vector2 deathDirection)
        {
            if (___player.data.dead) return;
            DeathHandler.PlayerDied(___player);
        }

        [HarmonyPatch("RPCA_Die_Phoenix")]
        [HarmonyPrefix]
        public static void RPCA_Die_Phoenix(HealthHandler __instance, Player ___player, Vector2 deathDirection)
        {
            if ((___player.data.dead || __instance.isRespawning)) return;
            DeathHandler.PlayerDied(___player);
        }
    }
}

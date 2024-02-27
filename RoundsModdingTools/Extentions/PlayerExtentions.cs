using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection;
using System.Text;
using Unbound.Core;

namespace ModdingTools.Extentions {
    public static class PlayerExtentions {


        public static void AddCard(this Player player, CardInfo cardInfo, bool reassign = false) =>
            CardUtilities.AddToPlayer(player, cardInfo, reassign: reassign);

        public static bool CanAddCard(this Player player, CardInfo cardInfo) => 
            CardChoiceSpawnUniqueCardPatch.CardChoicePatchSpawnUniqueCard.PlayerIsAllowedCard(player, cardInfo);

        public static List<CardInfo> Cards(this Player player) =>
            player.data.currentCards;

        public static Gun Gun(this Player player) => 
            player.data.weaponHandler.gun;

        public static GunAmmo Ammo(this Player player) =>
            player.data.weaponHandler.GetComponentInChildren<GunAmmo>();
        
        public static HealthHandler HealthHandler(this Player player) => 
            player.data.healthHandler;

        public static Gravity Gravity(this Player player) => 
            player.GetComponent<Gravity>();

        public static Block Block(this Player player) => 
            player.data.block;

        public static CharacterStatModifiers Stats(this Player player) => 
            player.data.stats;

        public static bool Alive(this Player player) => 
            PlayerUtilites.IsPlayerAlive(player);

        public static bool Simulated(this Player player) => 
            PlayerUtilites.IsPlayerSimulated(player);

        public static bool AliveAndSimulated(this Player player) =>
            PlayerUtilites.IsPlayerAliveAndSimulated(player);
        
        public static Player GetClosestPlayer(this Player player) =>
            PlayerUtilites.GetClosestPlayerToPlayer(player);
        
        public static Player GetClosestTeammate(this Player player) =>
             PlayerUtilites.GetClosestTeammateToPlayer(player);

        /// <summary>
        /// Kills the player, returning true if it was their last life
        /// </summary>
        public static bool Kill(this Player player, Vector2? deathDirection = null) =>
            PlayerUtilites.KillPlayer(player, deathDirection);
    }
}

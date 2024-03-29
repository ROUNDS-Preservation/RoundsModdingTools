﻿using System.Collections.Generic;
using UnityEngine;

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
            PlayerUtilities.IsPlayerAlive(player);

        public static bool Simulated(this Player player) =>
            PlayerUtilities.IsPlayerSimulated(player);

        public static bool AliveAndSimulated(this Player player) =>
            PlayerUtilities.IsPlayerAliveAndSimulated(player);

        public static Player GetClosestPlayer(this Player player) =>
            PlayerUtilities.GetClosestPlayerToPlayer(player);

        public static Player GetClosestTeammate(this Player player) =>
             PlayerUtilities.GetClosestTeammateToPlayer(player);

        /// <summary>
        /// Kills the player, returning true if it was their last life
        /// </summary>
        public static bool Kill(this Player player, Vector2? deathDirection = null) =>
            PlayerUtilities.KillPlayer(player, deathDirection);


        public static bool BlacklistCatagory(this Player player, CardCategory category) =>
             PlayerUtilities.BlacklistCatagoryFormPlayer(player, category);

        public static bool UnblacklistCatagory(this Player player, CardCategory category) =>
             PlayerUtilities.UnblacklistCatagoryFormPlayer(player, category);

        public static List<CardCategory> GetBlacklistedCatagories(this Player player) =>
            PlayerUtilities.GetPlayerBlacklistedCatagories(player);
    }
}

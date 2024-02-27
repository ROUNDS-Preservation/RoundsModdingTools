﻿using ModdingTools.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using Unbound.Core;
using Photon.Pun;

namespace ModdingTools {
    public static class PlayerUtilites {
        public static Player GetPlayer(int id){
            if(PlayerManager.instance.players.Any(p => p.playerID == id))
                return PlayerManager.instance.players.FirstOrDefault(p => p.playerID == id);
            return null;
        }

        public static bool IsPlayerAlive(Player player) {
            return !player.data.dead;
        }
        public static bool IsPlayerSimulated(Player player) {
            return player.data.playerVel.GetFieldValue<bool>("simulated");
        }
        public static bool IsPlayerAliveAndSimulated(Player player) {
            return IsPlayerAlive(player) && IsPlayerSimulated(player);
        }

        public static Player GetClosestPlayerToPlayer(Player player) {
            return PlayerManager.instance.players.Where(p => p.Alive() && p.playerID != player.playerID)
                .OrderBy(p => Vector2.Distance(player.transform.position, p.transform.position))
                .First();
        }
        public static Player GetClosestTeammateToPlayer(Player player) {
            return PlayerManager.instance.players.Where(p => p.Alive() && p.playerID == player.playerID && p.playerID != player.playerID)
                .OrderBy(p => Vector2.Distance(player.transform.position, p.transform.position))
                .First();
        }
        public static Player GetClosestOpponentToPlayer(Player player) {
            return PlayerManager.instance.players.Where(p => p.Alive() && p.teamID != player.teamID)
                .OrderBy(p => Vector2.Distance(player.transform.position, p.transform.position))
                .First();
        }
        /// <summary>
        /// Kills the player, returning true if it was their last life
        /// </summary>
        public static bool KillPlayer(Player player, Vector2? deathDirection = null) {
            if(deathDirection == null) deathDirection = Vector2.down;
            if(player.Stats().remainingRespawns > 0) {
                player.data.view.RPC("RPCA_Die_Phoenix", RpcTarget.All, deathDirection);
                return false;
            } else {
                player.data.view.RPC("RPCA_Die", RpcTarget.All, deathDirection);
                return true;
            }

        }
    }
}

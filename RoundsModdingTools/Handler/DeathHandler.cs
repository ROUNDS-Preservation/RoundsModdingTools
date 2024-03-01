using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unbound.Core.Networking;
using Photon.Pun;
using Unbound.Core;

namespace ModdingTools.Handler
{
    public class DeathHandler
    {
        public static Action<Player, Dictionary<Player, float>>? PlayerDiedEvent;

        // Dictionary to track damaging players and their last damage time for each hurt player
        private static Dictionary<Player, Dictionary<int, float>> _damageTrackingDict = new Dictionary<Player, Dictionary<int, float>>();

        internal static void PlayerTakeDamage(Player hurtPlayer, Player damagingPlayer) {
            if(damagingPlayer == null) return;

            if(!_damageTrackingDict.ContainsKey(hurtPlayer))
                _damageTrackingDict[hurtPlayer] = new Dictionary<int, float>();

            _damageTrackingDict[hurtPlayer][damagingPlayer.playerID] = Time.time;
        }

        private static void HandlePlayerDeath(Player deadPlayer, List<Player> damagingPlayers, List<float> lastDamageList) {
            try {
                Dictionary<Player, float> playerDamageDict = new Dictionary<Player, float>();
                for(int i = 0; i < lastDamageList.Count; i++) {
                    playerDamageDict.Add(damagingPlayers[i], lastDamageList[i]);
                }

                PlayerDiedEvent?.Invoke(deadPlayer, playerDamageDict);
            } catch(Exception e) {
                Debug.LogException(e);
            }
        }

        [UnboundRPC]
        private static void RPCA_PlayerDied(int deadPlayerID, int[] damagingPlayersID, float[] lastDamageList) {
            Player deadPlayer = PlayerUtilities.GetPlayer(deadPlayerID);
            List<Player> damagingPlayers = damagingPlayersID.Select(playerID => PlayerUtilities.GetPlayer(playerID)).ToList();

            HandlePlayerDeath(deadPlayer, damagingPlayers, lastDamageList.ToList());
        }
        
        internal static void PlayerDied(Player deadPlayer) {
            if(!_damageTrackingDict.ContainsKey(deadPlayer))
                _damageTrackingDict[deadPlayer] = new Dictionary<int, float>();

            // Retrieve damaging player IDs and calculate the time since last damage for the dead player
            List<int> damagingPlayersID = new List<int>(_damageTrackingDict[deadPlayer].Keys);
            List<float> lastDamageList = _damageTrackingDict[deadPlayer].Values.Select(lastDamage => Time.time - lastDamage).ToList();

            if(PhotonNetwork.OfflineMode) {
                RPCA_PlayerDied(deadPlayer.playerID, damagingPlayersID.ToArray(), lastDamageList.ToArray());
            } else if(PhotonNetwork.IsMasterClient) {
                NetworkingManager.RPC(typeof(DeathHandler), "RPCA_PlayerDied", deadPlayer.playerID, damagingPlayersID.ToArray(), lastDamageList.ToArray());
            }
        }
    }
}

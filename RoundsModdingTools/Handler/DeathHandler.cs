using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unbound.Core.Networking;
using Photon.Pun;
using Unbound.Core;

namespace ModdingTools.Handler {
    public class DeathHandler {
        public static List<Action<Player, Dictionary<Player, float>>> playerDiedActionDict = new List<Action<Player, Dictionary<Player, float>>>();

        internal static Dictionary<Player, Dictionary<int, float>> damagingPlayerList = new Dictionary<Player, Dictionary<int, float>>();

        internal static void PlayerTakeDamage(Player hurtPlayer, Player damagingPlayer) {
            if(damagingPlayer == null) return;
            if(!damagingPlayerList.ContainsKey(hurtPlayer))
                damagingPlayerList[hurtPlayer] = new Dictionary<int, float>();
            damagingPlayerList[hurtPlayer][damagingPlayer.playerID] = Time.time;
        }

        internal static void WhenPlayerDied(Player deadPlayer, List<Player> damagingPlayer, List<float> lastDamageList) {
            try {
                foreach(Action<Player, Dictionary<Player, float>> playerDiedAction in playerDiedActionDict) {
                    Dictionary<Player, float> playerDamageDict = new Dictionary<Player, float>();
                    for(int i = 0; i < lastDamageList.Count; i++) {
                        playerDamageDict.Add(damagingPlayer[i], lastDamageList[i]);
                    }

                    playerDiedAction.Invoke(deadPlayer, playerDamageDict);
                }
            } catch(Exception e) {
                Debug.LogException(e);
            }
        }

        [UnboundRPC]
        public static void RPCA_PlayerDied(int deadPlayerID, int[] damagingPlayersID, float[] lastDamageList) {
            Player deadPlayer = PlayerUtilities.GetPlayer(deadPlayerID);
            List<Player> damagingPlayers = damagingPlayersID.Select(playerID => PlayerUtilities.GetPlayer(playerID)).ToList();

            WhenPlayerDied(deadPlayer, damagingPlayers, lastDamageList.ToList());
        }

        internal static void PlayerDied(Player deadPlayer) {
            if(!damagingPlayerList.ContainsKey(deadPlayer))
                damagingPlayerList[deadPlayer] = new Dictionary<int, float>();

            if(PhotonNetwork.IsMasterClient || PhotonNetwork.OfflineMode) {
                List<int> damagingPlayersID = new List<int>(damagingPlayerList[deadPlayer].Keys);
                List<float> lastDamageList = damagingPlayerList[deadPlayer].Values.Select(lastDamage => Time.time - lastDamage).ToList();

                if(PhotonNetwork.OfflineMode) {
                    RPCA_PlayerDied(deadPlayer.playerID, damagingPlayersID.ToArray(), lastDamageList.ToArray());
                } else if(PhotonNetwork.IsMasterClient) {
                    NetworkingManager.RPC(typeof(DeathHandler), "RPCA_PlayerDied", deadPlayer.playerID, damagingPlayersID.ToArray(), lastDamageList.ToArray());
                }
            }
        }
    }
}

using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.Cards.Utils;
using UnboundLib.Networking;

namespace ModdingTools {
    public static class CardUtilities {
        internal static List<Func<string,CardInfo>> lookupFunctions = new List<Func<string,CardInfo>>();
        internal static Dictionary<CardInfo,bool> Reasonabilities = new Dictionary<CardInfo,bool>();

        public static void SetReasonability(CardInfo card, bool Reasonability = false) {
            Reasonabilities[card] = Reasonability;
        }

        public static bool AddLookupFunc(Func<string, CardInfo> function) {
            if (lookupFunctions.Contains(function)) return false;
            lookupFunctions.Add(function);
            return true;
        }

        public static void AddToPlayer(Player player, CardInfo? card = null, string lookupString = "", bool reasign = false) {
            if(player == null) return;
            if(card != null) {
                if(PhotonNetwork.OfflineMode) {
                    RPC_AddFromObject(player.playerID, card.name, reasign);
                } else if(PhotonNetwork.IsMasterClient) {
                    NetworkingManager.RPC(typeof(CardUtilities), "RPC_AddFromObject", player.playerID, card.name, reasign);
                }
            }else if(lookupString != "") {
                if(PhotonNetwork.OfflineMode) {
                    RPC_AddFromString(player.playerID, lookupString, reasign);
                } else if(PhotonNetwork.IsMasterClient) {
                    NetworkingManager.RPC(typeof(CardUtilities), "RPC_AddFromString", player.playerID, card.name, reasign);
                }
            }

        }

        [UnboundRPC]
        public static void RPC_AddFromObject(int playerID, string cardObject, bool reasign) {
            Player player = PlayerUtilites.GetPlayer(playerID);
            CardInfo card = CardManager.cards.Values.Any(c=> c.cardInfo.name == cardObject) 
                ? CardManager.cards.Values.First(c => c.cardInfo.name == cardObject).cardInfo
                : CardChoiceSpawnUniqueCardPatch.CardChoiceSpawnUniqueCardPatch.NullCard;
            ApplyCard(player, card, reasign);

        }

        [UnboundRPC]
        public static void RPC_AddFromString(int playerID, string lookupString, bool reasign) {
            Player player = PlayerUtilites.GetPlayer(playerID);
            CardInfo card;
            foreach(var func in lookupFunctions) {
                card = func(lookupString);
                if(card != null) goto found;
            }
            card = CardChoiceSpawnUniqueCardPatch.CardChoiceSpawnUniqueCardPatch.NullCard;
            found:
            ApplyCard(player, card, reasign);
        }

        internal static void ApplyCard(Player player, CardInfo card, bool reasign) {
            bool doAsign = !reasign || !Reasonabilities.ContainsKey(card) || Reasonabilities[card];
            CardBarHandler.instance.AddCard(player.playerID, card);
            card.gameObject.GetComponent<CardInfo>().sourceCard = card;
            if(doAsign) card.GetComponent<ApplyCardStats>().InvokeMethod("ApplyStats");
            if(card.GetComponent<CustomCard>() is CustomCard modCard) {
                if(doAsign) {
                    modCard.OnAddCard(player, player.data.weaponHandler.gun, player.data.weaponHandler.GetComponentInChildren<GunAmmo>(),
                        player.data, player.data.healthHandler, player.GetComponent<Gravity>(), player.data.block, player.data.stats);
                }
                if(reasign) {
                    modCard.OnReassignCard(player, player.data.weaponHandler.gun, player.data.weaponHandler.GetComponentInChildren<GunAmmo>(),
                        player.data, player.data.healthHandler, player.GetComponent<Gravity>(), player.data.block, player.data.stats);
                } 
            }
        }


        internal static CardInfo? FindFromObjectName(string name) {
            return CardManager.cards.Values.Any(c => c.cardInfo.name == name) 
                ? CardManager.cards.Values.First(c => c.cardInfo.name == name).cardInfo
                : null;
        }
    }
}

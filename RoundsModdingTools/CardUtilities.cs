using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.Cards.Utils;
using UnboundLib.Networking;

namespace ModdingTools {
    public static class CardUtilities {
        internal static List<Func<string,CardInfo>> LookupFunctions = new List<Func<string,CardInfo>>();
        internal static Dictionary<CardInfo,bool> Reassignabilities = new Dictionary<CardInfo,bool>();
        internal static Dictionary<string, CardInfo> HiddenCards = new Dictionary<string, CardInfo>();

        public static void SetReasonability(CardInfo card, bool reassignability = false) {
            Reassignabilities[card] = reassignability;
        }

        public static bool AddLookupFunc(Func<string, CardInfo> function) {
            if (LookupFunctions.Contains(function)) return false;
            LookupFunctions.Add(function);
            return true;
        }

        public static void AddToPlayer(Player player, CardInfo? card = null, string lookupString = "", bool reassign = false) {
            if(player == null) return;
            if(card != null) {
                if(PhotonNetwork.OfflineMode) {
                    RPC_AddFromObject(player.playerID, 
                        HiddenCards.ContainsValue(card) ? HiddenCards.Keys.First(key => HiddenCards[key] == card) : card.name,
                        reassign);
                } else if(PhotonNetwork.IsMasterClient) {
                    NetworkingManager.RPC(typeof(CardUtilities), "RPC_AddFromObject", player.playerID,
                        HiddenCards.ContainsValue(card) ? HiddenCards.Keys.First(key => HiddenCards[key] == card) : card.name,
                        reassign);
                }
            }else if(lookupString != "") {
                if(PhotonNetwork.OfflineMode) {
                    RPC_AddFromString(player.playerID, lookupString, reassign);
                } else if(PhotonNetwork.IsMasterClient) {
                    NetworkingManager.RPC(typeof(CardUtilities), "RPC_AddFromString", player.playerID, lookupString, reassign);
                }
            }

        }

        [UnboundRPC]
        public static void RPC_AddFromObject(int playerID, string cardObject, bool reassign) {
            Player player = PlayerUtilites.GetPlayer(playerID);
            CardInfo card = CardManager.cards.Values.Any(c=> c.cardInfo.name == cardObject) 
                ? CardManager.cards.Values.First(c => c.cardInfo.name == cardObject).cardInfo
                : HiddenCards.ContainsKey(cardObject)
                ? HiddenCards[cardObject]
                : CardChoiceSpawnUniqueCardPatch.CardChoiceSpawnUniqueCardPatch.NullCard;
            ApplyCard(player, card, reassign);

        }

        [UnboundRPC]
        public static void RPC_AddFromString(int playerID, string lookupString, bool reassign) {
            Player player = PlayerUtilites.GetPlayer(playerID);
            CardInfo card;
            foreach(var func in LookupFunctions) {
                card = func(lookupString);
                if(card != null) goto found;
            }
            card = CardChoiceSpawnUniqueCardPatch.CardChoiceSpawnUniqueCardPatch.NullCard;
            found:
            ApplyCard(player, card, reassign);
        }

        internal static void ApplyCard(Player player, CardInfo card, bool reassign) {
            bool doAssign = !reassign || !Reassignabilities.ContainsKey(card) || Reassignabilities[card];
            if(reassign)
                SilentAddCard(player.playerID, card);
            else
                CardBarHandler.instance.AddCard(player.playerID, card);
            card.gameObject.GetComponent<CardInfo>().sourceCard = card;
            if(doAssign) card.GetComponent<ApplyCardStats>().InvokeMethod("ApplyStats");
            if(card.GetComponent<CustomCard>() is CustomCard modCard) {
                if(doAssign) {
                    modCard.OnAddCard(player, player.data.weaponHandler.gun, player.data.weaponHandler.GetComponentInChildren<GunAmmo>(),
                        player.data, player.data.healthHandler, player.GetComponent<Gravity>(), player.data.block, player.data.stats);
                }
                if(reassign) {
                    modCard.OnReassignCard(player, player.data.weaponHandler.gun, player.data.weaponHandler.GetComponentInChildren<GunAmmo>(),
                        player.data, player.data.healthHandler, player.GetComponent<Gravity>(), player.data.block, player.data.stats);
                } 
            }
        }

        public static void AddHiddenCard(CardInfo card, string identifierOverride = "") {
            if(identifierOverride == "")
                HiddenCards.Add(card.name, card);
            else
                HiddenCards.Add(identifierOverride, card);
        }

        public static CardInfo? FindFromObjectName(string name) {
            return CardManager.cards.Values.Any(c => c.cardInfo.name == name)
                ? CardManager.cards.Values.First(c => c.cardInfo.name == name).cardInfo
                : HiddenCards.ContainsKey(name)
                ? HiddenCards[name]
                : null;
        }
        
        public static void SilentAddCard(int playerID, CardInfo card) {
            CardBar bar = ((CardBar[])CardBarHandler.instance.GetFieldValue("cardBars")).ElementAt(playerID);
            var temp = bar.soundCardPick;
            bar.soundCardPick = null;
            CardBarHandler.instance.AddCard(playerID, card);
            bar.soundCardPick = temp;
        }
    }
}

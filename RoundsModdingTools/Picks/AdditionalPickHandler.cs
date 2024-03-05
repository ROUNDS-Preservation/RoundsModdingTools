using static CardChoiceSpawnUniqueCardPatch.CardChoicePatchSpawnUniqueCard;
using static CardChoiceSpawnUniqueCardPatch.CardChoiceSpawnUniqueCardPatch;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unbound.Core;
using Unbound.Gamemodes;
using UnityEngine;

namespace ModdingTools.Picks {
    public static class AdditionalPickHandler {

        public static void PickFromPool(Player picker, bool validate = true) {

        }



        internal static IEnumerator DoFilteredPick(Player picker, List<CardInfo> cards) {
            Func<CardInfo,bool> ValidForPicker = (card) => PlayerIsAllowedCard(picker,card);
            CardChoiceVisuals.instance.Show(picker.playerID, true);
            CardChoice.instance.SetFieldValue("pickerType", PickerType.Player);
            CardChoice.instance.pickrID = picker.playerID;
            CardChoice.instance.IsPicking = true;
            CardChoice.instance.picks = 1;
            ArtHandler.instance.SetSpecificArt(CardChoice.instance.cardPickArt);
            var chilren = CardChoice.instance.GetFieldValue<Transform[]>("children");
            for(int j = 0; j < chilren.Length; j++) {
                var child = chilren[j];
                var pos = child.transform.position;
                var rot = child.transform.rotation;
                var validCards = cards.Where(CanCardSpawn).Where(ValidForPicker).ToArray();
                var card = DrawRandom(validCards);
                if(card == null) card = NullCard;
                GameObject gameObject = CardChoice.instance.InvokeMethod<GameObject>("Spawn", card.gameObject, pos, rot);
                gameObject.GetComponent<CardInfo>().sourceCard = card;
                gameObject.GetComponentInChildren<DamagableEvent>().GetComponent<Collider2D>().enabled = false;
                CardChoice.instance.GetFieldValue<List<GameObject>>("spawnedCards").Add(gameObject);
                yield return new WaitForSecondsRealtime(0.1f);
            }

            yield break;
        }



        internal static IEnumerator DoExtraPicks(Player picker, int pickcount) {
            CardChoiceVisuals.instance.Show(picker.playerID, true);
            CardChoice.instance.SetFieldValue("pickerType", PickerType.Player);
            CardChoice.instance.pickrID = picker.playerID;
            CardChoice.instance.IsPicking = true;
            CardChoice.instance.picks = pickcount;
            ArtHandler.instance.SetSpecificArt(CardChoice.instance.cardPickArt);
            CardChoice.instance.Pick();

            while(CardChoice.instance.IsPicking) {
                yield return null;
            }

            UIHandler.instance.StopShowPicker();
            CardChoiceVisuals.instance.Hide();
        }


    }
}

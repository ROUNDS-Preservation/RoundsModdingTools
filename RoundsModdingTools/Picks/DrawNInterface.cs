using static DrawNCards.DrawNCards;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ModdingTools.Picks {
    public static class DrawNInterface {

        internal static IEnumerator DoFilteredPickWithSize(Player picker, List<CardInfo> cards, int size, bool validate) {
            var oldSize = GetPickerDraws(picker.playerID);
            SetPickerDraws(picker.playerID, size);
            typeof(DrawNCards.DrawNCards).Assembly.GetType(typeof(DrawNCards.DrawNCards).Assembly.GetName().FullName+ "CardChoicePatchStartPick")
                .GetMethod("Prefix",BindingFlags.Static | BindingFlags.NonPublic).Invoke(null,new object[] { CardChoice.instance, picker.playerID });
            //That should work... I hope.
            //TODO: test once we have a working DrawNCards

            yield return AdditionalPickHandler.DoFilteredPick(picker, cards, validate);

            SetPickerDraws(picker.playerID, oldSize);
        }


        internal static IEnumerator DoExtraPicksWithSize(Player picker, int count,  int size) {
            var oldSize = GetPickerDraws(picker.playerID);
            SetPickerDraws(picker.playerID, size);

            yield return AdditionalPickHandler.DoExtraPicks(picker, count);

            SetPickerDraws(picker.playerID, oldSize);
        }

    }
}

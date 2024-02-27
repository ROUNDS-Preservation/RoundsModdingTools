using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModdingTools.Extentions {
    public static class CardExtentions {

        public static void AddToPlayer(this CardInfo cardInfo, Player player, bool reassign = false) =>
            CardUtilities.AddToPlayer(player, cardInfo, reassign: reassign);

        public static bool CanAddToPlayer(this CardInfo cardInfo, Player player) =>
            CardChoiceSpawnUniqueCardPatch.CardChoicePatchSpawnUniqueCard.PlayerIsAllowedCard(player,cardInfo);

        public static void AddAsHidden(this CardInfo cardInfo, string identifierOverride = "") =>
            CardUtilities.AddHiddenCard(cardInfo, identifierOverride);

        public static void CantBeReassigned(this CardInfo cardInfo) =>
            CardUtilities.SetReasonability(cardInfo);
    }
}

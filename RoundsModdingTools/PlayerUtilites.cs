using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModdingTools {
    public static class PlayerUtilites {
        public static Player GetPlayer(int id){
            if(PlayerManager.instance.players.Any(p => p.playerID == id))
                return PlayerManager.instance.players.FirstOrDefault(p => p.playerID == id);
            return null;
        }
    }
}

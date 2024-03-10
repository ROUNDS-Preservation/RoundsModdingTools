using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ModdingTools.TeporaryEffects {
    public class TemporaryStats : MonoBehaviour {

        public class BlockStats {
            public List<GameObject> ObjectsToSpawn = new List<GameObject>();
            public float CooldownMultiplierMultiplier = 1;
            public float CooldownMultiplierAdd = 0;
            public float CooldownAddMultiplier = 1;
            public float CooldownAddAdd = 0;
            public float ForceToAddMultiplier = 1;
            public float ForceToAddAdd = 0;
            public float ForceToAddUpMultiplier = 1;
            public float ForceUpAddUpAdd = 0;
            public int AdditionalBlocksMultiplier = 1;
            public int AdditionalBlocksAdd = 0;
            public float HeallingMultiplier = 1;
            public float HeallingAdd = 0;
        }

        public class GunStats {

        }

        public class PlayerStats {
            public float SizeMultiplier = 1;
            public float SizeAdd = 0;
            public float MaxHealthMultiplier = 1;
            public float MaxHealthAdd = 0;
            public bool adjustHealth = true;
            public float SpeedMultiplier = 1;
            public float SpeedAdd = 0;
            public float JumpHeightMultiplier = 1;
            public float JumpHeightAdd = 0;
            public float JumpCountMultiplier = 1;
            public float JumpCountAdd = 0;
            public float GravityMultiplier = 1;
            public float GravityAdd = 0;
            /* Not sure if i should actually expose this variable, it is kinda fecky
            public float GravityExponentMultiplier = 1;
            public float GravityExponentAdd = 0;
            */

        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenWorldDemo.LowPolyScene
{

    public class CharacterStatsData_SO : ScriptableObject
    {
        [Header("Stats Info")]
        public int MaxHealth;
        public int CurrentHealth;
        public int BaseDefense;
        public int CurrentDefense;

    }

}

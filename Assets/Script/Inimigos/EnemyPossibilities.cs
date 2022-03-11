using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCI.Enemies{
    [Serializable]
    public class EnemyPossibilities
    {
        [Range(0, 100)]
        public float possibility = 100;
        public EnemyActions action;
        public float delay = 0;

        public EnemyPossibilities(){
            action = EnemyActions.Nothing;
            possibility = 0;
        }
    }
}
using System;

namespace PCI.Enemies{
    [Serializable]
    public class EnemyTimerPossibilities : EnemyPossibilities{
        public float timer;
        public bool loop;
        public int times = 0;
    }
}


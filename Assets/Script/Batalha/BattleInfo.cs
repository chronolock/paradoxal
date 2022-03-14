using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCI.Enemies;

namespace PCI.Battle{
    public class BattleInfo : MonoBehaviour
    {
        public Vector3Int centerOfArena;

        public Vector2Int sizeOfArena;

        public List<Vector3Int> startCharacterPos;

        public List<Enemy> enemies;

        public List<Vector3Int> enemiesPos;

        public void startBattle(){
            if(!BattleManager.inBattle){
                BattleManager.StartBattle(centerOfArena, sizeOfArena, startCharacterPos, enemies, enemiesPos);
            }
        }
    }
}
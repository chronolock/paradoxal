using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCI.Battle;

namespace PCI.Enemies{
    public class Enemy : Character
    {
        
        public static List<Enemy> instance = new List<Enemy>();

        public List<EnemyPossibilities> befforeAttacked;
        public List<EnemyPossibilities> afterAttacked;
        public List<EnemyPossibilities> whenPlayerApproaches;
        public List<EnemyPossibilities> whenPlayerPairs;
        public List<EnemyPossibilities> whenPlayerMove;
        public List<EnemyTimerPossibilities> onTimer;

        public int maxHealth;
        public int currentHealth;

        public Vector2Int startPosition;

        public float distanceUp;

        private Vector2Int currentPos;

        
        void Start()
        {
            execOnStart();
            instance.Add(this);
            currentHealth = maxHealth;
        }

        void Update()
        {
            execOnUpdate();
        }

        public static Enemy getEnemyByID(string id){
            for(int i = 0; i < instance.Count; i++){
                if(instance[i].GetID() == id){
                    return instance[i];
                }
            }
            return null;
        }

        public void hurt(int damage){
            currentHealth -= damage;
        }

        public void WalkToUp(){

        }

        public void WalkToDown(){

        }

        public void WalkToLeft(){

        }

        public void WalktoRight(){

        }

        public void FaceUp(){
            faceDirection = 2;
        }
        public void FaceDown(){
            faceDirection = 0;
        }
        public void FaceLeft(){
            faceDirection = 3;
        }
        public void FaceRight(){
            faceDirection = 1;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCI.Battle;

namespace PCI.Enemies{
    public class Enemy : MonoBehaviour
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

        public StatusChar status;

        public float energySpeed;

        public Skill basicAttack;
        public Skill mainSkill;
        public Skill secondSkill;

        private SpriteRenderer spRender;

        private Vector2Int currentPos;

        private float energy = 0;

        private int energyLevel = 0;

        private string randId = ArenaID.MONSTER+"0";

        private int faceDirection = 0;

        // Start is called before the first frame update
        void Start()
        {
            randId = ArenaID.MONSTER+Random.Range(0, 9999999);
            spRender = GetComponent<SpriteRenderer>();
            instance.Add(this);
            currentHealth = maxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            //status.arrowCurrentChar.SetActive(false);

            if(energyLevel < 5){
                energy += Time.deltaTime*energySpeed;
            }
            if(energy >= 1){
                if(energyLevel < 5){
                    energy = 0;
                    energyLevel++;
                }
            }


            status.barraEnergia.transform.localScale = new Vector3(energy, 1, 1);

            status.barraVida.transform.localScale = new Vector3(currentHealth/maxHealth, 1, 1);

            switch(energyLevel){
                case 0:
                    status.iconeEnergia.sprite = Global.N0;
                break;
                case 1:
                    status.iconeEnergia.sprite = Global.N1;
                break;
                case 2:
                    status.iconeEnergia.sprite = Global.N2;
                break;
                case 3:
                    status.iconeEnergia.sprite = Global.N3;
                break;
                case 4:
                    status.iconeEnergia.sprite = Global.N4;
                break;
                case 5:
                    status.iconeEnergia.sprite = Global.N5;
                break;
            }
        }

        private void setNewPos(int x, int y){
            setNewPos(x, y, false);
        }

        private void setNewPos(int x, int y, bool ignoreEnergy){
            if(!BattleManager.inBattle){
                return;
            }

            BattleManager.getPosObject(randId);

            //if(BattleManager.registerOnPos(x, y, randId)){
                currentPos = new Vector2Int(x, y);
                if(!ignoreEnergy){
                    energyLevel--;
                    //Debug.Log("Energy: "+energyLevel);
                }
                
            //}

            spRender.sortingOrder = 50 + y;
                updatePos();
        }

        private void updatePos(){
            transform.position = new Vector2(transform.position.x, transform.position.y + distanceUp);
        }

        public static Enemy getEnemyByID(string id){
            for(int i = 0; i < instance.Count; i++){
                if(instance[i].randId == id){
                    return instance[i];
                }
            }
            return null;
        }

        public void hurt(int damage){
            currentHealth -= damage;
        }

        public string GetId(){
            return randId;
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
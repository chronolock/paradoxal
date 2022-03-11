using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityStandardAssets._2D;
using PCI.Enemies;

namespace PCI.Battle{
    public class BattleManager : MonoBehaviour
    {
        public Tilemap battleTilemap;

        public Tile arenaTile;

        public Color tileColorArena1;
        public Color tileColorArena2;
        public Color tileColorAreaSkill;
        public Color tileColorSelectedSkill;

        public Character player;

        private static BattleManager instance;

        private static List<Vector3Int> currentArena;

        private static List<Vector3Int> skillTileMarked;
        private static Vector3Int skillTileMarkedSelected;
        private static bool hasTargetSkill = false;

        private static bool hasSingleTargetSkill = false;

        public static bool inBattle = false;

        private float currentLerpTime = 0;

        private float maxLerpTime = 1;

        private bool inReverseLerp = true;

        private Arena objInGrid;

        private static int initPosX = 0;
        private static int initPosY = 0;
        private static int endPosX = 0;
        private static int endPosY = 0;

        void Awake(){
            instance = this;
            currentArena = new List<Vector3Int>();
            skillTileMarked = new List<Vector3Int>();
            objInGrid = new Arena();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(BattleManager.inBattle){

                if(hasSingleTargetSkill){
                    BattleManager.setTargetSkillTile(BattleManager.getTileByMousePos());
                }

                currentLerpTime += Time.deltaTime;

                Color lerpedColor = Color.Lerp(inReverseLerp ? tileColorArena1 : tileColorArena2, inReverseLerp ? tileColorArena2 : tileColorArena1, currentLerpTime/maxLerpTime);
                Color skillLerpedColor = Color.Lerp(inReverseLerp ? tileColorArena1 : tileColorAreaSkill, inReverseLerp ? tileColorAreaSkill : tileColorArena1, currentLerpTime/maxLerpTime);
                Color skillSelectedLerpedColor = Color.Lerp(inReverseLerp ? tileColorArena1 : tileColorSelectedSkill, inReverseLerp ? tileColorSelectedSkill : tileColorArena1, currentLerpTime/maxLerpTime);

                if(currentLerpTime > maxLerpTime){
                    currentLerpTime = 0;
                    inReverseLerp = !inReverseLerp;
                }

                for(int i = 0; i < currentArena.Count; i++){
                    instance.battleTilemap.SetColor(currentArena[i], lerpedColor);
                }

                for(int i = 0; i < skillTileMarked.Count; i++){
                    instance.battleTilemap.SetColor(skillTileMarked[i], skillLerpedColor);
                }

                if(hasTargetSkill){
                    instance.battleTilemap.SetColor(skillTileMarkedSelected, skillSelectedLerpedColor);
                }
                
            }
        }

        

        public static void RegisterOnPos(){

        }

        public static Vector2 MoveCharTo(Vector3Int position){
            Vector3Int oldPos = instance.objInGrid.GetPosByValue("P");
            Vector3Int newPos = new Vector3Int(oldPos.x + position.x, oldPos.y + position.y, 0);

            if(checkPosInArena(newPos)){

                if(instance.objInGrid.GetValue(newPos) == "0"){
                    instance.objInGrid.SetValue(oldPos, "0");
                    instance.objInGrid.SetValue(newPos, "P");
                    return instance.battleTilemap.GetCellCenterLocal(newPos);
                }
            }
            
            return instance.battleTilemap.GetCellCenterLocal(oldPos);
        }

        public static void StartBattle(Vector3Int centerOfArena, Vector2Int size, Vector3Int startCharPos, List<Enemy> inimigos, List<Vector3Int> inimigosPos){
            initPosX = centerOfArena.x - size.x / 2;
            initPosY = centerOfArena.y - size.y / 2;
            endPosX = centerOfArena.x + size.x / 2;
            endPosY = centerOfArena.y + size.y / 2;

            Camera.main.GetComponent<CameraFollow>().enabled = false;
            Camera.main.GetComponent<MoveObj>().moveTo(instance.battleTilemap.GetCellCenterLocal(centerOfArena) + new Vector3(0, 0, Camera.main.transform.position.z));

            for(int i = initPosX; i <= endPosX; i++){
                //instance.objInGrid.Add(new List<TilePos>());
                for(int j = initPosY; j <= endPosY; j++){
                    Vector3Int tilePos = new Vector3Int(i, j, 0);
                    currentArena.Add(tilePos);
                    instance.battleTilemap.SetTile(tilePos, instance.arenaTile);
                    instance.battleTilemap.SetTileFlags(tilePos, TileFlags.None);
                    //int lastRow = instance.objInGrid.Count - 1;
                    if((startCharPos.x == i) && (startCharPos.y == j)){
                        instance.objInGrid.SetValue(i, j, "P");
                    } else {
                        instance.objInGrid.SetValue(i, j, "0");
                    }

                    

                    for(int l = 0; l < inimigos.Count; l++){
                        if((inimigosPos[l].x == i) && (inimigosPos[l].y == j)){
                            instance.objInGrid.SetValue(i, j, inimigos[l].GetId());
                        }
                    }
                }
            }

            instance.player.moveToBattle(instance.battleTilemap.GetCellCenterLocal(startCharPos));

            inBattle = true;
        }

        public static void setSkillTile(Vector3Int pos){
            if(checkPosInArena(pos)){
                skillTileMarked.Add(pos);
            }
        }

        public static void setTargetSkillTile(Vector3Int pos){
            if(checkPosInTargetSkill(pos)){
                hasTargetSkill = true;
                skillTileMarkedSelected = pos;
            } else {
                skillTileMarkedSelected = new Vector3Int(int.MinValue, int.MinValue, 0);
            }
        }

        public static Vector3Int getSingleTargetSkill(){
            return skillTileMarkedSelected;
        }

        public static bool hasSingleTargetSet(){
            if(skillTileMarkedSelected.x == int.MinValue && skillTileMarkedSelected.y == int.MinValue){
                return false;
            }
            return true;
        }

        public static void setSingleTargetSkillTile(){
            hasSingleTargetSkill = true;
        }

        public static void clearSkillTile(){
            skillTileMarked = new List<Vector3Int>();
            hasTargetSkill = false;
            hasSingleTargetSkill = false;
        }

        public static void clearTargetSkillTile(){
            hasTargetSkill = false;
            hasSingleTargetSkill = false;
            skillTileMarkedSelected = new Vector3Int(int.MinValue, int.MinValue, 0);
        }

        public static Vector3 getTilePos(Vector3Int pos){
            return  instance.battleTilemap.GetCellCenterLocal(pos);
        }

        private static bool checkPosInArena(Vector3Int pos){
            if((pos.x >= initPosX) && (pos.y >= initPosY) && (pos.x <= endPosX) && (pos.y <= endPosY)){
                return true;
            }

            return false;
        }

        private static bool checkPosInTargetSkill(Vector3Int pos){
            for(int i = 0; i < skillTileMarked.Count; i++){
                if(skillTileMarked[i] == pos){
                    return true;
                }
            }

            return false;
        }

        public static Vector3Int getPosObject(string id){
            return instance.objInGrid.GetPosByValue(id);

        }

        public static Vector3Int getTileByMousePos(){
            return instance.battleTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        }
    }
}




class TilePos{
    public int x;
    public int y;
    public float value;

    public TilePos(int x, int y, float value){
        this.x = x;
        this.y = y;
        this.value = value;
    }

    public Vector3Int getPos(){
        return new Vector3Int(x, y, 0);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public int power;
    public TypeSkill typeSkill;
    public int energyCost; 

    public bool singleTarget;

    public TargetAnimation target;

    public int range;

    public Vector3 ajusteAnimacao;

    private SpriteRenderer spRender;

    private Animator animator;

    private Vector3Int originSkill;

    private bool inPlay = false;

    void Awake()
    {
        spRender = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        spRender.sortingOrder = 50;
    }

    // Update is called once per frame
    void Update()
    {
        if(inPlay){
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
                endSkill();
            }
        }

        if(!BattleManager.inBattle){
            return;
        }
    }

    public void clearSkillArea(){
        if(!BattleManager.inBattle){
            return;
        }

        BattleManager.clearSkillTile();
    }

    public void prepareSkill(Vector3Int origin, int quadrant){


        if(!BattleManager.inBattle){
            return;
        }

        originSkill = origin;

        if(typeSkill == TypeSkill.Rect){
            for(int i = origin.x - range; i <= origin.x + range; i++){
                for(int j = origin.y - range; j <= origin.y + range; j++){
                    BattleManager.setSkillTile(new Vector3Int(i, j, 0));
                }
            }
        }

        if(typeSkill == TypeSkill.Melee){
            BattleManager.setSkillTile(new Vector3Int(origin.x + 1, origin.y, 0));
            BattleManager.setSkillTile(new Vector3Int(origin.x - 1, origin.y, 0));
            BattleManager.setSkillTile(new Vector3Int(origin.x, origin.y - 1, 0));
            BattleManager.setSkillTile(new Vector3Int(origin.x, origin.y + 1, 0));
        }

        if(typeSkill == TypeSkill.InLine){
            Vector3Int nextPosSkill =  getNextTileInQuadrant(origin, quadrant);
            for(int i = 0; i < range; i++){
                BattleManager.setSkillTile(nextPosSkill);
                nextPosSkill = getNextTileInQuadrant(nextPosSkill, quadrant);
            }
        }

        if(typeSkill == TypeSkill.Self){
            BattleManager.setSkillTile(origin);
        }

        if(singleTarget){
            BattleManager.setSingleTargetSkillTile();
        }
    }

    private Vector3Int getNextTileInQuadrant(Vector3Int pos, int quadrant){
        switch(quadrant){
            case 0:
                return pos + new Vector3Int(0, -1, 0);
            case 1: 
                return pos + new Vector3Int(1, -1, 0);
            case 2: 
                return pos + new Vector3Int(1, 0, 0);
            case 3: 
                return pos + new Vector3Int(1, 1, 0);
            case 4: 
                return pos + new Vector3Int(0, 1, 0);
            case 5: 
                return pos + new Vector3Int(-1, 1, 0);
            case 6: 
                return pos + new Vector3Int(-1, 0, 0);
            case 7: 
                return pos + new Vector3Int(-1, -1, 0);
        }
        return Vector3Int.zero;
    }

    public bool execSkill(){
        if(!BattleManager.hasSingleTargetSet() && singleTarget){
            return false;
        }
        
        Skill tmpClone = Instantiate(this);

        if(singleTarget){
            if(target == TargetAnimation.Target){
                tmpClone.transform.position = BattleManager.getTilePos(BattleManager.getSingleTargetSkill());
            }
        } else {
            tmpClone.transform.position = BattleManager.getTilePos(originSkill);
        }

        tmpClone.transform.position = tmpClone.transform.position + ajusteAnimacao;

        tmpClone.play();

        return true;
    }


    public void play(){
        spRender.enabled = true;
        animator.SetTrigger("play");
        inPlay = true;
        BattleManager.clearTargetSkillTile();
    }

    public void endSkill(){
        Destroy(gameObject);
    }
}

public enum TypeSkill
{
    Melee,
    InLine,
    Circle,
    Rect,
    Self
}

public enum TargetAnimation{
    Origin,
    Target
}
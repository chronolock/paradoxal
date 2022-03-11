using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using PCI.Battle;

public class Player : MonoBehaviour
{
    public float walkSpeed;
    
    public float energySpeed;

    public Vector2 ajustPos;

    public StatusChar status;

    private Animator anima;
    private Rigidbody2D rbody;
    private SpriteRenderer sRender;

    private int faceDirection;

    private Vector2 moveTo;
    private Vector3Int moveToBattlePos;

    private bool stopped;

    private bool movingToBattle = false;

    private float countTimeToWalk = 0;

    private float maxTimeToWalk = 1;

    private Vector2 startPos = Vector2.zero;

    private float energy = 0;

    private int energyLevel = 0;

    public bool currentChar = false;

    public Skill mainSkill;
    public Skill secondSkill;
    public Skill basicSkill;

    private bool inMainAttackPrepare = false;
    private bool inBasicAttackPrepare = false;
    private bool movedInPrepare = false;

    private Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        anima = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        sRender = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if(transform.parent){
            mousePos = transform.parent.InverseTransformPoint(Mouse.current.position.ReadValue());
        } else {
            mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }

        anima.SetBool("stopped", stopped);        
        anima.SetInteger("faceDirection", faceDirection);
        anima.SetBool("inBattle", BattleManager.inBattle);

        if(BattleManager.inBattle){
            if(energyLevel < 5){
                energy += Time.deltaTime*energySpeed;
            }
            if(energy >= 1){
                if(energyLevel < 5){
                    energy = 0;
                    energyLevel++;
                }
            }
            status.barraEnergia.transform.localScale = new Vector3(energyLevel == 5 ? 1 : energy, 1, 1);
            status.enableStatusBar(true);

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

        } else {
            energy = 0;
            energyLevel = 0;
            status.enableStatusBar(false);
        }


        updateMainSkill();
        updateBasicSkill();
    }

    public void prepareBasicSkill(InputAction.CallbackContext context){
        //Debug.Log(context.started+ " - "+context.performed+" - "+context.canceled);

        if(!BattleManager.inBattle){
            return;
        }

        if(context.started){
            inBasicAttackPrepare = true;
            movedInPrepare = false;
            basicSkill.prepareSkill(BattleManager.getPosObject("P"), getQuadrante(mousePos));
             
        }
        if(context.canceled){
            inBasicAttackPrepare = false;
            if(basicSkill.energyCost <= energyLevel){
                if(basicSkill.execSkill()){
                    energyLevel -= basicSkill.energyCost;
                }
            }
            basicSkill.clearSkillArea();
        }
    }

    private void updateBasicSkill(){
        if(!inBasicAttackPrepare){
            //basicSkill.clearSkillArea();
            return;
        }
        if(movedInPrepare || mainSkill.typeSkill == TypeSkill.InLine){
            basicSkill.clearSkillArea();
            movedInPrepare = false;
            basicSkill.prepareSkill(BattleManager.getPosObject("P"), getQuadrante(mousePos));
        }
    }

    public void prepareMainSkill(InputAction.CallbackContext context){
        if(!BattleManager.inBattle){
            return;
        }

        if(context.started){
            inMainAttackPrepare = true;
            movedInPrepare = false;
            mainSkill.prepareSkill(BattleManager.getPosObject("P"), getQuadrante(mousePos));
        }
        
        if(context.canceled){
            inMainAttackPrepare = false;
            if(mainSkill.energyCost <= energyLevel){
                if(mainSkill.execSkill()){
                    energyLevel -= mainSkill.energyCost;
                }
            }
            mainSkill.clearSkillArea();
        }

    }

    private void updateMainSkill(){
        if(!inMainAttackPrepare){
            //mainSkill.clearSkillArea();
            return;
        }
        if(movedInPrepare || mainSkill.typeSkill == TypeSkill.InLine){
            mainSkill.clearSkillArea();
            movedInPrepare = false;
            mainSkill.prepareSkill(BattleManager.getPosObject("P"), getQuadrante(mousePos));
        }
    }

    void FixedUpdate() {
        rbody.velocity = Vector2.zero;

        if(moveTo != Vector2.zero){
            stopped = false;
            if(BattleManager.inBattle){
                if(movingToBattle){
                    faceDirection = 1;
                    countTimeToWalk += Time.deltaTime;
                    rbody.MovePosition( new Vector2(Mathf.Lerp(startPos.x, moveTo.x, countTimeToWalk/maxTimeToWalk), Mathf.Lerp(startPos.y, moveTo.y, countTimeToWalk/maxTimeToWalk)));
                    if(countTimeToWalk >= maxTimeToWalk){
                        countTimeToWalk = 0;
                        moveTo = Vector2.zero;
                        movingToBattle = false;
                    }
                } else {
                    rbody.MovePosition(BattleManager.MoveCharTo(moveToBattlePos) + ajustPos);
                    moveTo = Vector2.zero;
                    movedInPrepare = true;
                }
            } else {
                rbody.MovePosition(rbody.position + moveTo.normalized * Time.fixedDeltaTime * walkSpeed);
            }
        } else {
            stopped = true;
        }
    }

    public void moveToBattle(Vector2 pos){
        pos += ajustPos;
        movingToBattle = true;
        startPos = rbody.position;
        moveTo = pos;
    }

    public void moveChar(InputAction.CallbackContext context){

        Vector2 move = context.ReadValue<Vector2>();

        if(movingToBattle){
            return;
        }

        if(move != Vector2.zero){
            if((move.x != 0) && (move.y != 0)){
                if(((move.x > 0) && (faceDirection != 1)) && ((move.y > 0) && (faceDirection != 2))){
                    faceDirection = 2;
                    moveToBattlePos = new Vector3Int(1, 1, 0);
                } else {
                    if(((move.x > 0) && (faceDirection != 1)) && ((move.y < 0) && (faceDirection != 0))){
                        faceDirection = 0;
                        moveToBattlePos = new Vector3Int(1, -1, 0);
                    } else {
                        if(((move.x < 0) && (faceDirection != 3)) && ((move.y < 0) && (faceDirection != 0))){
                            faceDirection = 0;
                            moveToBattlePos = new Vector3Int(-1, -1, 0);
                        } else {
                            if(((move.x < 0) && (faceDirection != 3)) && ((move.y > 0) && (faceDirection != 2))){
                                faceDirection = 2;
                                moveToBattlePos = new Vector3Int(-1, 1, 0);
                            }
                        }
                    }
                }
            } else {
                if(move.x > 0){
                    faceDirection = 1;
                    moveToBattlePos = new Vector3Int(1, 0, 0);
                } else {
                    if(move.x < 0){
                        faceDirection = 3;
                        moveToBattlePos = new Vector3Int(-1, 0, 0);
                    } else {
                        if(move.y > 0){
                            faceDirection = 2;
                            moveToBattlePos = new Vector3Int(0, 1, 0);
                        } else {
                            if(move.y < 0){
                                faceDirection = 0;
                                moveToBattlePos = new Vector3Int(0, -1, 0);
                            }
                        }
                    }
                }
            }
        }
        
        moveTo = move;
    }

    public void onMouseClick(InputAction.CallbackContext context){
        //Debug.Log(context+" | "+context.ReadValueAsButton());
        //Debug.Break();
    }


    private int getQuadrante(Vector2 pos){
        return getQuadrante(pos.x, pos.y);
    }

    private int getQuadrante(float x, float y){
        
        Vector2 vAngle = new Vector2(transform.position.x, transform.position.y) - new Vector2(x, y);
        float angle = Mathf.Atan2(vAngle.y, vAngle.x) * Mathf.Rad2Deg;

        if((angle > 0) && (angle <= 22.5)){
            return 6;
        } else {
            if((angle > 22.5) && (angle <= 67.5)){
                return 7;
            } else {
                if((angle > 67.5) && (angle <= 112.5)){
                    return 0;
                } else {
                    if((angle > 112.5) && (angle <= 157.5)){
                        return 1;
                    } else {
                        if((angle > 157.5) && (angle <= 180)){
                            return 2;
                        } else {
                            if((angle > -22.5) && (angle <= 0)){
                                return 6;
                            } else {
                                if((angle > -67.5) && (angle <= -22.5)){
                                    return 5;
                                } else {
                                    if((angle > -112.5) && (angle <= -67.5)){
                                        return 4;
                                    } else {
                                        if((angle > -157.5) && (angle <= -112.5)){
                                            return 3;
                                        } else {
                                            return 2;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;
using PCI.Battle;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    public float energySpeed;
    public Vector2 ajustSpritePosition;
    public float walkSpeed;
    public int walkCostInBattle;
    public StatusChar status;

    private Animator anima;
    private Rigidbody2D rbody;
    private SpriteRenderer sRender;

    protected int faceDirection;

    protected Vector2 moveTo;
    protected Vector3Int moveToInBattle;

    private bool movingToBattle = false;

    private float countTimeToWalk = 0;

    private float maxTimeToWalk = 1;

    private Vector3Int currentBattlePos = Vector3Int.zero;
    private Vector2 startTilePos = Vector2.zero;
    private Vector2 startCharPos = Vector2.zero;

    private float energy = 0;
    private int energyLevel = 0;
    private bool stopped = true;

    public Skill mainSkill;
    public Skill secondSkill;
    public Skill basicSkill;

    protected string prefixID = "";
    private string randId = "";

    private bool controlKeyDown = false;
    private Vector2 controlValue = Vector2.zero;

    public UnityEvent beforeMove;
    public UnityEvent afterMove;
    public UnityEvent beforeMoveInBattle;
    public UnityEvent afterMoveInBattle;
    public UnityEvent beforeAttackMainSkill;
    public UnityEvent afterAttackMainSkill;
    public UnityEvent preparingMainSkill;
    public UnityEvent startPreparingMainSkill;
    public UnityEvent beforeAttackSecondSkill;
    public UnityEvent afterAttackSecondSkill;
    public UnityEvent preparingSecondSkill;
    public UnityEvent startPreparingSecondSkill;
    public UnityEvent beforeAttackBasicSkill;
    public UnityEvent afterAttackBasicSkill;
    public UnityEvent preparingBasicSkill;
    public UnityEvent startPreparingBasicSkill;


    void Start()
    {
        execOnStart();
    }

    void Update()
    {
        execOnUpdate();
    }

    void FixedUpdate() {
        execOnFixedUpdate();
    }

    public string GetID(){
        if(randId == ""){
            randId = prefixID + Random.Range(0, 9999999);
        }
        return randId;
    }

    protected void execOnStart(){
        anima = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        sRender = GetComponent<SpriteRenderer>();

        rbody.gravityScale = 0;
    }

    protected void execOnUpdate(){
        if(controlKeyDown){
            updateMoveVector();
        }

        anima.SetInteger("faceDirection", faceDirection);
        anima.SetBool("inBattle", BattleManager.inBattle);
        anima.SetBool("stopped", stopped);

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
    }

    protected void execOnFixedUpdate(){
        rbody.velocity = Vector2.zero;

        if(BattleManager.inBattle){
            
            if(movingToBattle){
                stopped = false;
                faceDirection = Quadrant.GetQuadrantByAngle(transform.position, startTilePos);
                countTimeToWalk += Time.deltaTime;
                rbody.MovePosition( Vector2.Lerp(startCharPos, startTilePos, countTimeToWalk/maxTimeToWalk) );

                if(countTimeToWalk >= maxTimeToWalk){
                    countTimeToWalk = 0;
                    movingToBattle = false;
                }

            } else {
                stopped = true;
                if(moveToInBattle != Vector3Int.zero){
                    rbody.MovePosition(BattleManager.MoveCharTo(GetID(), moveToInBattle) + ajustSpritePosition);
                    afterMoveInBattle.Invoke();
                }
            }
        } else {
            if(moveTo == Vector2.zero){
                stopped = true;
            } else {
                stopped = false;
                rbody.MovePosition(rbody.position + moveTo.normalized * Time.fixedDeltaTime * walkSpeed);
                afterMove.Invoke();
            }
        }

        moveTo = Vector2.zero;
        moveToInBattle = Vector3Int.zero;
    }

    public void EnterInBattle(Vector3Int startPos){
        movingToBattle = true;
        currentBattlePos = startPos;
        startTilePos = BattleManager.getTilePos(currentBattlePos);
        startCharPos = transform.position;
    }

    public void MoveInBattle(Vector2Int direction){
        MoveInBattle(((Vector3Int)direction));
    }

    public void MoveInBattle(Vector3Int direction){
        MoveInBattle(direction, 1 , false);
    }

    public void MoveInBattle(Vector3Int direction, bool freeMove){
        MoveInBattle(direction, 1, freeMove);
    }

    public void MoveInBattle(Vector3Int direction, int steps, bool freeMove){
        if(!BattleManager.inBattle){
            return;
        }

        if(BattleManager.CanMoveTo(GetID(), direction)){
            beforeMoveInBattle.Invoke();
            if(freeMove){
                FreeMoveInBattle(direction * steps);
            } else {
                if(energyLevel >= walkCostInBattle){
                    energyLevel -= walkCostInBattle;
                    FreeMoveInBattle(direction * steps);
                }
            }
        }
    }

    public void MoveInBattle(int quadrant){
        MoveInBattle(quadrant, 1, false);
    }

    public void MoveInBattle(int quadrant, bool freeMove){
        MoveInBattle(quadrant, 1, freeMove);
    }

    public void MoveInBattle(int quadrant, int steps, bool freeMove){
        MoveInBattle(Quadrant.GetVectorByQuadrant(quadrant), steps, freeMove);
        
    }

    private void FreeMoveInBattle(Vector3Int direction){
        moveToInBattle = direction;
        changeFaceDirection(((Vector2Int) moveToInBattle));
    }

    public void Move(int quadrant){
        if(BattleManager.inBattle){
            return;
        }

        Vector3Int direction = Quadrant.GetVectorByQuadrant(quadrant);
        Move(((Vector2Int)direction));
    }

    public void Move(Vector2 direction){
        if(BattleManager.inBattle){
            return;
        }

        changeFaceDirection(direction);
        moveTo = direction;
        beforeMove.Invoke();
    }

    public void changeFaceDirection(Vector2 direction){
        if(direction != Vector2.zero){
            if((direction.x != 0) && (direction.y != 0)){
                if(((direction.x > 0) && (faceDirection != 1)) && ((direction.y > 0) && (faceDirection != 2))){
                    faceDirection = 2;
                } else {
                    if(((direction.x > 0) && (faceDirection != 1)) && ((direction.y < 0) && (faceDirection != 0))){
                        faceDirection = 0;
                    } else {
                        if(((direction.x < 0) && (faceDirection != 3)) && ((direction.y < 0) && (faceDirection != 0))){
                            faceDirection = 0;
                        } else {
                            if(((direction.x < 0) && (faceDirection != 3)) && ((direction.y > 0) && (faceDirection != 2))){
                                faceDirection = 2;
                            }
                        }
                    }
                }
            } else {
                if(direction.x > 0){
                    faceDirection = 1;
                } else {
                    if(direction.x < 0){
                        faceDirection = 3;
                    } else {
                        if(direction.y > 0){
                            faceDirection = 2;
                        } else {
                            if(direction.y < 0){
                                faceDirection = 0;
                            }
                        }
                    }
                }
            }
        }
    }

    protected void prepareSkill(Skill skill){
        if(!BattleManager.inBattle){
            return;
        }

        dispachSkillEvent(skill, startPreparingMainSkill, startPreparingSecondSkill, startPreparingBasicSkill);

        skill.prepareSkill(BattleManager.getPosObject(GetID()), Quadrant.GetQuadrantByAngle(transform.position, GetMousePos()));
    }

    protected void updateSkill(Skill skill){
        if(!skill.preparingSkill){
            return;
        }

        dispachSkillEvent(skill, preparingMainSkill, preparingSecondSkill, preparingBasicSkill);

        Vector3Int currentPos = BattleManager.getPosObject(GetID());

        if((currentPos != skill.lastOriginPoint) || skill.typeSkill == TypeSkill.InLine){
            skill.clearSkillArea();
            skill.prepareSkill(currentPos, Quadrant.GetQuadrantByAngle(transform.position, GetMousePos()));
        }
    }

    protected void execSkill(Skill skill){
        if(skill.energyCost <= energyLevel){
            dispachSkillEvent(skill, beforeAttackMainSkill, beforeAttackSecondSkill, beforeAttackBasicSkill);
            if(skill.execSkill()){
                energyLevel -= skill.energyCost;
                dispachSkillEvent(skill, afterAttackMainSkill, afterAttackSecondSkill, afterAttackBasicSkill);
            }
        } else {
            skill.cancelSkill();
        }
    }

    private Vector2 GetMousePos(){
        if(transform.parent){
            return transform.parent.InverseTransformPoint(Mouse.current.position.ReadValue());
        } else {
            return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }
    }

    public void MoveByKeyboard(InputAction.CallbackContext context){
        if(movingToBattle){
            return;
        }

        if(context.started){
            controlKeyDown = true;
        }

        if(context.canceled){
            controlKeyDown = false;
            return;
        }

        controlValue = context.ReadValue<Vector2>();
    }

    protected void updateMoveVector()
    {
        if(BattleManager.inBattle){
            Vector2Int bMove = new Vector2Int(controlValue.x > 0 ? 1 : controlValue.x < 0 ? -1 : 0, controlValue.y > 0 ? 1 : controlValue.y < 0 ? -1 : 0);
            MoveInBattle(bMove);
        } else {
            Move(controlValue);
        }
    }

    protected void dispachSkillEvent(Skill skill, UnityEvent mainEvent, UnityEvent secondEvent, UnityEvent basicEvent){
        if(skill == mainSkill){
            mainEvent.Invoke();
        } else {
            if(skill == secondSkill){
                secondEvent.Invoke();
            } else {
                if(skill == basicSkill){
                    basicEvent.Invoke();
                }
            }
        }
    }
}
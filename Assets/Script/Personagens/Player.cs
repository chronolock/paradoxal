using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using PCI.Battle;

public class Player : Character
{    
    public bool currentChar = false;

    public int groupPos = 0;

    private string typeSkillInUse = "";

    private bool useSecondSkill = false;

    void Start()
    {
        execOnStart();
    }

    void Update()
    {
        
        execOnUpdate();

        if(BattleManager.inBattle){
            switch(typeSkillInUse){
                case "Basic":
                    updateSkill(basicSkill);
                break;
                case "Main":
                    updateSkill(mainSkill);
                break;
                case "Second":
                    updateSkill(secondSkill);
                break;
            }
        }
    }

    public void BasicSkillByCommand(InputAction.CallbackContext context){
        if(!BattleManager.inBattle){
            return;
        }

        if(context.started){
            prepareSkill(basicSkill);
             typeSkillInUse = "Basic";
        }

        if(context.canceled){
            execSkill(basicSkill);
        }
    }

    public void MainSkillByCommand(InputAction.CallbackContext context){
        if(!BattleManager.inBattle){
            return;
        }

        if(context.started){
            if(useSecondSkill){
                prepareSkill(secondSkill);
                typeSkillInUse = "Second";
            } else {
                prepareSkill(mainSkill);
                typeSkillInUse = "Main";
            }
        }

        if(context.canceled){
            if(useSecondSkill){
                execSkill(secondSkill);
            } else {
                execSkill(mainSkill);
            }
        }
    }

    public void SecondSkillTrigger(InputAction.CallbackContext context){
        if(context.started){
            useSecondSkill = true;
            if(mainSkill.preparingSkill){
                mainSkill.cancelSkill();
                prepareSkill(secondSkill);
                typeSkillInUse = "Second";
            }
        }

        if(context.canceled){
            useSecondSkill = false;
            if(secondSkill.preparingSkill){
                secondSkill.cancelSkill();
                prepareSkill(mainSkill);
                typeSkillInUse = "Main";
            }
        }
    }

    void FixedUpdate() {
        execOnFixedUpdate();
    }
}

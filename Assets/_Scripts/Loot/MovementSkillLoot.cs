using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TE
{
    public class MovementSkillLoot : Loot
    {
        public enum MovementSkillTypes
        {
            DASH,
            JUMP
        }


        public MovementSkillTypes skillType;
        public string info;

        public override void CustomBehavior()
        {
            Debug.Log("Picked up a " + skillType + "!");
            Session session = Game.instance.session;
            switch (skillType)
            {
                case MovementSkillTypes.DASH:
                    Game.instance.ChangeInfoTextSprite("XboxOne_RT");
                    info = session.IsDashUnlocked() ? "Du hast den Doppel-DASH freigeschaltet!" :
                        "Du kannst nun mit <sprite name=\"XboxOne_RT\"> dashen.";
                    session.CollectedDashLoot();
                    break;
                case MovementSkillTypes.JUMP:
                    info = "Du hast den Doppelsprung freigeschaltet!";
                    session.CollectedJumpLoot();
                    break;
                default:
                    break;
            }

            
            Game.instance.ShowTextBox(info);
        }
    }
}
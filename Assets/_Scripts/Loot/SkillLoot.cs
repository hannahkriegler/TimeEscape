using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TE
{
    public class SkillLoot : Loot
    {
        public enum SkillType
        {
            DASH,
            JUMP,
            FIREBALL
        }


        public SkillType skillType;
        public string info;

        public override void CustomBehavior()
        {
            Debug.Log("Picked up a " + skillType + "!");
            Session session = Game.instance.session;
            switch (skillType)
            {
                case SkillType.DASH:
                    Game.instance.ChangeInfoTextSprite("XboxOne_RT");
                    info = session.IsDashUnlocked() ? "Du hast den Doppel-DASH freigeschaltet!" :
                        "Du kannst nun mit <sprite name=\"XboxOne_RT\"> dashen.";
                    session.CollectedDashLoot();
                    break;
                case SkillType.JUMP:
                    info = "Du hast den Doppelsprung freigeschaltet!";
                    session.CollectedJumpLoot();
                    break;
                case SkillType.FIREBALL:
                    Game.instance.ChangeInfoTextSprite("XboxOne_LT");
                    info = "Du hast den Feuerball freigeschaltet!\n" +
                        "Schieße einen Feuerball mit <sprite name=\"XboxOne_LT\">!";
                    session.UnlockFireball();
                    break;
                default:
                    break;
            }

            
            Game.instance.ShowTextBox(info);
        }
    }
}
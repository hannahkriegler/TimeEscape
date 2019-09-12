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
                    info = session.IsDashUnlocked() ? "Du hast den Doppel-DASH freigeschaltet!" : "Du kannst nun mit RT dashen.";
                    session.CollectedDashLoot();
                    break;
                case MovementSkillTypes.JUMP:
                    info = "Du hast den Doppelsprung freigeschaltet!";
                    session.CollectedJumpLoot();
                    break;
                default:
                    break;
            }

            ShowTextinfo();
        }

        private void ShowTextinfo()
        {
            Game.instance.lootInfo.SetActive(true);
            Game.instance.lootInfo.GetComponentInChildren<TextMeshProUGUI>().text = info;
            StartCoroutine(HideTextBox());
        }

        IEnumerator HideTextBox()
        {
            yield return new WaitForSeconds(3);
            Game.instance.lootInfo.SetActive(false);

        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TE
{
    public class Gems : Loot
    {
        public enum GemTypes
        {
            SwordBuff, // +2 sec for hits on enemies
            LessSkillCosts, // reduces skill cost
            LessTimeMagicCosts, // reduces time magic costs
            IncreasePlayerSpeed, // Player moves slightly faster
            IncreasePlayerJump, // Player can jum slightly higher
            ReduceDamage, // Player loses less time if he gets hit from enemy
            CrazyGem, // Doubles Damage, Doubles countdown speed
            DamageDash, // Dash makes damage
            FasterSword // Sword is faster
        }


        public GemTypes gemType;
        public string info;

        public override void GemBehaviour()
        {
            Debug.Log("Picked up a " + gemType + "!");
            switch (gemType)
            {
                case GemTypes.SwordBuff:
                    Game.instance.timeBonusOnHit += 2;
                    info = "Schlag dir deine Zeit zusammen!";
                    break;
                case GemTypes.LessSkillCosts:
                    // TODO Where are skill costs?
                    Debug.Log("Hannah needs to Implement this gem!");
                    info = "Keine Funktion, Dumme Programmierer!";
                    break;
                case GemTypes.LessTimeMagicCosts:
                    // TODO Where are magic costs?
                    Debug.Log("Hannah needs to Implement this gem!");
                    info = "Keine Funktion, Faule Programmierer!";
                    break;
                case GemTypes.IncreasePlayerSpeed:
                    Game.instance.player.moveSpeed *= 1.25f;
                    info = "Schneller, SCHNELLER!";
                    break;
                case GemTypes.IncreasePlayerJump:
                    Game.instance.player.jumpVelocity *= 1.25f;
                    info = "Höheres Springen!";
                    break;
                case GemTypes.ReduceDamage:
                    Game.instance.player.takenDamageModifier *= 1.125f;
                    info = "Mehr Schaden gegen Gegner, YEAH!";
                    break;
                case GemTypes.CrazyGem:
                    Game.instance.player.damageModifier *= 2;
                    Game.instance.worldTimeScale *= 2f;
                    info = "WTF IS HAPPENING?!";
                    break;
                case GemTypes.DamageDash:
                    // TODO Where to implement this?
                    Debug.Log("Hannah needs to Implement this gem!");
                    info = "Error 404, Gem Not Found!";
                    break;
                case GemTypes.FasterSword:
                    // TODO 
                    Debug.Log("Hannah needs to Implement this gem!");
                    info = "Keine Funktion, Lame!";
                    break;
            }
            
            //ShowTextinfo();
        }

        private void ShowTextinfo()
        {
            textBox.SetActive(true);
            textBox.GetComponentInChildren<TextMeshProUGUI>().text = info;
            StartCoroutine(HideTextBox());
        }

        IEnumerator HideTextBox()
        {
            yield return new WaitForSeconds(2);
            Debug.Log("HERE");
            textBox.SetActive(false);
            
        }
    }
}
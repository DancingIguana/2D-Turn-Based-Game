using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class BatlleHUDEnemy : MonoBehaviour
{
    //[SerializeField] Text nameText;
    //[SerializeField] Text levelText;
    [SerializeField] Text hpText;

    [SerializeField] HPBar hpBar;

    Fighter _fighter;
    public void SetData(Fighter fighter) {
        //nameText.text = fighter.Base.Name;
        //levelText.text = "Lvl " + fighter.Level; 
        _fighter = fighter;
        hpBar.SetHP((float) fighter.HP / fighter.MaxHp, (int) fighter.HP);
    }
    public void UpdateHP()
    {
        hpBar.SetHP((float) _fighter.HP / _fighter.MaxHp, _fighter.HP);

    }
}

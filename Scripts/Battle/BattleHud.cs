using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    //[SerializeField] Text nameText;
    //[SerializeField] Text levelText;
    [SerializeField] Text hpText;
    [SerializeField] Text mpText;
    [SerializeField] HPBar hpBar;
    [SerializeField] MPBar mpBar;
    [SerializeField] Image icon;
    [SerializeField] Image iconBG;
    [SerializeField] Image BGBG;

    Fighter _fighter;

    public void SetData(Fighter fighter) {
        //nameText.text = fighter.Base.Name;
        //levelText.text = "Lvl " + fighter.Level; 

        _fighter = fighter;

        hpBar.SetHP((float) fighter.HP / fighter.MaxHp, (int) fighter.HP);
        mpBar.SetMP((float) fighter.MP / fighter.MaxMp, (int) fighter.MP);
        iconBG.sprite = fighter.Base.IconBG;
        icon.sprite = fighter.Base.Icon;
        BGBG.sprite = fighter.Base.IconBG;

    }

    public void UpdateHP()
    {
        hpBar.SetHP((float) _fighter.HP / _fighter.MaxHp, _fighter.HP);
        mpBar.SetMP((float) _fighter.MP / _fighter.MaxMp, (int) _fighter.MP);
    }
}

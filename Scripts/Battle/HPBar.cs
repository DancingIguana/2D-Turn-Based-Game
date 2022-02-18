using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;
    [SerializeField] Text healthText;
    [SerializeField] Text healthTextBG;
    
    public void SetHP(float hpNormalized, int hp) {
        health.transform.localScale = new Vector3(hpNormalized,1f);
        healthText.text = "" + hp;
        healthTextBG.text = "" + hp;
    }

}

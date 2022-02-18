using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DamageIcon : MonoBehaviour
{
    [SerializeField] GameObject health;
    [SerializeField] Text damageText;
    
    public void SetHP(float hpNormalized, int damage) {
        health.transform.localScale = new Vector3(hpNormalized,1f);
        damageText.text = "" + damage;
    }

}

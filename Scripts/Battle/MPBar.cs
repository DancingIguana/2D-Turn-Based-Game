using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPBar : MonoBehaviour
{
    [SerializeField] GameObject mp;
    [SerializeField] Text mpText;
    [SerializeField] Text mpTextBG;

    public void SetMP(float mpNormalized, int mp2) {
        mp.transform.localScale = new Vector3(mpNormalized,1f);
        mpText.text = "" + mp2;
        mpTextBG.text = "" + mp2;
    }
}

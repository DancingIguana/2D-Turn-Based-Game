using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DungeonUI : MonoBehaviour
{
    [SerializeField] GameObject iconBox;
    [SerializeField] BattleHud iconTemplate;
    [SerializeField] Text score;
    FighterParty playerParty;
    // Start is called before the first frame update
    void Start()
    {
        playerParty =  Characters.instance.playerParty;
        score.text = "TOTAL SCORE: " + TotalScore.instance.totalScore;
        spawnCharIcons();
    }

    void spawnCharIcons() 
    {
        int ypos = 0;
        for(int i = 0; i < playerParty.Fighters.Count; i++) {
            BattleHud charIcon = Instantiate(iconTemplate);
            charIcon.transform.SetParent(iconBox.transform, false);
            charIcon.transform.localPosition = new Vector3 (320, 170 - ypos, 0);
            charIcon.SetData(playerParty.Fighters[i]);
            ypos += 110;
        }
    }
}

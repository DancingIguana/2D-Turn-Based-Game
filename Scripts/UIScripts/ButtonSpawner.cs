using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonSpawner : MonoBehaviour
{
    [SerializeField] GameObject buttonTemplate;
    private BattleUnit playerUnit;
    private Text[] texts;
    // Start is called before the first frame update
    private List<Move> moves;
    [SerializeField] List<Button> skillMenuButtons;
    List<GameObject> spawnedButtons = new List<GameObject>();

    public void Setup(Fighter player) {
        moves = player.Moves;
        foreach(var move in moves) {
            GameObject button = Instantiate(buttonTemplate, transform);
            spawnedButtons.Add(button);
            texts =  button.GetComponentsInChildren<Text>();
            texts[0].text = move.Base.Name;

            if(move.Base.Type == FighterType.Physical) {
                texts[1].text = move.Base.Cost + "HP";
            } else {
                texts[1].text = move.Base.Cost + "SP";
            }
            Button btn = button.GetComponent<Button>();
            skillMenuButtons.Add(btn);
        }
    }

    public List<Button> SkillMenuButtons {
        get{ return skillMenuButtons; }
    }
    
    public void CleanSkillMenu() {
        
        if(skillMenuButtons.Count > 1)
            skillMenuButtons.RemoveRange(1, skillMenuButtons.Count - 1);
        
        Debug.Log(skillMenuButtons.Count);

        for(var i = 0 ; i < spawnedButtons.Count ; i ++)
        {
            Destroy(spawnedButtons[i]);
        }
        spawnedButtons = new List<GameObject>();
    }
    
}

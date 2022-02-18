using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetScore : MonoBehaviour
{
    [SerializeField] Text scoreText;
    void Start() {

        Debug.Log(BattleSystem.nTurns);

        scoreText.text = "" + (int)(StoreEnemy.instance.enemy.Level*50 + (float)(1/(float)(BattleSystem.nTurns))*100);
        TotalScore.instance.totalScore += (int)(StoreEnemy.instance.enemy.Level*50 + (float)(1/(float)(BattleSystem.nTurns))*100);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{

    [SerializeField] bool isPlayerUnit;
    public Fighter Fighter {get; set;}

    public void Setup(Fighter fighter) {
        Fighter = fighter;

        GameObject model = Instantiate(Fighter.Base.FModel);

        model.tag = "EnemyModel";

        if(isPlayerUnit)
            model.tag = "PlayerModel";
        
        model.transform.position = this.transform.position;
        if(isPlayerUnit){
            model.transform.position = this.transform.position + new Vector3(-10,0,0);
            model.transform.rotation = Quaternion.Euler(0,180f,0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FighterParty : MonoBehaviour
{
    [SerializeField] List<Fighter> fighters;
    //public static FighterParty instance;
    private void Start()
    {
        foreach ( var fighter in fighters )
        {
            fighter.Init();
        }
        //instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }


    public List<Fighter> Fighters {
        get {return fighters;}
    }

    public Fighter GetHealthyFighter()
    {
        return fighters.Where(x => x.HP > 0).FirstOrDefault();
    }

    public int GetAliveCount()
    {
        int counter = 0;
        for(int i = 0; i < fighters.Count; i++) {
            if(fighters[i].HP > 0)
                counter++;
        }
        return counter;
    }

    public int GetNextAlive(int index)
    {
        for(int i = index+1; i < fighters.Count; i++) {
            if(fighters[i].HP>0) {
                return i;
            }
        }
        return 0;
    }

    public int GetPrevAlive(int index) {
        return 0;
    }
    public int GetRandomAlive() {
        List<int> healthyFighters = new List<int>();

        for(int i = 0; i < fighters.Count; i++) {
            if(fighters[i].HP>0) {
                healthyFighters.Add(i);
            }
        }
        int r = Random.Range(0,healthyFighters.Count);
        return healthyFighters[r];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fighter
{
    [SerializeField] FighterBase _base;
    [SerializeField] int level;

    public FighterBase Base {
        get{
            return _base;
        }
    }
    public  int Level {
        get {
            return level;
        }
    }
    public int HP { get; set; }
    public int MP { get; set;}
    public List<Move> Moves { get; set; }

    public void Init()
    {

        HP = MaxHp;
        MP = MaxMp;

        //Generate moves
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if(move.Level <= Level)
                Moves.Add(new Move(move.Base));
            if(Moves.Count >= 10)
                break;
        }
    }


    
    public int Attack {
        get { return Mathf.FloorToInt((Base.Attack + Base.Attack*Level*0.1f)); }
    }

    public int Defense {
        get { return Mathf.FloorToInt((Base.Defense + Base.Defense*Level*0.1f)); }
    }
    public int MpAttack {
        get { return Mathf.FloorToInt((Base.MpAttack + Base.MpAttack*Level*0.1f)); }
    }
    public int MpDefense {
        get { return Mathf.FloorToInt((Base.MpDefense + Base.MpDefense*Level*0.1f)); }
    }
    public int MaxHp {
        get { return Mathf.FloorToInt((Base.MaxHp + Base.MaxHp*Level*0.1f)); }
    }
    public int MaxMp {
        get { return Mathf.FloorToInt((Base.MaxMp + Base.MaxMp*Level*0.1f)); }
    }

    public void Heal() {
        HP += 50;
        if(HP >= MaxHp) {
            HP = MaxHp;
        }
    }

    public DamageDetails TakeDamage(Move move, Fighter attacker)
    {
        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type);
        float critical = 1f;
        if(Random.value * 100f <= 10 && (type > 1f || move.Base.Type == FighterType.Physical))
            critical = 2f;

        //Debug.Log("Damage Details");
        //Debug.Log(type);
        //Debug.Log(critical);
        

        float modifiers = Random.Range(0.85f,1f) * type * critical;
        float a = (2*attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d*modifiers);
        //Debug.Log("Took damage of");
        //Debug.Log(damage);

        var damageDetails = new DamageDetails ()
        {
            Type = type,
            Critical = critical,
            Fainted = false,
            Damage = damage
        };

        HP -= damage;
        if(HP <= 0) 
        {
            HP = 0;
            damageDetails.Fainted = true;
        }
        return damageDetails;
    }

    public void skillCost(Move move) 
    {
        if(move.Base.Type == FighterType.Physical) {
            HP -= move.Cost;
        }
        else {
            MP -= move.Cost;
        }
    }

    public Move GetRandomMove() {
        int r = Random.Range(0,Moves.Count);
        return Moves[r];
    }


}

public class DamageDetails
{
    public bool Fainted {get; set;}

    public float Critical {get; set;}

    public float Type {get; set;}

    public int Damage {get; set;}
}

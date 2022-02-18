using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "FighterBase", menuName = "Fighter/Create new fighter")]
public class FighterBase : ScriptableObject
{
    [SerializeField] string fName;
    [SerializeField] GameObject fModel;
    [SerializeField] Sprite cutin;
    [SerializeField] Sprite icon;
    [SerializeField] Sprite iconBG;
    [SerializeField] FighterType type;

    //Base Stats
    [SerializeField] int maxHp;
    [SerializeField] int maxMp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int mpAttack;
    [SerializeField] int mpDefense;

    [SerializeField] List<LearnableMove>  learnableMoves;

    [SerializeField] List<AudioClip> attackSounds;
    [SerializeField] AudioClip criticalSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip wakeupSound;
    [SerializeField] List<AudioClip> entranceSounds;

    public string Name {
        get { return fName; } 
    }

    public GameObject FModel{
        get { return fModel; } 
    }
    
    public Animator FAnimator {
        get { return fModel.GetComponentInChildren<Animator>(); }
    }

    public Sprite Cutin {
        get { return cutin; }
    }

    public Sprite Icon {
        get { return icon; }
    }

    public Sprite IconBG {
        get { return iconBG; }
    }
    public FighterType Type {
        get { return type; } 
    }
    public int MaxHp {
        get { return maxHp; } 
    }
    public int MaxMp {
        get { return maxMp; }
    }
    public int Attack {
        get { return attack; }
    }
    public int Defense {
        get { return defense; }
    }
    public int MpAttack {
        get { return mpAttack; }
    }
    public int MpDefense {
        get { return mpDefense; }
    }
    public List<LearnableMove> LearnableMoves {
        get { return learnableMoves; }
    }

    public AudioClip AttackSound {
        get { return attackSounds[Random.Range(0, attackSounds.Count)]; }
    }

    public AudioClip CriticalSound {
        get { return criticalSound; }
    }

    public AudioClip DeathSound {
        get { return deathSound; }
    }

    public AudioClip HurtSound {
        get { return hurtSound; }
    }
    public AudioClip WakeupSound {
        get { return wakeupSound; }
    }

    public AudioClip EntranceSound {
        get { return entranceSounds[Random.Range(0, entranceSounds.Count)]; }
    }
}

[System.Serializable]
public class LearnableMove {
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base {
        get { return moveBase; }
    }

    public int Level {
        get { return level; }
    }
}

public enum FighterType{
    None,
    Physical,
    Fire,
    Ice,
    Wind,
    Electric,
    Heal
}

public class TypeChart
{
    //1/2 = Resistance
    //1 = Normal
    //2 = Weakness
    static float[][] chart = {
        //                  PHY FIR   ICE    WIN   ELE
        /*PHY*/ new float[] {1f, 1f,   1f,    1f,   1f},
        /*FIR*/ new float[] {1f, 0.5f, 2f,    1f,   1f},
        /*ICE*/ new float[] {1f, 2f,   0.5f,  1f,   1f},
        /*WIN*/ new float[] {1f, 1f,   1f,  0.5f,   2f},
        /*ELE*/ new float[] {1f, 1f,   1f,    2f,   0.5f}
    };

    public static float GetEffectiveness(FighterType attackType, FighterType defenseType)
    {
        if(attackType == FighterType.None || defenseType == FighterType.None)
            return 1;
        
        int row = (int)attackType - 1;
        int col = (int)defenseType - 1;

        return chart[row][col];
    }
}
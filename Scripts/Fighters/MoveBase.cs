using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move",menuName = "Fighter/Create new move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string mName;
    
    [TextArea] 
    [SerializeField] string description;
    [SerializeField] FighterType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int cost;

    public string Name {
        get { return mName; }
    }
    public string Description {
        get { return description; }
    }
    public FighterType Type {
        get { return type; }
    }
    public int Power {
        get { return power; }
    }

    public int Accuracy {
        get { return accuracy; }
    }
    public int Cost {
        get { return cost; }
    }

}

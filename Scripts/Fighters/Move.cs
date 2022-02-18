using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public MoveBase Base { get; set; }

    public int Cost {get; set;}
    
    public Move(MoveBase fBase) {
        Base = fBase;
        Cost = fBase.Cost;
    }
}

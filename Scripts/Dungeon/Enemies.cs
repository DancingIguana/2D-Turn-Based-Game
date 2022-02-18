using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemies : MonoBehaviour
{
    [SerializeField] List<Fighter> enemyList;

    public List<Fighter> EnemyList {
        get {return enemyList;}
    }
}

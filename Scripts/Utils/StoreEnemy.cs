using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreEnemy : MonoBehaviour
{
    public static StoreEnemy instance;
    public Fighter enemy;
    public bool playerTurn = false;

    private void Awake()
    {
        
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }
    
}

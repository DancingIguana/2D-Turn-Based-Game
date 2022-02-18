using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    public FighterParty playerParty;
    public Enemies enemyList;
    public Vector3 playerPos;
    public List<Vector3> enemyPos;
    public static Characters instance;

    void Awake()
    {
        playerParty = GetComponent<FighterParty>();
        enemyList = GetComponent<Enemies>();
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

}

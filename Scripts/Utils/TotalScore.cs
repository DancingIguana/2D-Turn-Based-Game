using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalScore : MonoBehaviour
{
    // Start is called before the first frame update
    public int totalScore;
    public static TotalScore instance;

     void Awake()
    {
        totalScore = 0;
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionImage : MonoBehaviour
{
    public static TransitionImage instance;
    public SpriteRenderer image;

    private void Awake()
    {
        image = GetComponent<SpriteRenderer>();
        
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    [SerializeField] AudioSource audioSrc;
    [SerializeField] AudioClip hover;
    [SerializeField] AudioClip summon;
    [SerializeField] AudioClip fail;
    [SerializeField] AudioClip critical;
    [SerializeField] AudioClip fire;
    [SerializeField] AudioClip wind;
    [SerializeField] AudioClip electric;
    [SerializeField] AudioClip ice;
    [SerializeField] AudioClip heal;
    [SerializeField] AudioClip physical;
    [SerializeField] AudioClip openSkill;
    [SerializeField] AudioClip closeSkill;
    [SerializeField] AudioClip openAction;
    [SerializeField] AudioClip startBattle;


    public AudioClip Hover {
        get { return hover; }
    }

    public AudioClip Summon {
        get { return summon; }
    }

    public AudioClip Fail {
        get { return fail; }
    }
    public AudioClip Critical {
        get { return critical; }
    }

    public AudioClip Fire {
        get { return fire; }
    }

    public AudioClip Wind {
        get { return wind; }
    }

    public AudioClip Electric {
        get { return electric; }
    }

    public AudioClip Ice {
        get { return ice; }
    }

    public AudioClip Heal {
        get { return heal; }
    }
    public AudioClip Physical {
        get { return physical; }
    }

    public AudioClip OpenSkill {
        get {return openSkill;}
    }
    public AudioClip CloseSkill {
        get { return closeSkill; }
    }
    public AudioClip OpenAction{
        get { return openAction; }
    }

    public AudioClip StartBattle{
        get { return startBattle; }
    }
}

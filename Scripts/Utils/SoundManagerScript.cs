using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    static AudioSource audioSrc;
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip aclip, float volume = 0.3f)
    {
        Debug.Log("Playing" + aclip);
        audioSrc.PlayOneShot(aclip, volume);
    }
}

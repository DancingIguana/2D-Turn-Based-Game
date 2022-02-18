using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class buttonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] AudioClip hover;
    [SerializeField] AudioClip close;

    // Start is called before the first frame update
    public void OnPointerEnter (PointerEventData eventData)
    {
        SoundManagerScript.PlaySound(hover,1);
    }

    public void OnPointerClick (PointerEventData eventData)
    {
        SoundManagerScript.PlaySound(close, 1);
    }
}

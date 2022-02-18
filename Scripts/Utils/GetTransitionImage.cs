using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetTransitionImage : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Image image;
    // Start is called before the first frame update
    void Start()
    {
        image.sprite = TransitionImage.instance.image.sprite;
        image.rectTransform.sizeDelta = new Vector2(800,450);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimControl : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject title;
    [SerializeField] GameObject subtitle;
    [SerializeField] GameObject btn;
    Animator titleAnim;
    Animator subtitleAnim;
    void Start()
    {
        titleAnim = title.GetComponent<Animator>();
        subtitleAnim = title.GetComponent<Animator>();
        StartCoroutine(StartMenu());
    }

    IEnumerator StartMenu()
    {
        yield return new WaitForSeconds(3f);
        title.SetActive(true);
        yield return new WaitForSeconds(2f);
        subtitle.SetActive(true);
        yield return new WaitForSeconds(2f);
        btn.SetActive(true);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

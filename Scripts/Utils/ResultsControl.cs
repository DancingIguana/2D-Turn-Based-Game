using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsControl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject blackBG;
    Animator anim;
    void Start()
    {
        anim = blackBG.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene()
    {
        yield return null;
        anim.SetBool("close",true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class countdown : MonoBehaviour
{
    [SerializeField] GameObject image;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CDown());
    }

    // Update is called once per frame

    IEnumerator CDown()
    {
        yield return new WaitForSeconds(26f);
        image.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);


    }
}

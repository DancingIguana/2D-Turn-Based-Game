using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CheckWin : MonoBehaviour
{
    private GameObject character;
    private bool defined = false;
    [SerializeField] GameObject victoryText;
    [SerializeField] GameObject fade;

    void Start()
    {
        StartCoroutine(waitSpawn());
    }
    IEnumerator waitSpawn()
    {
        yield return new WaitForSeconds(1f);
        character = GameObject.FindGameObjectsWithTag("Player")[0];
        defined = true;
    }
    IEnumerator Win()
    {
        yield return new WaitForSeconds(5f);
        fade.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
        yield return null;
    }
    void Update()
    {
        if(defined)
        {
            if(character.transform.position.x <= -4)
            {
                victoryText.SetActive(true);
                StartCoroutine(Win());
                Debug.Log("The end!");
            }
        }
    }
}

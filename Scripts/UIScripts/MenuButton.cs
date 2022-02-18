using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject transition;
    public void OnPointerClick (PointerEventData eventData)
    {
        Debug.Log("Clicked");
        StartCoroutine(startGame());
    }
    IEnumerator startGame()
    {
        yield return null;
        transition.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
}

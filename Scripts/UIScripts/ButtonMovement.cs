using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonMovement : MonoBehaviour, IPointerEnterHandler
{
    // Start is called before the first frame update
    GameObject playerGO;
    Player player;


    void Start()
    {
        StartCoroutine(GetPlayer());
    }

    IEnumerator GetPlayer()
    {
        yield return new WaitForSeconds(1f);
        playerGO = GameObject.FindGameObjectsWithTag("Player")[0];
        player = playerGO.GetComponent<Player>();
    }
    // Update is called once per frame
    public void OnPointerEnter (PointerEventData eventData)
     {
         player.moveDirection = new Vector2(1.0f,0.0f);
     }
}

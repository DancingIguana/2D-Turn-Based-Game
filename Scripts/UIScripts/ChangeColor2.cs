 using UnityEngine;
 using UnityEngine.EventSystems;
 using UnityEngine.UI;
 
 public class ChangeColor2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
 
     private Text myText;
     [SerializeField] Text action;
     [SerializeField] int id;

     void Start (){
         myText = GetComponentInChildren<Text>();
     }
 
     public void OnPointerEnter (PointerEventData eventData)
     {
         action.color = Color.white;
         
     }
       public void OnPointerClick(PointerEventData pointerEventData)
    {
        action.color = Color.black;
    }
 
     public void OnPointerExit (PointerEventData eventData)
     {
         action.color = Color.black;
     }
     
 }
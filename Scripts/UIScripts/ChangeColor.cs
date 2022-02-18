 using UnityEngine;
 using UnityEngine.EventSystems;
 using UnityEngine.UI;
 
 public class ChangeColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
 
     private Text myText;
     [SerializeField] Text SkillName;
     [SerializeField] Text Cost;

     void Start (){
         myText = GetComponentInChildren<Text>();
     }
    
     public void OnPointerEnter (PointerEventData eventData)
     {
         SkillName.color = Color.black;
         Cost.color = Color.black;
     }
     
      public void OnPointerClick(PointerEventData pointerEventData)
    {
        SkillName.color = Color.white;
        Cost.color = Color.white;
    }
     public void OnPointerExit (PointerEventData eventData)
     {   
         SkillName.color = Color.white;
         Cost.color = Color.white;
     }
 }
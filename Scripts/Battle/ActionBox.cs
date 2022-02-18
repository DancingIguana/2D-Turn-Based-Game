using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActionBox : MonoBehaviour
{
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject skillSelector;
    [SerializeField] GameObject charSelector;
    [SerializeField] List<Button> actionButtons;
    [SerializeField] List<Button> skillButtons;
    [SerializeField] List<Button> selectButtons;
    [SerializeField] Text selectName;
    // Start is called before the first frame update

    public void EnableActionMenu(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }
    public void EnableSkillMenu(bool enabled)
    {
        skillSelector.SetActive(enabled);
    }
    public void EnableSelectMenu(bool enabled)
    {
        charSelector.SetActive(enabled);   
    }

    public List<Button> ActionButtons {
        get{ return actionButtons; }
    }

    public List<Button> SelectButtons {
        get {return selectButtons;}
    }
    public Text SelectName {
        get {return selectName;}
    }
}

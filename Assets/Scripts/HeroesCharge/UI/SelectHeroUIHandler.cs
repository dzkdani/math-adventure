using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectHeroUIHandler : MonoBehaviour 
{
    public Image HeroIcon; 
    public string HeroName;
    public TextMeshProUGUI HeroLevelTxt;
    public Image DisabledImg;
    public Image SelectedIcon;
    public Button SelectHeroBtn; 

    void Awake()
    {
        DisabledImg.enabled = false;
        SelectedIcon.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectTroopUIHandler : MonoBehaviour
{
    public Image TroopIcon;
    public string TroopName;
    public TextMeshProUGUI TroopLevelTxt;
    public Image DisabledImg;
    public Image SelectedIcon;
    public Button SelectTroopBtn;

    void Awake()
    {
        DisabledImg.enabled = false;
        SelectedIcon.enabled = false;
    }
}

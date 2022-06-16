using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectStageUIHandler : MonoBehaviour
{
    public string StageName;
    public Image IconLocked;
    public Image IconCleared;
    public Button SelectStageBtn;
    public GameObject StarImg;

    private void Start() {
        IconLocked.enabled = true; 
        StarImg.SetActive(false);
        IconCleared.enabled = false;
        SelectStageBtn.interactable = false;
    }
}

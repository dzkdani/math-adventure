using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureRoomQuizSubTopicUI : MonoBehaviour
{
    public TextMeshProUGUI SubTopicTitleTxt;
    public List<Image> CorrectAnswerProgressList;
    public Image ChestCloseImg;
    public Image ChestOpenImg;
    public Image LockImg;
    public Image UnlockedImg;
    public Button ChooseSubTopicBtn;

    void Awake()
    {
        ChestCloseImg.enabled = true;
        ChestOpenImg.enabled = false;
        LockImg.enabled = true;
        UnlockedImg.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StudyRoomQuizSubTopicUI : MonoBehaviour
{
    public TextMeshProUGUI SubTopicTitleTxt;
    public List<Image> CorrectAnswerProgressList;
    public Image LockImg;
    public Image UnlockedImg;
    public Button ChooseSubTopicBtn;

    void Awake()
    {
        LockImg.enabled = true;
        UnlockedImg.enabled = false;
    }
}

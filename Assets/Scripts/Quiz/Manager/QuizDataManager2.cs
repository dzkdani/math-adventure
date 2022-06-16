using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;

public class QuizDataManager2 : MonoBehaviour
{
    //[System.Serializable]
    //public enum QuizRoom
    //{
    //    Study,
    //    Treasure
    //}

    //[System.Serializable]
    //public struct QuizRoomReward
    //{
    //    public int mark;
    //    public int reward;
    //    public int afterQuiz_reward;
    //}

    //[Header("DEBUG MODE")]
    //[Space(5)]
    //public bool IsResetQuizData;

    //[Header("QUIZ DATA LIST")]
    //[Space(5)]
    //public List<MainQuizData> StudyRoomQuizList = new List<MainQuizData>();
    //public List<MainQuizData> TreasureRoomQuizList = new List<MainQuizData>();
    //public List<int> ActiveClassList = new List<int>();
    //public List<QuizRoomReward> studyRoomRewards = new List<QuizRoomReward>();
    //public List<QuizRoomReward> treasureRoomMarks = new List<QuizRoomReward>();
    //public int TotalCorrectAnswersToGetReward = 6;

    //public float Multiplier_Reward_StudyRoom = 0.5f;
    //public float Multiplier_Reward_TreasureRoom = 0.5f;

    //public float Multiplier_Mark_StudyRoom = 0.5f;
    //public float Multiplier_Mark_TreasureRoom = 0.5f;

    //[Header("CURRENT STUDENT STATS")]
    //[Space(5)]
    //[SerializeField] private QuizRoom CurrentStudentQuizRoom;
    //[SerializeField] private MainQuizData.StudentClassEnum CurrentStudentClass;
    //[SerializeField] private MainQuizData.SemesterEnum CurrentStudentSemester;
    //[SerializeField] private string CurrentStudentMainBabId;
    //[SerializeField] private string CurrentStudentMainBabTitle;
    //[SerializeField] private string CurrentStudentSubBabId;
    //[SerializeField] private string CurrentStudentSubBabTitle;

    //private bool HasFinishedLoadingQuizAssets;

    //private int studyRoomIntroLoadCount;
    //private int studyRoomQuizLoadCount;

    //private int treasureRoomIntroLoadCount;
    //private int treasureRoomQuizLoadCount;

    //public static QuizDataManager2 Instance;

    //void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //    DontDestroyOnLoad(gameObject);

    //    //if (IsResetQuizData)
    //    //{
    //    //    ResetQuizData();
    //    //}
    //    //LoadQuizData();
    //    //CalculateTotalCorrectAnswers();
    //}
    //void Start()
    //{
    //    LoadingQuizAssets_StudyRoom();
    //    LoadingQuizAssets_TreasureRoom();
    //}

    //public void SaveQuizData()
    //{
    //    for (int i = 0; i < StudyRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < StudyRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                int bulean = -1;
    //                if (StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].HasDoneFinalQuestion) bulean = 1;
    //                else bulean = 0;

    //                PlayerPrefs.SetInt(
    //                    "StudyRoom" + "_" +
    //                    StudyRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    StudyRoomQuizList[i].Semester.ToString() + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "HasDoneFinalQuestion", bulean
    //                    );
    //                PlayerPrefs.SetInt(
    //                    "StudyRoom" + "_" +
    //                    StudyRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    StudyRoomQuizList[i].Semester.ToString() + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "HasAnsweredFinalQuestion",
    //                    StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].HasAnsweredFinalQuestion
    //                    );
    //                PlayerPrefs.SetInt(
    //                    "StudyRoom" + "_" +
    //                    StudyRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    StudyRoomQuizList[i].Semester.ToString() + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "HasEarnedFinalQuestionReward",
    //                    StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].HasEarnedFinalQuestionReward
    //                    );
    //                for (int l = 0; l < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList.Count; l++)
    //                {
    //                    PlayerPrefs.SetInt(
    //                        "StudyRoom" + "_" +
    //                    StudyRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    StudyRoomQuizList[i].Semester.ToString() + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "Soal" + (l + 1) +
    //                    "CorrectAnswer",
    //                    StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].IsCorrectAnswer
    //                    );
    //                }
    //            }
    //        }
    //    }
    //    for (int i = 0; i < TreasureRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < TreasureRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                int bulean = -1;
    //                if (TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].HasDoneFinalQuestion) bulean = 1;
    //                else bulean = 0;

    //                PlayerPrefs.SetInt(
    //                    "TreasureRoom" + "_" +
    //                    TreasureRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    TreasureRoomQuizList[i].Semester.ToString() + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "HasDoneFinalQuestion", bulean
    //                    );

    //                PlayerPrefs.SetInt(
    //                    "TreasureRoom" + "_" +
    //                    TreasureRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    TreasureRoomQuizList[i].Semester.ToString() + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "HasAnsweredFinalQuestion",
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].HasAnsweredFinalQuestion
    //                    );
    //                PlayerPrefs.SetInt(
    //                    "TreasureRoom" + "_" +
    //                    TreasureRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    TreasureRoomQuizList[i].Semester.ToString() + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "HasEarnedFinalQuestionReward",
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].HasEarnedFinalQuestionReward
    //                    );
    //                for (int l = 0; l < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList.Count; l++)
    //                {
    //                    PlayerPrefs.SetInt(
    //                    "TreasureRoom" + "_" +
    //                    TreasureRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    TreasureRoomQuizList[i].Semester.ToString() + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "Soal" + (l + 1) +
    //                    "CorrectAnswer",
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].IsCorrectAnswer
    //                    );
    //                }
    //            }
    //        }
    //    }
    //    CalculateTotalCorrectAnswers();
    //}
    //private void LoadQuizData()
    //{
    //    for (int i = 0; i < StudyRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < StudyRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                int bulean =
    //                PlayerPrefs.GetInt(
    //                    "StudyRoom" + "_" +
    //                    StudyRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    StudyRoomQuizList[i].Semester.ToString() + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "HasDoneFinalQuestion"
    //                    );
    //                if (bulean.Equals(1)) StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].HasDoneFinalQuestion = true;
    //                else StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].HasDoneFinalQuestion = false;

    //                StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].HasAnsweredFinalQuestion =
    //                PlayerPrefs.GetInt(
    //                    "StudyRoom" + "_" +
    //                    StudyRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    StudyRoomQuizList[i].Semester.ToString() + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "HasAnsweredFinalQuestion"
    //                    );
    //                StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].HasEarnedFinalQuestionReward =
    //                PlayerPrefs.GetInt(
    //                    "StudyRoom" + "_" +
    //                    StudyRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    StudyRoomQuizList[i].Semester.ToString() + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "HasEarnedFinalQuestionReward"
    //                    );
    //                for (int l = 0; l < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList.Count; l++)
    //                {
    //                    StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].IsCorrectAnswer =
    //                    PlayerPrefs.GetInt(
    //                    "StudyRoom" + "_" +
    //                    StudyRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    StudyRoomQuizList[i].Semester.ToString() + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "Soal" + (l + 1) +
    //                    "CorrectAnswer",
    //                    -1
    //                    );
    //                }
    //            }
    //        }
    //    }
    //    for (int i = 0; i < TreasureRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < TreasureRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                int bulean =
    //                PlayerPrefs.GetInt(
    //                    "TreasureRoom" + "_" +
    //                    TreasureRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    TreasureRoomQuizList[i].Semester.ToString() + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "HasDoneFinalQuestion"
    //                    );
    //                if (bulean.Equals(1)) TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].HasDoneFinalQuestion = true;
    //                else TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].HasDoneFinalQuestion = false;

    //                TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].HasAnsweredFinalQuestion =
    //                PlayerPrefs.GetInt(
    //                    "TreasureRoom" + "_" +
    //                    TreasureRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    TreasureRoomQuizList[i].Semester.ToString() + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "HasAnsweredFinalQuestion"
    //                    );
    //                TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].HasEarnedFinalQuestionReward =
    //                PlayerPrefs.GetInt(
    //                    "TreasureRoom" + "_" +
    //                    TreasureRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    TreasureRoomQuizList[i].Semester.ToString() + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "HasEarnedFinalQuestionReward"
    //                    );
    //                for (int l = 0; l < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList.Count; l++)
    //                {
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].IsCorrectAnswer =
    //                    PlayerPrefs.GetInt(
    //                    "TreasureRoom" + "_" +
    //                    TreasureRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    TreasureRoomQuizList[i].Semester.ToString() + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "Soal" + (l + 1) +
    //                    "CorrectAnswer",
    //                    -1
    //                    );
    //                }
    //            }
    //        }
    //    }
    //}
    //private void ResetQuizData()
    //{
    //    for (int i = 0; i < StudyRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < StudyRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                PlayerPrefs.DeleteKey(
    //                    "StudyRoom" + "_" +
    //                    StudyRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    StudyRoomQuizList[i].Semester.ToString() + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "HasDoneFinalQuestion"
    //                    );
    //                PlayerPrefs.DeleteKey(
    //                    "StudyRoom" + "_" +
    //                    StudyRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    StudyRoomQuizList[i].Semester.ToString() + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "HasAnsweredFinalQuestion"
    //                    );
    //                PlayerPrefs.DeleteKey(
    //                    "StudyRoom" + "_" +
    //                    StudyRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    StudyRoomQuizList[i].Semester.ToString() + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "HasEarnedFinalQuestionReward"
    //                    );
    //                for (int l = 0; l < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList.Count; l++)
    //                {
    //                    PlayerPrefs.DeleteKey(
    //                    "StudyRoom" + "_" +
    //                    StudyRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    StudyRoomQuizList[i].Semester.ToString() + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "Soal" + (l + 1) +
    //                    "CorrectAnswer"
    //                    );
    //                }
    //            }
    //        }
    //    }
    //    for (int i = 0; i < TreasureRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < TreasureRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                PlayerPrefs.DeleteKey(
    //                   "TreasureRoom" + "_" +
    //                   TreasureRoomQuizList[i].StudentClass.ToString() + "_" +
    //                   TreasureRoomQuizList[i].Semester.ToString() + "_" +
    //                   TreasureRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                   TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                   "HasDoneFinalQuestion"
    //                   );
    //                PlayerPrefs.DeleteKey(
    //                    "TreasureRoom" + "_" +
    //                    TreasureRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    TreasureRoomQuizList[i].Semester.ToString() + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "HasAnsweredFinalQuestion"
    //                    );
    //                PlayerPrefs.DeleteKey(
    //                    "TreasureRoom" + "_" +
    //                    TreasureRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    TreasureRoomQuizList[i].Semester.ToString() + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "HasEarnedFinalQuestionReward"
    //                    );
    //                for (int l = 0; l < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList.Count; l++)
    //                {
    //                    PlayerPrefs.DeleteKey(
    //                    "TreasureRoom" + "_" +
    //                    TreasureRoomQuizList[i].StudentClass.ToString() + "_" +
    //                    TreasureRoomQuizList[i].Semester.ToString() + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId +
    //                    "Soal" + (l + 1) +
    //                    "CorrectAnswer"
    //                    );
    //                }
    //            }
    //        }
    //    }
    //}
    //public void CalculateTotalCorrectAnswers()
    //{
    //    for (int i = 0; i < StudyRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < StudyRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                int totalCorrectAnswer = 0;
    //                int totalIncorrectAnswer = 0;
    //                for (int l = 0; l < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList.Count; l++)
    //                {
    //                    if (StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].IsCorrectAnswer == 1)
    //                    {
    //                        totalCorrectAnswer++;
    //                    }
    //                    else
    //                    {
    //                        totalIncorrectAnswer++;
    //                    }
    //                }
    //                StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].TotalCorrectAnswers = totalCorrectAnswer;
    //                StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].TotalIncorrectAnswers = totalIncorrectAnswer;
    //            }
    //        }
    //    }
    //    for (int i = 0; i < TreasureRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < TreasureRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                int totalCorrectAnswer = 0;
    //                int totalIncorrectAnswer = 0;
    //                for (int l = 0; l < (TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList.Count - 1); l++)
    //                {
    //                    if (TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].IsCorrectAnswer == 1)
    //                    {
    //                        totalCorrectAnswer++;
    //                    }
    //                    else
    //                    {
    //                        totalIncorrectAnswer++;
    //                    }
    //                }
    //                TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].TotalCorrectAnswers = totalCorrectAnswer;
    //                TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].TotalIncorrectAnswers = totalIncorrectAnswer;
    //            }
    //        }
    //    }
    //}
    //#region GETTER AND SETTER

    //public QuizRoom GetCurrentStudentQuizRoom()
    //{
    //    return CurrentStudentQuizRoom;
    //}
    //public void SetCurrentStudentQuizRoom(QuizRoom _studentQuizRoom)
    //{
    //    CurrentStudentQuizRoom = _studentQuizRoom;
    //}
    //public MainQuizData.StudentClassEnum GetCurrentStudentClass()
    //{
    //    return CurrentStudentClass;
    //}
    //public void SetCurrentStudentClass(MainQuizData.StudentClassEnum _studentClassEnum)
    //{
    //    CurrentStudentClass = _studentClassEnum;
    //}
    //public MainQuizData.SemesterEnum GetCurrentStudentSemester()
    //{
    //    return CurrentStudentSemester;
    //}
    //public void SetCurrentStudentSemester(MainQuizData.SemesterEnum _studentSemesterEnum)
    //{
    //    CurrentStudentSemester = _studentSemesterEnum;
    //}
    //public string GetCurrentStudentMainBab()
    //{
    //    return CurrentStudentMainBabId;
    //}
    //public void SetCurrentStudentMainBab(string _studentMainBabId)
    //{
    //    CurrentStudentMainBabId = _studentMainBabId;
    //}
    //public void SetCurrentStudentMainBabTitle(string _studentMainBabTitle)
    //{
    //    CurrentStudentMainBabTitle = _studentMainBabTitle;
    //}
    //public string GetCurrentStudentMainBabTitle()
    //{
    //    return CurrentStudentMainBabTitle;
    //}
    //public string GetCurrentStudentSubBab()
    //{
    //    return CurrentStudentSubBabId;
    //}
    //public string GetCurrentStudentSubBabTitle()
    //{
    //    return CurrentStudentSubBabTitle;
    //}
    //public void SetCurrentStudentSubBab(string _studentSubBab)
    //{
    //    CurrentStudentSubBabId = _studentSubBab;
    //}
    //public void SetCurrentStudentSubBabTitle(string _studentSubBabTitle)
    //{
    //    CurrentStudentSubBabTitle = _studentSubBabTitle;
    //}
    //#endregion

    //#region STUDY_ROOM
    //public QuizData GetQuizData_StudyRoom(
    //    MainQuizData.StudentClassEnum _studentClass,
    //    MainQuizData.SemesterEnum _semester,
    //    string _mainBab,
    //    string _subBab)
    //{
    //    for (int i = 0; i < StudyRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < StudyRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                if (StudyRoomQuizList[i].StudentClass == _studentClass &&
    //                StudyRoomQuizList[i].Semester == _semester &&
    //                StudyRoomQuizList[i].SubQuizDataList[j].MainBabId == _mainBab &&
    //                StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId == _subBab)
    //                {
    //                    return StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k];
    //                }
    //            }
    //        }
    //    }
    //    return null;
    //}
    //public QuizData GetCurrentQuizData_StudyRoom()
    //{
    //    return GetQuizData_StudyRoom(CurrentStudentClass, CurrentStudentSemester, CurrentStudentMainBabId, CurrentStudentSubBabId);
    //}
    //private void CalculateQuizLoadCounts_StudyRoom()
    //{
    //    for (int i = 0; i < StudyRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < StudyRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                studyRoomIntroLoadCount++;
    //                for (int l = 0; l < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList.Count; l++)
    //                {
    //                    for (int m = 0; m < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].QuestionReference.Count; m++)
    //                    {
    //                        studyRoomQuizLoadCount++;
    //                    }
    //                    for (int m = 0; m < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].ExplanationReference.Count; m++)
    //                    {
    //                        studyRoomQuizLoadCount++;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
    //public void LoadingQuizAssets_StudyRoom()
    //{
    //    CalculateQuizLoadCounts_StudyRoom();
    //    for (int i = 0; i < StudyRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < StudyRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                if (StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].IntroReference != null)
    //                {
    //                    StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].IntroReference.LoadAssetAsync<VideoClip>().Completed += OnFinishedLoadingIntro_StudyRoom;
    //                }
    //                else
    //                {
    //                    Debug.LogWarning(
    //                        StudyRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                        StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId + ": Asset reference is null");
    //                }

    //                for (int l = 0; l < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList.Count; l++)
    //                {
    //                    for (int m = 0; m < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].QuestionReference.Count; m++)
    //                    {
    //                        if (StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].QuestionReference[m] != null)
    //                        {
    //                            //StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].QuestionReference[m].LoadAssetAsync<Sprite>().Completed += OnFinishedLoadingQuestion_StudyRoom;
    //                        }
    //                        else
    //                        {
    //                            Debug.LogWarning(
    //                                StudyRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                                StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId + "_" +
    //                                "Question" + m + ": Asset reference is null");
    //                        }
    //                    }
    //                    for (int m = 0; m < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].ExplanationReference.Count; m++)
    //                    {
    //                        if (StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].ExplanationReference[m] != null)
    //                        {
    //                            //StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].ExplanationReference[m].LoadAssetAsync<Sprite>().Completed += OnFinishedLoadingQuestion_StudyRoom;
    //                        }
    //                        else
    //                        {
    //                            Debug.LogWarning(
    //                                StudyRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                                StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId + "_" +
    //                                "Explanation" + m + ": Asset reference is null");
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
    //private void OnFinishedLoadingIntro_StudyRoom(AsyncOperationHandle<VideoClip> _operation)
    //{
    //    if (_operation.Result == null)
    //    {
    //        Debug.LogError("no videos here.");
    //        return;
    //    }

    //    studyRoomIntroLoadCount--;
    //    if (studyRoomIntroLoadCount <= 0)
    //    {
    //        LoadingIntroLog_StudyRoom();
    //    }
    //}
    //private void OnFinishedLoadingQuestion_StudyRoom(AsyncOperationHandle<Sprite> _operation)
    //{
    //    if (_operation.Result == null)
    //    {
    //        Debug.LogError("no sprites here.");
    //        return;
    //    }

    //    studyRoomQuizLoadCount--;
    //    if (studyRoomQuizLoadCount <= 0)
    //    {
    //        HasFinishedLoadingQuizAssets = true;
    //        LoadingQuizLog_StudyRoom();
    //    }
    //}
    //private void LoadingIntroLog_StudyRoom()
    //{
    //    for (int i = 0; i < StudyRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < StudyRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                Debug.Log(StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].IntroReference.Asset + " has finished loading");
    //            }
    //        }
    //    }
    //}
    //private void LoadingQuizLog_StudyRoom()
    //{
    //    for (int i = 0; i < StudyRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < StudyRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                for (int l = 0; l < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList.Count; l++)
    //                {
    //                    for (int m = 0; m < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].QuestionReference.Count; m++)
    //                    {
    //                        Debug.Log(StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].QuestionReference[m] + " has finished loading");
    //                    }
    //                    for (int m = 0; m < StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].ExplanationReference.Count; m++)
    //                    {
    //                        Debug.Log(StudyRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].QuestionReference[m] + " has finished loading");
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
    //#endregion

    //#region TREASURE_ROOM
    //public QuizData GetQuizData_Treasure(
    //     MainQuizData.StudentClassEnum _studentClass,
    //     MainQuizData.SemesterEnum _semester,
    //     string _mainBab,
    //     string _subBab)
    //{
    //    for (int i = 0; i < TreasureRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < TreasureRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                if (TreasureRoomQuizList[i].StudentClass == _studentClass &&
    //                TreasureRoomQuizList[i].Semester == _semester &&
    //                TreasureRoomQuizList[i].SubQuizDataList[j].MainBabId == _mainBab &&
    //                TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId == _subBab)
    //                {
    //                    return TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k];
    //                }
    //            }
    //        }
    //    }
    //    return null;
    //}
    //public QuizData GetCurrentQuizData_TreasureRoom()
    //{
    //    return GetQuizData_Treasure(CurrentStudentClass, CurrentStudentSemester, CurrentStudentMainBabId, CurrentStudentSubBabId);
    //}
    //private void CalculateQuizLoadCounts_TreasureRoom()
    //{
    //    for (int i = 0; i < TreasureRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < TreasureRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                treasureRoomIntroLoadCount++;
    //                for (int l = 0; l < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList.Count; l++)
    //                {
    //                    for (int m = 0; m < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].QuestionReference.Count; m++)
    //                    {
    //                        treasureRoomQuizLoadCount++;
    //                    }
    //                    for (int m = 0; m < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].ExplanationReference.Count; m++)
    //                    {
    //                        treasureRoomQuizLoadCount++;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
    //public void LoadingQuizAssets_TreasureRoom()
    //{
    //    CalculateQuizLoadCounts_TreasureRoom();
    //    for (int i = 0; i < TreasureRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < TreasureRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                if (TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].IntroReference != null)
    //                {
    //                    TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].IntroReference.LoadAssetAsync<VideoClip>().Completed += OnFinishedLoadingIntro_TreasureRoom;
    //                }
    //                else
    //                {
    //                    Debug.LogWarning(
    //                        TreasureRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                        TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId + ": Asset reference is null");
    //                }

    //                for (int l = 0; l < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList.Count; l++)
    //                {
    //                    for (int m = 0; m < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].QuestionReference.Count; m++)
    //                    {
    //                        if (TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].QuestionReference[m] != null)
    //                        {
    //                            //TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].QuestionReference[m].LoadAssetAsync<Sprite>().Completed += OnFinishedLoadingQuestion_TreasureRoom;
    //                        }
    //                        else
    //                        {
    //                            Debug.LogWarning(
    //                                TreasureRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                                TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId + "_" +
    //                                "Question" + m + ": Asset reference is null");
    //                        }
    //                    }
    //                    for (int m = 0; m < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].ExplanationReference.Count; m++)
    //                    {
    //                        if (TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].ExplanationReference[m] != null)
    //                        {
    //                            //TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].ExplanationReference[m].LoadAssetAsync<Sprite>().Completed += OnFinishedLoadingQuestion_TreasureRoom;
    //                        }
    //                        else
    //                        {
    //                            Debug.LogWarning(
    //                                TreasureRoomQuizList[i].SubQuizDataList[j].MainBabId + "_" +
    //                                TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].SubBabId + "_" +
    //                                "Explanation" + m + ": Asset reference is null");
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
    //private void OnFinishedLoadingIntro_TreasureRoom(AsyncOperationHandle<VideoClip> _operation)
    //{
    //    if (_operation.Result == null)
    //    {
    //        Debug.LogError("no videos here.");
    //        return;
    //    }

    //    treasureRoomIntroLoadCount--;
    //    if (treasureRoomIntroLoadCount <= 0)
    //    {
    //        LoadingIntroLog_TreasureRoom();
    //    }
    //}
    //private void OnFinishedLoadingQuestion_TreasureRoom(AsyncOperationHandle<Sprite> _operation)
    //{
    //    if (_operation.Result == null)
    //    {
    //        Debug.LogError("no sprites here.");
    //        return;
    //    }

    //    treasureRoomQuizLoadCount--;
    //    if (treasureRoomQuizLoadCount <= 0)
    //    {
    //        HasFinishedLoadingQuizAssets = true;
    //        LoadingQuizLog_TreasureRoom();
    //    }
    //}
    //private void LoadingIntroLog_TreasureRoom()
    //{
    //    for (int i = 0; i < TreasureRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < TreasureRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                Debug.Log(TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].IntroReference.Asset + " has finished loading");
    //            }
    //        }
    //    }
    //}
    //private void LoadingQuizLog_TreasureRoom()
    //{
    //    for (int i = 0; i < TreasureRoomQuizList.Count; i++)
    //    {
    //        for (int j = 0; j < TreasureRoomQuizList[i].SubQuizDataList.Count; j++)
    //        {
    //            for (int k = 0; k < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList.Count; k++)
    //            {
    //                for (int l = 0; l < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList.Count; l++)
    //                {
    //                    for (int m = 0; m < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].QuestionReference.Count; m++)
    //                    {
    //                        Debug.Log(TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].QuestionReference[m] + " has finished loading");
    //                    }
    //                    for (int m = 0; m < TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].ExplanationReference.Count; m++)
    //                    {
    //                        Debug.Log(TreasureRoomQuizList[i].SubQuizDataList[j].QuizDataList[k].QuestionList[l].QuestionReference[m] + " has finished loading");
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
    //#endregion
}

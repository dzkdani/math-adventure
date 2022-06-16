using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Soal", menuName = "Datas/Quiz/Soal")]
public class QuizData : ScriptableObject
{
    [System.Serializable]
    public enum Kelas_Enum
    {
        Kelas3,
        Kelas4,
        Kelas5,
        Kelas6,
    }
    [System.Serializable]
    public enum Semester_Enum
    {
        Semester1,
        Semester2
    }

    [System.Serializable]
    public enum KategoriSoal_Enum
    {
        Study,
        Treasure
    }

    [System.Serializable]
    public enum Paket_Enum
    {
        Paket_Level_1,
        Paket_Level_2,
        Paket_Level_3,
        Paket_Level_4,
        Paket_Level_5
    }

    [Header("EDITABLE SECTION")]
    [Space(5)]
    public Paket_Enum Paket;
    public string Topik;
    public Semester_Enum Semester;
    public Kelas_Enum Kelas;
    public KategoriSoal_Enum KategoriSoal;

    public AssetReference IntroReference;
    public List<Sprite> IntroPictureList;
    public List<int> CorrectAnswerList;
    public List<QuestionData> QuestionList;
    

    [Header("NON-EDITABLE SECTION")]
    [Space(5)]
    public bool HasDoneFinalQuestion;
    public int HasEarnedFinalQuestionReward;
    public int HasAnsweredFinalQuestion;
    public int TotalCorrectAnswers;
    public int TotalIncorrectAnswers;
    public int Mark;

    [System.Serializable]
    public class QuestionData
    {
        [NonReorderable]
        public List<Sprite> QuestionReference;
        [NonReorderable]
        public List<Sprite> ExplanationReference;
        public string Answer;
        public int IsCorrectAnswer = -1;
    }

    public void Clear()
    {
        QuestionList.Clear();
        CorrectAnswerList.Clear();

        HasDoneFinalQuestion = false;
        HasEarnedFinalQuestionReward = 0;
        HasAnsweredFinalQuestion = 0;
        TotalCorrectAnswers = 0;
        TotalIncorrectAnswers = 0;
        Mark = 0;
    }
}

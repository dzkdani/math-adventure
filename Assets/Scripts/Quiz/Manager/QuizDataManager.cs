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

public class QuizDataManager : MonoBehaviour
{
    [System.Serializable]
    public struct QuizRoomReward
    {
        public int mark;
        public int reward;
        public int afterQuiz_reward;
    }

    [System.Serializable]
    public struct ListTopikByKelas
    {
        public QuizData.Kelas_Enum kelas;
        public List<ListTopikPerSemester> _listTopikPerSemester;
    }

    [System.Serializable]
    public struct ListTopikPerSemester
    {
        public QuizData.Semester_Enum semester;
        public List<ListTopik> topik;
    }

    [System.Serializable]
    public struct ListTopik
    {
        public int nomor;
        public string topik;
        public string id;
    }

    //[Header("DEBUG MODE")]
    //[Space(5)]
    //public bool IsResetQuizData;

    [Header("QUIZ DATA LIST")]
    [Space(5)]
    public List<ListTopikByKelas> _listTopikPerKelas;
    public List<QuizRoomReward> studyRoomRewards = new List<QuizRoomReward>();
    public List<QuizRoomReward> treasureRoomMarks = new List<QuizRoomReward>();
    public int TotalCorrectAnswersToGetReward = 6;

    public float Multiplier_Reward_StudyRoom = 0.5f;
    public float Multiplier_Reward_TreasureRoom = 0.5f;

    public float Multiplier_Mark_StudyRoom = 0.5f;
    public float Multiplier_Mark_TreasureRoom = 0.5f;

    [Header("CURRENT STUDENT STATS")]
    [Space(5)]
    [SerializeField] private QuizData.KategoriSoal_Enum CurrentKategoriSoal;
    [SerializeField] private QuizData.Kelas_Enum CurrentClass;
    [SerializeField] private QuizData.Semester_Enum CurrentSemester;
    [SerializeField] private QuizData.Paket_Enum CurrentPaket;
    [SerializeField] private string CurrentTopik;

    private bool HasFinishedLoadingQuizAssets;

    private int studyRoomIntroLoadCount;
    private int studyRoomQuizLoadCount;

    private int treasureRoomIntroLoadCount;
    private int treasureRoomQuizLoadCount;

    public static QuizDataManager Instance;

    void InisialisasiTopik()
    {
        _listTopikPerKelas = new List<ListTopikByKelas>();

        APIManager.Instance.categoryList.categories.Find(x => x.group.Equals("topic")).categoryDataStructs.ForEach(y => {
            char kelas = y.note.First();
            char semester = y.note.Last();
            string[] pisah = y.label.Split('_');

            QuizData.Kelas_Enum _class;
            switch (kelas)
            {
                case '3': _class = QuizData.Kelas_Enum.Kelas3; break;
                case '4': _class = QuizData.Kelas_Enum.Kelas4; break;
                case '5': _class = QuizData.Kelas_Enum.Kelas5; break;
                case '6': _class = QuizData.Kelas_Enum.Kelas6; break;

                default: _class = QuizData.Kelas_Enum.Kelas3; break;
            }

            QuizData.Semester_Enum _semester;
            switch (semester)
            {
                case '1': _semester = QuizData.Semester_Enum.Semester1; break;
                case '2': _semester = QuizData.Semester_Enum.Semester2; break;

                default: _semester = QuizData.Semester_Enum.Semester1; break;
            }

            ListTopikByKelas temp;

            if (_listTopikPerKelas.Exists(x => x.kelas.Equals(_class)))
            {
                temp = _listTopikPerKelas.Single(x => x.kelas.Equals(_class));
            }
            else
            {
                temp = new ListTopikByKelas
                {
                    kelas = _class,
                    _listTopikPerSemester = new List<ListTopikPerSemester>()
                };
                _listTopikPerKelas.Add(temp);
            }

            ListTopikPerSemester temp2;

            if (temp._listTopikPerSemester.Exists(x => x.semester.Equals(_semester)))
            {
                temp2 = temp._listTopikPerSemester.Single(x => x.semester.Equals(_semester));
            }
            else
            {
                temp2 = new ListTopikPerSemester
                {
                    semester = _semester,
                    topik = new List<ListTopik>()
                };
                temp._listTopikPerSemester.Add(temp2);
            }

            ListTopik temp3;

            if(temp2.topik.Exists(x => x.id.Equals(y.id))){

            }
            else
            {
                temp3 = new ListTopik
                {
                    topik = pisah[2],
                    id = y.id,
                    nomor = int.Parse(pisah[1])
                };
                temp2.topik.Add(temp3);
            }
            //durung
            //if (temp2.topik.Exists(x => x.topik.Equals()))
            //{
            //    temp2 = temp._listTopikPerSemester.Single(x => x.semester.Equals(_semester));
            //}
            //else
            //{
            //    temp2 = new ListTopikPerSemester
            //    {
            //        semester = _semester
            //    };
            //    temp._listTopikPerSemester.Add(temp2);
            //}
        });

        _listTopikPerKelas.ForEach(x => {
            x._listTopikPerSemester.ForEach(y => {
                y.topik.Sort((s1, s2) => s1.nomor.CompareTo(s2.nomor));
            });
        });
    }

    void Inisialisasi()
    {
        InisialisasiTopik();
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }
    void Start()
    {
        Inisialisasi();
        try
        {
            LoadingQuizAssets();
        }
        catch
        {
            StartCoroutine(LoadAgain());
        }
    }

    IEnumerator LoadAgain()
    {
        yield return new WaitForSeconds(5);
        LoadingQuizAssets();
    }

    public void CalculateTotalCorrectAnswers(QuizData _quizData)
    {
        int totalCorrectAnswer = 0;
        int totalIncorrectAnswer = 0;
        for (int l = 0; l < _quizData.QuestionList.Count; l++)
        {
            if (_quizData.QuestionList[l].IsCorrectAnswer == 1)
            {
                totalCorrectAnswer++;
            }
            else
            {
                totalIncorrectAnswer++;
            }
        }
        _quizData.TotalCorrectAnswers = totalCorrectAnswer;
        _quizData.TotalIncorrectAnswers = totalIncorrectAnswer;
        
    }
    #region GETTER AND SETTER

    public QuizData.KategoriSoal_Enum GetCurrentQuizKategoriSoal() { return CurrentKategoriSoal; }
    public void SetCurrentKategoriSoal(QuizData.KategoriSoal_Enum _kategoriSoal){ CurrentKategoriSoal = _kategoriSoal;}
    public QuizData.Kelas_Enum GetCurrentClass() { return CurrentClass; }
    public void SetCurrentClass(QuizData.Kelas_Enum _classEnum){ CurrentClass = _classEnum; }
    public QuizData.Semester_Enum GetCurrentSemester() { return CurrentSemester; }
    public void SetCurrentSemester(QuizData.Semester_Enum _semesterEnum) { CurrentSemester = _semesterEnum; }
    public QuizData.Paket_Enum GetCurrentPaket() { return CurrentPaket; }
    public void SetCurrentPaket(QuizData.Paket_Enum _paket) { CurrentPaket = _paket; }
    public void SetCurrentTopik(string _topik) { CurrentTopik = _topik; }
    public string GetCurrentTopik() { return CurrentTopik; }

    public QuizData GetCurrentQuizData()
    {
        return QuizDataLoader.Instance.GetQuizDatas().Find(i => i.Paket.Equals(CurrentPaket));
    }
    public List<ListTopik> GetListTopik()
    {
        return _listTopikPerKelas.Find(x => x.kelas.Equals(CurrentClass))._listTopikPerSemester.Find(x => x.semester.Equals(CurrentSemester)).topik; 
    }

    #endregion

    #region LOAD_VIDEO
    public void LoadingQuizAssets()
    {
        for (int j = 0; j < QuizDataLoader.Instance.GetQuizDatas().Count; j++)
        {
            if (QuizDataLoader.Instance.GetQuizData(j).IntroReference != null)
            {
                QuizDataLoader.Instance.GetQuizData(j).IntroReference.LoadAssetAsync<VideoClip>().Completed += OnFinishedLoadingIntro;
            }
            else
            {
                Debug.LogWarning("Paket " + j + " : Asset reference is null");
            }
        }
    }
    private void OnFinishedLoadingIntro(AsyncOperationHandle<VideoClip> _operation)
    {
        if (_operation.Result == null)
        {
            Debug.LogError("no videos here.");
            return;
        }

        studyRoomIntroLoadCount--;
        if (studyRoomIntroLoadCount <= 0)
        {
            LoadingIntroLog();
        }
    }
    
    private void LoadingIntroLog()
    {
        for (int j = 0; j < QuizDataLoader.Instance.GetQuizDatas().Count; j++)
        {
            Debug.Log(QuizDataLoader.Instance.GetQuizData(j).IntroReference.Asset + " has finished loading");
        }
    }
    
    #endregion
}

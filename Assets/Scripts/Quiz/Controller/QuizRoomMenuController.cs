using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class QuizRoomMenuController : MonoBehaviour
{
    [Header("CLASS MENU SECTION")]
    [Space(5)]
    public GameObject ClassMenu;
    public Transform ClassLayoutContainer;
    public GameObject ClassBtnPrefab;
    public Button BackFromClassBtn;

    [Header("MAINTOPIC MENU SECTION")]
    [Space(5)]
    public GameObject MainTopicMenu;
    public Transform MainTopicLayoutContainer;
    public GameObject MainTopicBtnPrefab;
    public TextMeshProUGUI MainTopicTitle;
    public Button BackFromMainTopicBtn;
    public Button NextSemesterBtn;
    public Button PrevSemesterBtn;
    private List<Button> MainTopicPrefabList = new List<Button>();

    [Header("SUBTOPIC MENU SECTION")]
    [Space(5)]
    public GameObject SubTopicMenu;
    public Transform SubTopicLayoutContainer;
    public GameObject StudyRoomSubTopicUIPrefab;
    public GameObject TreasureRoomSubTopicUIPrefab;
    public TextMeshProUGUI SubTopicTitle;
    public Button BackFromSubTopicBtn;
    private List<GameObject> StudyRoomSubTopicPrefabList = new List<GameObject>();
    private List<GameObject> TreasureRoomSubTopicPrefabList = new List<GameObject>();

    [Header("PROGRESSION SECTION")]
    public Sprite DefaultQuestionProgressionSprite;
    public Sprite CorrectQuestionProgressionSprite;
    public Sprite IncorrectQuestionProgressionSprite;

    [Header("SCENE SECTION")]
    [Space(5)]
    public string MenuSceneName;
    public string QuestionSceneName;

    [Header("LOADING SECTION")]
    [Space(5)]
    public GameObject LoadingScene;
    public Slider LoadingSlider;

    void Awake()
    {
        InitListeners();
    }
    void Start()
    {
        HideAllMenus();
        ClassMenu.SetActive(true);

        DisplayClass();
    }
    private void InitListeners()
    {
        BackFromClassBtn.onClick.AddListener(delegate
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFXType.ButtonClick);
            HideAllMenus();
            SceneManager.LoadScene(MenuSceneName);
        });

        NextSemesterBtn.onClick.AddListener(delegate
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFXType.ButtonClick);
            QuizDataManager.Instance.SetCurrentSemester(QuizData.Semester_Enum.Semester2);
            DisplayMainTopicSemesterTitle();
            switch (QuizDataManager.Instance.GetCurrentQuizKategoriSoal())
            {
                case QuizData.KategoriSoal_Enum.Study:
                    DisplayMainTopic(QuizDataManager.Instance.GetListTopik(), 0);
                    break;
                case QuizData.KategoriSoal_Enum.Treasure:
                    DisplayMainTopic(QuizDataManager.Instance.GetListTopik(), 1);
                    break;
            }
        });
        PrevSemesterBtn.onClick.AddListener(delegate
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFXType.ButtonClick);
            QuizDataManager.Instance.SetCurrentSemester(QuizData.Semester_Enum.Semester1);
            DisplayMainTopicSemesterTitle();
            switch (QuizDataManager.Instance.GetCurrentQuizKategoriSoal())
            {
                case QuizData.KategoriSoal_Enum.Study:
                    DisplayMainTopic(QuizDataManager.Instance.GetListTopik(), 0);
                    break;
                case QuizData.KategoriSoal_Enum.Treasure:
                    DisplayMainTopic(QuizDataManager.Instance.GetListTopik(), 1);
                    break;
            }
        });
        BackFromMainTopicBtn.onClick.AddListener(delegate
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFXType.ButtonClick);
            HideAllMenus();
            ClassMenu.SetActive(true);
        });

        BackFromSubTopicBtn.onClick.AddListener(delegate
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFXType.ButtonClick);
            HideAllMenus();
            MainTopicMenu.SetActive(true);
        });
    }
    private void HideAllMenus()
    {
        ClassMenu.SetActive(false);
        MainTopicMenu.SetActive(false);
        SubTopicMenu.SetActive(false);
    }
    
    #region CLASS
    private void DisplayClass()
    {
        for (int i = 0; i < 4; i++) 
        {
            int nomor = i + 3;
            Button spawnClass = Instantiate(ClassBtnPrefab, ClassLayoutContainer).GetComponent<Button>();
            spawnClass.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = nomor.ToString();

            int index = i;
            spawnClass.onClick.AddListener(delegate
            {
                ChooseClass(nomor);
            });
        }
    }
    private void ChooseClass(int _index)
    {
        switch(_index)
        {
            case 3:
                QuizDataManager.Instance.SetCurrentClass(QuizData.Kelas_Enum.Kelas3);
                break;
            case 4:
                QuizDataManager.Instance.SetCurrentClass(QuizData.Kelas_Enum.Kelas4);
                break;
            case 5:
                QuizDataManager.Instance.SetCurrentClass(QuizData.Kelas_Enum.Kelas5);
                break;
            case 6:
                QuizDataManager.Instance.SetCurrentClass(QuizData.Kelas_Enum.Kelas6);
                break;
        }
        QuizDataManager.Instance.SetCurrentSemester(QuizData.Semester_Enum.Semester1);

        HideAllMenus();
        MainTopicMenu.SetActive(true);
        LoadingScene.SetActive(true);
        DisplayMainTopicSemesterTitle();
        DisplayMainTopicSemesterTitle();
        switch (QuizDataManager.Instance.GetCurrentQuizKategoriSoal())
        {
            case QuizData.KategoriSoal_Enum.Study:
                DisplayMainTopic(QuizDataManager.Instance.GetListTopik(), 0);
                break;
            case QuizData.KategoriSoal_Enum.Treasure:
                DisplayMainTopic(QuizDataManager.Instance.GetListTopik(), 1);
                break;
        }
        StartCoroutine(APIManager.Instance.GetSoalList(
            QuizDataManager.Instance.GetCurrentQuizKategoriSoal().ToString(),
            QuizDataManager.Instance.GetCurrentClass().ToString(),
            null,
        null, null));
        StartCoroutine(Loading(false));
    }
    #endregion

    #region MAINTOPIC
    private void DisplayMainTopicSemesterTitle()
    {
        switch (QuizDataManager.Instance.GetCurrentSemester())
        {
            case QuizData.Semester_Enum.Semester1:
                MainTopicTitle.text = "SEMESTER 1";
                NextSemesterBtn.gameObject.SetActive(true);
                PrevSemesterBtn.gameObject.SetActive(false);
                break;
            case QuizData.Semester_Enum.Semester2:
                MainTopicTitle.text = "SEMESTER 2";
                NextSemesterBtn.gameObject.SetActive(false);
                PrevSemesterBtn.gameObject.SetActive(true);
                break;
        }
    }
    
    private bool Check_If_Study_Done_For_Treasure(int studyRoomIndex, int mainTopicIndex)
    {
        int unlock = 0;
        //List<QuizData> temp = QuizDataManager.Instance.StudyRoomQuizList[studyRoomIndex].SubQuizDataList[mainTopicIndex].QuizDataList;
        //temp.ForEach(qd =>
        //{
        //    if (qd.HasDoneFinalQuestion) { unlock++; }
        //});

        if (unlock >= 5) return true;
        else return false;
    }

    private void DisplayMainTopic(List<QuizDataManager.ListTopik> _TopikList, int id)
    {
        if (MainTopicPrefabList.Count == 0)
        {
            int nomor = 0;
            _TopikList.ForEach(x => {
                nomor++;
                Button spawnMainTopic = Instantiate(MainTopicBtnPrefab, MainTopicLayoutContainer).GetComponent<Button>();
                spawnMainTopic.GetComponent<TextMeshProUGUI>().text = "Topik " + nomor + " : " + x.topik;

                spawnMainTopic.onClick.AddListener(delegate
                {
                    StartCoroutine(ChooseMainTopic(x.id));
                });

                // if(id==1)spawnMainTopic.interactable = Check_If_Study_Done_For_Treasure(studyRoomIndex, mainTopicIndex);
                MainTopicPrefabList.Add(spawnMainTopic);
            });
        }
        else
        {
            for(int i = 0; i < MainTopicPrefabList.Count; i++)
            {
                MainTopicPrefabList[i].gameObject.SetActive(false);
            }

            int nomor = 0, j =0;

            _TopikList.ForEach(x => {
                nomor = j + 1;
                if (j >= MainTopicPrefabList.Count)
                {
                    Button spawnMainTopic = Instantiate(MainTopicBtnPrefab, MainTopicLayoutContainer).GetComponent<Button>();
                    MainTopicPrefabList.Add(spawnMainTopic);
                }
                MainTopicPrefabList[j].gameObject.SetActive(true);
                MainTopicPrefabList[j].GetComponent<TextMeshProUGUI>().text = "Topik " + nomor + " : " + x.topik;
                
                MainTopicPrefabList[j].onClick.RemoveAllListeners();
                MainTopicPrefabList[j].onClick.AddListener(delegate
                {
                    StartCoroutine(ChooseMainTopic(x.id));
                });
                // if (id == 1) MainTopicPrefabList[j].interactable = Check_If_Study_Done_For_Treasure(studyRoomIndex, mainTopicIndex);
                j++;
            });
        }
    }

    private IEnumerator ChooseMainTopic(string _topik)
    {
        if (_topik.Equals(QuizDataManager.Instance.GetCurrentTopik()) && QuizDataLoader.Instance.GetQuizData(0).KategoriSoal.Equals(QuizDataManager.Instance.GetCurrentQuizKategoriSoal()))
        {
            HideAllMenus();
            SubTopicMenu.SetActive(true);
            switch (QuizDataManager.Instance.GetCurrentQuizKategoriSoal())
            {
                case QuizData.KategoriSoal_Enum.Study:
                    DisplaySubTopic(QuizData.KategoriSoal_Enum.Study, StudyRoomSubTopicPrefabList);
                    break;
                case QuizData.KategoriSoal_Enum.Treasure:
                    DisplaySubTopic(QuizData.KategoriSoal_Enum.Treasure, StudyRoomSubTopicPrefabList);
                    break;
            }
        }
        else
        {
            QuizDataManager.Instance.SetCurrentTopik(_topik);

            int max = 0;
            switch (QuizDataManager.Instance.GetCurrentQuizKategoriSoal())
            {
                case QuizData.KategoriSoal_Enum.Study:
                    max = 5;
                    break;
                case QuizData.KategoriSoal_Enum.Treasure:
                    max = 4;
                    break;
            }

            LoadingScene.SetActive(true);

            QuizDataLoader.Instance.LoadSoal((QuizData.Paket_Enum)0);
            yield return Loading(true, 2, 0);
            
            for (int i = 1; i < 2; i++)
            {
                if (APIRequests.sukses.Equals(true))
                {
                    int num = i + 1;
                    QuizDataLoader.Instance.LoadSoal((QuizData.Paket_Enum)i);
                    yield return Loading(true, 2, i);
                }
            }

            HideAllMenus();
            SubTopicMenu.SetActive(true);
            switch (QuizDataManager.Instance.GetCurrentQuizKategoriSoal())
            {
                case QuizData.KategoriSoal_Enum.Study:
                    DisplaySubTopic(QuizData.KategoriSoal_Enum.Study, StudyRoomSubTopicPrefabList);
                    break;
                case QuizData.KategoriSoal_Enum.Treasure:
                    DisplaySubTopic(QuizData.KategoriSoal_Enum.Treasure, StudyRoomSubTopicPrefabList);
                    break;
            }
            LoadingScene.SetActive(false);
        }
        
    }
    #endregion

    #region SUBTOPIC
    private void DisplaySubTopic(QuizData.KategoriSoal_Enum _roomType, List<GameObject> _quizUIList)
    {
        if (_quizUIList.Count == 0)
        {
            SubTopicTitle.text = "";

            List<QuizData> qd = QuizDataLoader.Instance.GetQuizDatas();

            for (int k = 0; k < qd.Count; k++)
            {
                GameObject spawnSubTopic = null;
                if (_roomType == QuizData.KategoriSoal_Enum.Study)
                {
                    spawnSubTopic = Instantiate(StudyRoomSubTopicUIPrefab, SubTopicLayoutContainer);
                    StudyRoomQuizSubTopicUI subTopicUI = spawnSubTopic.GetComponent<StudyRoomQuizSubTopicUI>();
                    UpdateStudyRoomSubTopicUI(k, qd[k], subTopicUI);
                }
                else
                {
                    spawnSubTopic = Instantiate(TreasureRoomSubTopicUIPrefab, SubTopicLayoutContainer);
                    TreasureRoomQuizSubTopicUI subTopicUI = spawnSubTopic.GetComponent<TreasureRoomQuizSubTopicUI>();
                    UpdateTreasureRoomSubTopicUI(k, qd[k], subTopicUI);
                }
                _quizUIList.Add(spawnSubTopic);
                if (_roomType == QuizData.KategoriSoal_Enum.Treasure && k.Equals(3)) return;
            }
        }
        else
        {
            SubTopicTitle.text = "";
            List<QuizData> qd = QuizDataLoader.Instance.GetQuizDatas();

            for (int k = 0; k < qd.Count; k++)
            {
                if (_roomType == QuizData.KategoriSoal_Enum.Study)
                {
                    StudyRoomQuizSubTopicUI subTopicUI = _quizUIList[k].GetComponent<StudyRoomQuizSubTopicUI>();
                    UpdateStudyRoomSubTopicUI(k, qd[k], subTopicUI);
                }
                else
                {
                    TreasureRoomQuizSubTopicUI subTopicUI = _quizUIList[k].GetComponent<TreasureRoomQuizSubTopicUI>();
                    UpdateTreasureRoomSubTopicUI(k, qd[k], subTopicUI);
                }
                if (_roomType == QuizData.KategoriSoal_Enum.Treasure && k.Equals(3)) return;
            }
        }
    }

    private void UpdateTreasureRoomSubTopicUI(int index, QuizData qd, TreasureRoomQuizSubTopicUI subTopicUI)
    {
        int num = 1 + index;
        subTopicUI.SubTopicTitleTxt.text = "Subtopik : " + num;

        for (int l = 0; l < qd.CorrectAnswerList.Count - 1; l++)
        {
            if (qd.CorrectAnswerList[l] == -1)
            {
                subTopicUI.CorrectAnswerProgressList[l].sprite = DefaultQuestionProgressionSprite;
            }
            else if (qd.CorrectAnswerList[l] == 1)
            {
                subTopicUI.CorrectAnswerProgressList[l].sprite = CorrectQuestionProgressionSprite;
            }
            else if (qd.CorrectAnswerList[l] == 0)
            {
                subTopicUI.CorrectAnswerProgressList[l].sprite = IncorrectQuestionProgressionSprite;
            }
        }

        subTopicUI.UnlockedImg.enabled = false;
        subTopicUI.LockImg.enabled = false;

        if (qd.HasEarnedFinalQuestionReward == 1)
        {
            subTopicUI.ChestOpenImg.enabled = true;
            subTopicUI.ChestCloseImg.enabled = false;
        }
        else
        {
            subTopicUI.ChestOpenImg.enabled = false;
            subTopicUI.ChestCloseImg.enabled = true;
        }
        subTopicUI.ChooseSubTopicBtn.onClick.AddListener(delegate
        {
            ChooseSubTopic((QuizData.Paket_Enum)index);
        });
    }

    private void UpdateStudyRoomSubTopicUI(int index, QuizData qd, StudyRoomQuizSubTopicUI subTopicUI)
    {
        int num = 1 + index;
        subTopicUI.SubTopicTitleTxt.text = "Subtopik : " + num;

        for (int l = 0; l < subTopicUI.CorrectAnswerProgressList.Count; l++)
        {
            subTopicUI.CorrectAnswerProgressList[l].sprite = DefaultQuestionProgressionSprite;
        }
        for (int l = 0; l < qd.TotalCorrectAnswers; l++)
        {
            subTopicUI.CorrectAnswerProgressList[l].sprite = CorrectQuestionProgressionSprite;
        }
        if (qd.TotalCorrectAnswers >= 5)
        {
            subTopicUI.UnlockedImg.enabled = true;
            subTopicUI.LockImg.enabled = false;
        }
        else
        {
            subTopicUI.UnlockedImg.enabled = false;
            subTopicUI.LockImg.enabled = true;
        }
        subTopicUI.ChooseSubTopicBtn.onClick.AddListener(delegate
        {
            ChooseSubTopic((QuizData.Paket_Enum)index);
        });
    }

    private void ChooseSubTopic(QuizData.Paket_Enum _paket)
    {
        QuizDataManager.Instance.SetCurrentPaket(_paket);
        SceneManager.LoadScene(QuestionSceneName);
    }
    #endregion

    #region LOADING
    IEnumerator Loading(bool ManyTimes = false, float max = 0, float proggress = 0)
    {
        if (max.Equals(0) | max.Equals(1))
        {
            LoadingSlider.value = 0.0f;
            int time = 0;
            while (time < 2)
            {
                yield return new WaitForSeconds(1);
                LoadingSlider.value += 0.25f;
                time++;
            }

            while (APIRequests.sukses.Equals(false)) { yield return null; }

            time = 0;
            while (time < 2)
            {
                yield return new WaitForSeconds(1);
                LoadingSlider.value += 0.25f;
                time++;
            }
        }
        else
        {
            LoadingSlider.value = proggress / max;
            proggress++;

            yield return new WaitForSeconds(4);

            LoadingSlider.value = proggress / max;

            while (APIRequests.sukses.Equals(false)) { yield return null; }
        }
        LoadingScene.SetActive(ManyTimes);
    }
    #endregion
}

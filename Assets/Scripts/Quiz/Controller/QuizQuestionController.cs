using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizQuestionController : MonoBehaviour
{
    [Header("GENERAL SECTION")]
    [Space(5)]
    public GameObject QuizUIContainer;

    [Header("INTRO SECTION")]
    [Space(5)]
    public VideoPlayer QuizIntroPlayer;
    public GameObject IntroImageContainer;
    public List<Image> IntroImageList;
    public Button btnChangeIntro_Forward;
    public Button btnChangeIntro_Backward;

    [Header("QUESTION SECTION")]
    [Space(5)]
    public TextMeshProUGUI QuestionLevelTxt;
    public GameObject QuestionBorderContainer;
    public List<Image> QuestionImageList;
    public List<Button> QuestionChoiceList;

    [Header("EXPLANATION SECTION")]
    [Space(5)]
    public GameObject ExplanationBorderContainer;
    public List<Image> ExplanationImageList;
    public List<Image> ExplanationChoiceList;
    public Button CloseExplanationBtn;

    [Header("PROGRESSION SECTION")]
    [Space(5)]
    public StudyRoomQuizSubTopicUI studyRoomQuizProgressUI;
    public TreasureRoomQuizSubTopicUI treasureRoomQuizProgressUI;
    public Sprite DefaultQuestionProgressionSprite;
    public Sprite CorrectQuestionProgressionSprite;
    public Sprite IncorrectQuestionProgressionSprite;
    public int PlayerQuizProgress;

    [Header("RESULT SECTION")]
    [Space(5)]
    public GameObject ResultContainer;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI TotalCorrectAnswersTxt;
    public TextMeshProUGUI TotalIncorrectAnswersTxt;
    public TextMeshProUGUI BonusQuizRequreiementTxt;
    public TextMeshProUGUI Nilai;
    public Button ClaimRewardBtn;
    public Button TryAgainBtn;
    public Button BonusQuizBtn;
    public Button BackToMenuBtn_2;

    [Header("REWARD SECTION")]
    [Space(5)]
    public GameObject RewardContainer;
    public Image RewardImage;
    public TextMeshProUGUI RewardTxt;
    public Button BackToMenuBtn;

    [Header("REWARD GEM SECTION")]
    [Space(5)]
    public GameObject RewardGemContainer;
    public Button btnContinue;

    [Header("CHOICE SECTION")]
    [Space(5)]
    public Sprite DefaultChoiceSprite;
    public Sprite CorrectChoiceSprite;
    public Sprite IncorrectChoiceSprite;

    [Header("SCENE SECTION")]
    [Space(5)]
    public string MenuScene;
    public bool StudyRoom_HasGetTheKey = false;
    public bool TreasureRooom_HasbeenDone = false;

    private string CorrectAnswer;

    private int quizTotalCorrectAnswers;
    private int quizTotalIncorrectAnswers;
    public List<int> quizCorrectAnswersList;
    private bool isAnsweringBonusQuestion;
    private bool isCorrectAnswerBonusQuestion;
    private int Index_IntroPicture;

    void Awake()
    {
        Index_IntroPicture = 0;
    }
    void Start()
    {
        InitListeners();
        switch (QuizDataManager.Instance.GetCurrentQuizKategoriSoal())
        {
            case QuizData.KategoriSoal_Enum.Study:
                Debug.Log("Study room");
                StudyRoom_HasGetTheKey = QuizDataManager.Instance.GetCurrentQuizData().HasDoneFinalQuestion;
                quizCorrectAnswersList = new List<int>();
                StartCoroutine(DisplayQuizIntro(QuizDataManager.Instance.GetCurrentQuizData()));
                for (int i = 0; i < 10; i++)
                {
                    quizCorrectAnswersList.Add(-1);
                }
                DisplayQuestionProgression(QuizData.KategoriSoal_Enum.Study, QuizDataManager.Instance.GetCurrentQuizData());
                break;
            case QuizData.KategoriSoal_Enum.Treasure:
                Debug.Log("treasure room");
                TreasureRooom_HasbeenDone = QuizDataManager.Instance.GetCurrentQuizData().HasDoneFinalQuestion;
                quizCorrectAnswersList = new List<int>();
               // StartCoroutine(DisplayQuizIntro(QuizDataManager.Instance.GetCurrentQuizData()));
                for (int i = 0; i < 11; i++)
                {
                    quizCorrectAnswersList.Add(-1);
                }
                DisplayQuestionProgression(QuizData.KategoriSoal_Enum.Treasure, QuizDataManager.Instance.GetCurrentQuizData());
                break;
        }
        QuestionBorderContainer.SetActive(true);
        ExplanationBorderContainer.SetActive(false);
        ResultContainer.SetActive(false);
        RewardContainer.SetActive(false);
        DisplayQuestion();
    }
    private void InitListeners()
    {
        QuestionLevelTxt.text = "";
        for (int i = 0; i < QuestionChoiceList.Count; i++)
        {
            int index = i;
            QuestionChoiceList[i].onClick.AddListener(delegate
            {
                AudioManager.Instance.PlaySFX(AudioManager.SFXType.ButtonClick);
                switch (QuizDataManager.Instance.GetCurrentQuizKategoriSoal())
                {
                    case QuizData.KategoriSoal_Enum.Study:
                        CheckAnswer(QuestionChoiceList[index].gameObject.name, QuizDataManager.Instance.GetCurrentQuizData());
                        break;
                    case QuizData.KategoriSoal_Enum.Treasure:
                        CheckAnswer(QuestionChoiceList[index].gameObject.name, QuizDataManager.Instance.GetCurrentQuizData());
                        break;
                }
                
            });
        }
        CloseExplanationBtn.onClick.AddListener(delegate
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFXType.ButtonClick);
            QuestionBorderContainer.SetActive(true);
            ExplanationBorderContainer.SetActive(false);
            switch (QuizDataManager.Instance.GetCurrentQuizKategoriSoal())
            {
                case QuizData.KategoriSoal_Enum.Study:
                    CheckFinalQuestion(QuizData.KategoriSoal_Enum.Study, QuizDataManager.Instance.GetCurrentQuizData());
                    DisplayQuestion();
                    break;
                case QuizData.KategoriSoal_Enum.Treasure:
                    CheckFinalQuestion(QuizData.KategoriSoal_Enum.Treasure, QuizDataManager.Instance.GetCurrentQuizData());
                    CheckBonusQuestion(QuizDataManager.Instance.GetCurrentQuizData());
                    break;
            }
        });

        ClaimRewardBtn.onClick.AddListener(delegate
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFXType.ButtonClick);
            ResultContainer.SetActive(false);
            RewardContainer.SetActive(true);
            DisplayReward();
        });
        TryAgainBtn.onClick.AddListener(delegate
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFXType.ButtonClick);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        BonusQuizBtn.onClick.AddListener(delegate
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFXType.ButtonClick);
            ResultContainer.SetActive(false);
            QuestionBorderContainer.SetActive(true);
            DisplayQuestion();
        });
        BackToMenuBtn.onClick.AddListener(delegate
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFXType.ButtonClick);
            SceneManager.LoadScene(MenuScene);
        });
        BackToMenuBtn_2.onClick.AddListener(delegate
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFXType.ButtonClick);
            SceneManager.LoadScene(MenuScene);
        });
    }

    public void SaveAnswerProgress()
    {
        if(QuizDataManager.Instance.GetCurrentQuizKategoriSoal().Equals(QuizData.KategoriSoal_Enum.Study))
        {
            QuizDataManager.Instance.GetCurrentQuizData().QuestionList.ForEach(x => {
                int a = x.IsCorrectAnswer;
                QuizDataManager.Instance.GetCurrentQuizData().CorrectAnswerList.Add(a);
            });
        }
        else
        {
            QuizDataManager.Instance.GetCurrentQuizData().QuestionList.ForEach(x => {
                int a = x.IsCorrectAnswer;
                QuizDataManager.Instance.GetCurrentQuizData().CorrectAnswerList.Add(a);
            });
        }
        
    }

    private IEnumerator DisplayQuizIntro(QuizData _quizData)
    {
        if (_quizData.Paket.Equals(QuizData.Paket_Enum.Paket_Level_1))
        {
            QuizIntroPlayer.clip = (VideoClip)_quizData.IntroReference.Asset;
            QuizUIContainer.SetActive(false);
            QuizIntroPlayer.Play();
            yield return new WaitForSeconds((float)QuizIntroPlayer.clip.length);
            QuizIntroPlayer.gameObject.SetActive(false);
            QuizUIContainer.SetActive(true);
        }
        else
        {
            QuizIntroPlayer.gameObject.SetActive(false);

            IntroImageContainer.SetActive(true);
            int index = 0;

            IntroImageList.ForEach(x => {
                try
                {
                    x.sprite = _quizData.IntroPictureList[index];
                }
                catch
                {
                    x.gameObject.SetActive(false);
                }
                index++;
            });

            btnChangeIntro_Forward.onClick.AddListener(() => {
                IntroImageList[Index_IntroPicture].gameObject.SetActive(false);
                Index_IntroPicture++;
                if (Index_IntroPicture >= _quizData.IntroPictureList.Count) IntroImageContainer.SetActive(false);
            });

            btnChangeIntro_Backward.onClick.AddListener(() => {
                Index_IntroPicture--;
                if (Index_IntroPicture < 0) Index_IntroPicture = 0;
                IntroImageList[Index_IntroPicture].gameObject.SetActive(true);
            });
        }
        
    } 
    private void DisplayQuestion()
    {
        switch (QuizDataManager.Instance.GetCurrentQuizKategoriSoal())
        {
            case QuizData.KategoriSoal_Enum.Study:
                DisplayRoomQuestion(QuizDataManager.Instance.GetCurrentQuizData());
                break;
            case QuizData.KategoriSoal_Enum.Treasure:
                DisplayRoomQuestion(QuizDataManager.Instance.GetCurrentQuizData());
                break;
        }
    } //1
    private void DisplayRoomQuestion(QuizData _quizData)
    {
        HideQuestionImageList();
        for (int i = 0; i < _quizData.QuestionList[PlayerQuizProgress].QuestionReference.Count; i++)
        {
            QuestionImageList[i].gameObject.SetActive(true);
            QuestionImageList[i].sprite = (Sprite)_quizData.QuestionList[PlayerQuizProgress].QuestionReference[i];
        }
        CorrectAnswer = _quizData.QuestionList[PlayerQuizProgress].Answer;
    } //2
    private void DisplayBonusQuestion(QuizData _quizData)
    {
        QuestionLevelTxt.text = "Bonus";
        for (int i = 0; i < _quizData.QuestionList[_quizData.QuestionList.Count - 1].QuestionReference.Count; i++)
        {
            QuestionImageList[i].gameObject.SetActive(true);
            QuestionImageList[i].sprite = (Sprite)_quizData.QuestionList[_quizData.QuestionList.Count - 1].QuestionReference[i];
        }
        CorrectAnswer = _quizData.QuestionList[_quizData.QuestionList.Count - 1].Answer;
    }
    private void DisplayQuestionProgression(QuizData.KategoriSoal_Enum _quizRoom, QuizData _quizData)
    {
        if (_quizRoom == QuizData.KategoriSoal_Enum.Study)
        {
            treasureRoomQuizProgressUI.gameObject.SetActive(false);
            
            if (quizTotalCorrectAnswers >= 5)
            {
                studyRoomQuizProgressUI.LockImg.enabled = false;
                studyRoomQuizProgressUI.UnlockedImg.enabled = true;

                for (int i = 0; i < studyRoomQuizProgressUI.CorrectAnswerProgressList.Count; i++)
                {
                    studyRoomQuizProgressUI.CorrectAnswerProgressList[i].sprite = CorrectQuestionProgressionSprite;
                }
            }
            else
            {
                studyRoomQuizProgressUI.LockImg.enabled = true;
                studyRoomQuizProgressUI.UnlockedImg.enabled = false;

                int temp = _quizData.QuestionList.Count - quizTotalIncorrectAnswers;
                int temp3 = 0, temp2 = _quizData.QuestionList.Count - quizTotalIncorrectAnswers;

                for (int i = 0; i < studyRoomQuizProgressUI.CorrectAnswerProgressList.Count; i++)
                {
                    studyRoomQuizProgressUI.CorrectAnswerProgressList[i].sprite = DefaultQuestionProgressionSprite;
                    if (temp3 < quizTotalCorrectAnswers)
                    {
                        studyRoomQuizProgressUI.CorrectAnswerProgressList[i].sprite = CorrectQuestionProgressionSprite;
                        temp3++;
                    }
                }

                if (quizTotalIncorrectAnswers >= 6)
                {
                    for (int i = (studyRoomQuizProgressUI.CorrectAnswerProgressList.Count - 1); i >= temp2; i--)
                    {
                        studyRoomQuizProgressUI.CorrectAnswerProgressList[i].sprite = IncorrectQuestionProgressionSprite;
                    }
                }

            }
        }
        else
        {
            studyRoomQuizProgressUI.gameObject.SetActive(false);
            for (int i = 0; i < _quizData.QuestionList.Count - 1; i++)
            {
                if (quizCorrectAnswersList[i] == -1)
                {
                    treasureRoomQuizProgressUI.CorrectAnswerProgressList[i].sprite = DefaultQuestionProgressionSprite;
                }
                else if (quizCorrectAnswersList[i] == 1)
                {
                    treasureRoomQuizProgressUI.CorrectAnswerProgressList[i].sprite = CorrectQuestionProgressionSprite;
                }
                else if (quizCorrectAnswersList[i] == 0)
                {
                    treasureRoomQuizProgressUI.CorrectAnswerProgressList[i].sprite = IncorrectQuestionProgressionSprite;
                }
            }

            treasureRoomQuizProgressUI.UnlockedImg.enabled = false;
            treasureRoomQuizProgressUI.LockImg.enabled = false;

            OpenClosedChest(_quizData);
        }
    }

    private void CheckAnswer(string _playerAnswer, QuizData _quizData)
    {

        if (_playerAnswer == CorrectAnswer)
        {
            if (isAnsweringBonusQuestion)
            {
                quizCorrectAnswersList[_quizData.QuestionList.Count - 1] = 1;
                isCorrectAnswerBonusQuestion = true;
                RewardGemContainer.SetActive(true);
                CurrencyManager.Instance.IncrementGem(1);
            }
            else 
            {
                quizTotalCorrectAnswers++;
                Debug.Log(quizTotalCorrectAnswers + " JJ");
                quizCorrectAnswersList[PlayerQuizProgress] = 1;

                //add boss key
                if (QuizDataManager.Instance.GetCurrentQuizKategoriSoal() == QuizData.KategoriSoal_Enum.Study) //get key only from study room
                {
                    if (quizTotalCorrectAnswers >= 5 && !StudyRoom_HasGetTheKey)
                    {
                        StudyRoom_HasGetTheKey = true;
                        PlayerDataManager.Instance.AddKey(1);
                    }
                }
            }
            DisplayQuestionProgression(QuizDataManager.Instance.GetCurrentQuizKategoriSoal(), _quizData);
        }
        else
        {
            if (isAnsweringBonusQuestion)
            {
                quizCorrectAnswersList[_quizData.QuestionList.Count - 1] = 0;
                isCorrectAnswerBonusQuestion = false;
            }
            else
            {
                quizTotalIncorrectAnswers++;
                quizCorrectAnswersList[PlayerQuizProgress] = 0;
                if (!QuizDataManager.Instance.GetCurrentQuizData().HasDoneFinalQuestion)
                {
                    QuizDataManager.Instance.CalculateTotalCorrectAnswers(_quizData);
                }
               
            }
            DisplayQuestionProgression(QuizDataManager.Instance.GetCurrentQuizKategoriSoal(), _quizData);
        }

        QuestionBorderContainer.SetActive(false);

        if (isCorrectAnswerBonusQuestion && _quizData.HasAnsweredFinalQuestion == 0)
        {
            btnContinue.onClick.AddListener(delegate {
                RewardGemContainer.SetActive(false);
                ExplanationBorderContainer.SetActive(true);
                if (isAnsweringBonusQuestion)
                {
                    _quizData.HasAnsweredFinalQuestion = 1;
                    PlayerQuizProgress--;
                    isAnsweringBonusQuestion = false;
                    DisplayBonusQuestionExplanation(_quizData);
                }
                else
                {
                    DisplayExplanation(_quizData);
                }
                DisplayExplanationChoiceImageList(_playerAnswer);
            });
        }
        else
        {
            ExplanationBorderContainer.SetActive(true);
            if (isAnsweringBonusQuestion)
            {
                _quizData.HasAnsweredFinalQuestion = 1;
                PlayerQuizProgress--;
                isAnsweringBonusQuestion = false;
                DisplayBonusQuestionExplanation(_quizData);
            }
            else
            {
                DisplayExplanation(_quizData);
            }
            DisplayExplanationChoiceImageList(_playerAnswer);
        }
       
    } //3

    private void OpenClosedChest(QuizData _quizData)
    {
        if (_quizData.HasEarnedFinalQuestionReward == 1)
        {
            treasureRoomQuizProgressUI.ChestOpenImg.enabled = true;
            treasureRoomQuizProgressUI.ChestCloseImg.enabled = false;
        }
        else
        {
            treasureRoomQuizProgressUI.ChestOpenImg.enabled = false;
            treasureRoomQuizProgressUI.ChestCloseImg.enabled = true;
        }
    }

    private void CheckBonusQuestion(QuizData _quizData)
    {
        if (quizTotalCorrectAnswers >= QuizDataManager.Instance.TotalCorrectAnswersToGetReward && _quizData.HasAnsweredFinalQuestion.Equals(0))
        {
            isAnsweringBonusQuestion = true;
            DisplayBonusQuestion(_quizData);
            _quizData.HasEarnedFinalQuestionReward = 1;
            OpenClosedChest(_quizData);
        }
        else
        {
            DisplayQuestion();
        }
    } //8
    private void CheckFinalQuestion(QuizData.KategoriSoal_Enum _quizRoom, QuizData _quizData)
    {

        if (_quizRoom == QuizData.KategoriSoal_Enum.Study)
        {
            if (quizTotalCorrectAnswers >= 5 || PlayerQuizProgress >= (_quizData.QuestionList.Count - 1))
            {
                //Display result
                DisplayResult(_quizRoom, _quizData);

                //Save data
                if (quizTotalCorrectAnswers >= _quizData.TotalCorrectAnswers && !_quizData.HasDoneFinalQuestion)
                {
                    Debug.Log("HELP");
                    for (int i = 0; i < _quizData.QuestionList.Count; i++)
                    {
                        _quizData.QuestionList[i].IsCorrectAnswer = quizCorrectAnswersList[i];
                    }
                    QuizDataManager.Instance.CalculateTotalCorrectAnswers(_quizData);
                }
            }
            else NextProgress();
        }
        else
        {
            if (PlayerQuizProgress >= (_quizData.QuestionList.Count - 2))
            {
                DisplayResult(_quizRoom, _quizData);

                if (quizTotalCorrectAnswers >= _quizData.TotalCorrectAnswers && !TreasureRooom_HasbeenDone)
                {
                    for (int i = 0; i < _quizData.QuestionList.Count; i++)
                    {
                        _quizData.QuestionList[i].IsCorrectAnswer = quizCorrectAnswersList[i];
                    }
                    QuizDataManager.Instance.CalculateTotalCorrectAnswers(_quizData);
                }
                
            }
            else NextProgress();
        }
    } //7

    private void NextProgress()
    {
        PlayerQuizProgress++;
        QuestionLevelTxt.text = "";
    }

    private void HideQuestionImageList()
    {
        for(int i = 0; i < QuestionImageList.Count; i++)
        {
            QuestionImageList[i].gameObject.SetActive(false);
        }
    }

    private void DisplayExplanation(QuizData _quizData)
    {
        HideExplanationImageList();
        for (int i = 0; i < _quizData.QuestionList[PlayerQuizProgress].ExplanationReference.Count; i++)
        {
            ExplanationImageList[i].gameObject.SetActive(true);
            ExplanationImageList[i].sprite = (Sprite)_quizData.QuestionList[PlayerQuizProgress].ExplanationReference[i];
        }
    }  // 4
    private void DisplayBonusQuestionExplanation(QuizData _quizData)
    {
        HideExplanationImageList();
        for (int i = 0; i < _quizData.QuestionList[_quizData.QuestionList.Count - 1].ExplanationReference.Count; i++)
        {
            ExplanationImageList[i].gameObject.SetActive(true);
            ExplanationImageList[i].sprite = (Sprite)_quizData.QuestionList[_quizData.QuestionList.Count - 1].ExplanationReference[i];
        }
    }
    private void DisplayExplanationChoiceImageList(string _playerAnswer)
    {
        for (int i = 0; i < ExplanationChoiceList.Count; i++)
        {
            ExplanationChoiceList[i].sprite = DefaultChoiceSprite;
            if (_playerAnswer == ExplanationChoiceList[i].name)
            {
                if (_playerAnswer == CorrectAnswer)
                {
                    ExplanationChoiceList[i].sprite = CorrectChoiceSprite;
                }
                else
                {
                    ExplanationChoiceList[i].sprite = IncorrectChoiceSprite;
                }
            }
            if (ExplanationChoiceList[i].name == CorrectAnswer)
            {
                ExplanationChoiceList[i].sprite = CorrectChoiceSprite;
            }
        }
    } //6
    private void HideExplanationImageList()
    {
        for (int i = 0; i < ExplanationImageList.Count; i++)
        {
            ExplanationImageList[i].gameObject.SetActive(false);
        }
    } //5
    private void DisplayResult(QuizData.KategoriSoal_Enum _quizRoom, QuizData _quizData)
    {
        ExplanationBorderContainer.SetActive(false);
        ResultContainer.SetActive(true);
        TotalCorrectAnswersTxt.text = quizTotalCorrectAnswers.ToString();
        TotalIncorrectAnswersTxt.text = quizTotalIncorrectAnswers.ToString();
        if (_quizRoom == QuizData.KategoriSoal_Enum.Study)
        {
            BonusQuizRequreiementTxt.gameObject.SetActive(false);
            BonusQuizBtn.gameObject.SetActive(false);
            ClaimRewardBtn.gameObject.SetActive(true);
            Nilai.gameObject.SetActive(true);
            if (quizTotalCorrectAnswers == 0)
            {
                Title.text = "MAAF";
                ClaimRewardBtn.gameObject.SetActive(false);
                TryAgainBtn.gameObject.SetActive(true);
                BackToMenuBtn_2.gameObject.SetActive(true);
            }
            else if (quizTotalIncorrectAnswers >=6)
            {
                TryAgainBtn.gameObject.SetActive(true);
            }
        }
        else
        {
            BonusQuizRequreiementTxt.gameObject.SetActive(true);
            BonusQuizRequreiementTxt.text = string.Format(BonusQuizRequreiementTxt.text, QuizDataManager.Instance.TotalCorrectAnswersToGetReward);
            Nilai.gameObject.SetActive(true);
            BonusQuizBtn.gameObject.SetActive(false);
            ClaimRewardBtn.gameObject.SetActive(true);

        }
    }
    //ngitung score bangsat blm selesai
    private void DisplayReward()
    {
        if (QuizDataManager.Instance.GetCurrentQuizKategoriSoal() == QuizData.KategoriSoal_Enum.Study)
        {
            RewardImage.sprite = CurrencyManager.Instance.CoinCurrencyIcon;
            
            float hadiah = 0;
            int divider = PlayerQuizProgress + 1;

            float Mark =(float)quizTotalCorrectAnswers / divider;
            Mark = (float)Mark * QuizDataManager.Instance.Multiplier_Mark_StudyRoom;

            if (!QuizDataManager.Instance.GetCurrentQuizData().HasDoneFinalQuestion)
            {
                SaveAnswerProgress();
                QuizDataManager.Instance.GetCurrentQuizData().Mark = Mathf.RoundToInt(Mark);
                hadiah = Mark;
            }
            else
            {
                hadiah = (float)Mark * QuizDataManager.Instance.Multiplier_Reward_StudyRoom;
            }

            if (StudyRoom_HasGetTheKey) QuizDataManager.Instance.GetCurrentQuizData().HasDoneFinalQuestion = true;

            RewardTxt.text = "x" + Mathf.RoundToInt(hadiah);
            Nilai.text = "" + Mark;
            CurrencyManager.Instance.IncrementCoin(Mathf.RoundToInt(hadiah));
        }
        else
        {
            RewardImage.sprite = CurrencyManager.Instance.DiamondCurrencyIcon;
            RewardImage.color = Color.cyan;

            float Mark = (float)quizTotalCorrectAnswers * QuizDataManager.Instance.Multiplier_Mark_TreasureRoom;
            float hadiah = (float)quizTotalCorrectAnswers * QuizDataManager.Instance.Multiplier_Reward_TreasureRoom;

            QuizDataManager.Instance.treasureRoomMarks.ForEach(x =>
            {
                if (quizTotalCorrectAnswers.Equals(x.mark)) Mark = x.reward;
            });

            if (!QuizDataManager.Instance.GetCurrentQuizData().HasDoneFinalQuestion)
            {
                SaveAnswerProgress();
                QuizDataManager.Instance.GetCurrentQuizData().Mark = Mathf.RoundToInt(Mark);
            }
            else
            {
                hadiah = (float)hadiah / 2;
            }

            RewardTxt.text = RewardTxt.text = "x" + Mathf.RoundToInt(hadiah);
            Nilai.text = "" + Mark;
            QuizDataManager.Instance.GetCurrentQuizData().HasDoneFinalQuestion = true;
            CurrencyManager.Instance.IncrementDiamond(Mathf.RoundToInt(hadiah));
        }
    }

}

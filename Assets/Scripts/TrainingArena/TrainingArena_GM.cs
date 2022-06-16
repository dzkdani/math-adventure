using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class TrainingArena_GM : MonoBehaviour
{
    public TrainingArena_PlayerController PlayerController;

    public TerrainManager TerrainManager;
    public QuizManager quizManager;
    public TextMeshProUGUI UI_SetTerrainUsed;

    public Transform Kamera;

    public Transform RambuTandaSeru;
    bool isRambuTandaSeruOn;

    public GameObject BaseGame, UI_Main;
    public Slider UI_SliderEnergy;
    public TextMeshProUGUI UI_TextCurrentEnergy, UI_TextCurrentLevel, UI_TextCurrentScore;
    static int _newTotalPlayerEnergy = 0;

    public GameObject QuizDifficultySelection; //Kuiz;

    public GameObject UI_DisplayFinalScore;
    public TextMeshProUGUI UI_TextFinalScore;

    public bool PauseGame = false;
    public GameObject UI_PauseMenu;

    public Image BlackFilm;

    public GameObject canvasTutorial;
    public List<GameObject> tutorial;
    public List<GameObject> tutorial2;
    public List<GameObject> tutorial3;
    public List<GameObject> tutorial4;
    public List<GameObject> tutorial5;
    public List<GameObject> tutorial6;
    public Button nextTutorial;
    public int indexTutorial = 0;
    int _indexTutorial;

    bool StopLevelFromIncreasing = false, CountOnce = false, hasScoreBeenSaved = false;

    public void Restart()
    {
        SceneManager.LoadScene("true");
    }

    void Tutorial(List<GameObject> temp_tutor, int index = 0)
    {
        bool temp = TrainingArenaSettingManager.Instance.isTutorialOn;
        _indexTutorial = 0;
        nextTutorial.onClick.RemoveAllListeners();
        if (temp == true)
        {
            nextTutorial.onClick.AddListener(() => {
                try { NextTutorial(temp_tutor[_indexTutorial], temp_tutor[_indexTutorial + 1], temp_tutor.Capacity); }
                catch { NextTutorial(temp_tutor[_indexTutorial], null,temp_tutor.Capacity); }
            });

            if (index.Equals(1))
            {
                UI_SliderEnergy.gameObject.SetActive(false);
                UI_TextCurrentLevel.transform.parent.gameObject.SetActive(false);
            }
            else if (index.Equals(2))
            {
                UI_SliderEnergy.gameObject.SetActive(true);
                UI_TextCurrentLevel.transform.parent.gameObject.SetActive(true);
            }

            else if (index.Equals(6))
            {
                nextTutorial.gameObject.SetActive(false);

            }

            canvasTutorial.SetActive(true);
           // PlayerController.isEnergyStartDecreasing = false;
            temp_tutor[0].SetActive(true);
        }
       // else if(temp != true) PlayerController.isEnergyStartDecreasing = true;
    }

    public void StartTutorial() {
        
        bool temp = TrainingArenaSettingManager.Instance.isTutorialOn;
        if (temp == true)
        {
            PauseGame = true;
            AudioListener.volume = 0;
            indexTutorial++;
            switch (indexTutorial)
            {
                case 1:
                    Tutorial(tutorial, indexTutorial);
                    break;
                case 2:
                    Tutorial(tutorial2, indexTutorial);
                    break;
                case 3:
                    Tutorial(tutorial3, indexTutorial);
                    break;
                case 4:
                    Tutorial(tutorial4, indexTutorial);
                    break;
                case 5:
                    Tutorial(tutorial5, indexTutorial);
                    break;
                case 6:
                    Tutorial(tutorial6, indexTutorial);
                    break;
            }
        }
        //else if (temp != true) PlayerController.isEnergyStartDecreasing = true;
    }

    public void NextTutorial(GameObject papan_before, GameObject papan_after, int capacity)
    {
        bool temp = TrainingArenaSettingManager.Instance.isTutorialOn;
        papan_before.SetActive(false);
        _indexTutorial += 1;
        if (_indexTutorial < capacity) papan_after.SetActive(true);
        else { papan_before.SetActive(false);  canvasTutorial.SetActive(false); _indexTutorial = 0; PauseGame = false; AudioListener.volume = 1; }
    }

    void UserInterfaceDisplay()
    {
        //score
        if (PlayerController.Score <= 0)
        {
            PlayerController.Score = 0;
            UI_TextCurrentScore.text = "";
        }
        else { UI_TextCurrentScore.text = "" +PlayerController.Score; }

        //energy
        if (PlayerController.Energy <= 0)
        {
            PlayerController.Energy = 0;
            UI_SliderEnergy.value = 0;

            PlayerController.SoundController.Run.Stop();
            PlayerController.gameObject.SetActive(false);

            BaseGame.SetActive(false);
            UI_Main.SetActive(false);

            UI_DisplayFinalScore.SetActive(true);

            UI_TextFinalScore.SetText("{0}", PlayerController.Score);

            if (!hasScoreBeenSaved)
            {
                SaveScore(PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentName);
                hasScoreBeenSaved = true;
            }

            UI_DisplayFinalScore.GetComponent<CanvasGroup>().DOFade(1, 1).OnStart(() => { CalculatingCardPrize(); });
        }
        else if (PlayerController.Energy <= 60)
        {
            UI_SliderEnergy.value = PlayerController.Energy;
        }
        else UI_SliderEnergy.value = UI_SliderEnergy.maxValue;
        UI_TextCurrentEnergy.text = "" + PlayerController.Energy;

        int level = quizManager.levelling.LevelValue;
        int exp = quizManager.levelling.Exp;

        if (level >= 26) { StopLevelFromIncreasing = true; }

        if (StopLevelFromIncreasing) { UI_TextCurrentLevel.text = "26"; }
        else UI_TextCurrentLevel.text = "" + level;

        if (TrainingArenaSettingManager.Instance.isShowSetName) { UI_SetTerrainUsed.SetText(PlayerController.SpawnedSetName); }
        
        RambuTandaSeruStartBlinking();
    }

    public void SaveScore(string sd)
    {
        PlayerPrefs.SetInt("PlayerScore",PlayerController.Score);
    }

    public void SaveLevel()
    {
       HasilBelajarManager.Instance.SetLevel(quizManager.levelling.LevelValue, quizManager.levelling.Exp);
    }

    public void SaveProgressHasilBelajar()
    {
        BlackFilm.gameObject.SetActive(true);
        BlackFilm.DOFade(1, 0.5f).OnComplete(() => {
            StartCoroutine(Ya____SaveProgressHasilBelajar());
        });
    }

    IEnumerator Ya____SaveProgressHasilBelajar()
    {
        HasilBelajarManager.Instance.SaveProgress(quizManager.levelling.LevelValue, quizManager.levelling.Exp);
        PlayerDataManager.Instance.UpdatePlayerCurrency();
        yield return new WaitForSeconds(5);
        BackToMainMenu();
    }

    #region PAUSE_MENU
    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        PauseGame = false;
        AudioListener.pause = false;
        BtnBackManager.instance.W = null;
        SceneManager.LoadScene("GameMenu");
    }

    public void pause(bool pause)
    {
        // true = game is pause, false = game is not pause
        if (!UI_DisplayFinalScore.activeSelf && !CardDeck.gameObject.activeSelf)
        {
            AudioListener.pause = pause;
            UI_Main.SetActive(!pause);
            UI_PauseMenu.SetActive(pause);
            PauseGame = pause;
        }
    }
    #endregion

    #region QUIZ
    IEnumerator WaitOneSec()
    {
        PlayerController.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
    }

    void RunQuiz()
    {
        _newTotalPlayerEnergy = 0;
        if (PlayerController.isQuizOn)
        {
            PlayerController.isQuizOn = false;
            PlayerController.PlayerReset();
            QuizDifficultySelection.SetActive(true);
            PlayerController.gameObject.SetActive(false);
            TimeManager.isOn = true;
            UI_Main.SetActive(false);
        }

        if (AnswerChecker.Oked)
        {
            AnswerChecker.Oked = false;
            UI_Main.SetActive(true);
            StartCoroutine(WaitOneSec());
            TimeManager.isOn = false;
            if (AnswerChecker.RightAnswer)
            {
                if (Levelling.Easy == true)
                {
                    PlayerController.Energy += 1;
                }
                else if (Levelling.Medium == true)
                {
                    PlayerController.Energy += 2;
                }
                else if (Levelling.Hard == true)
                {
                    PlayerController.Energy += 3;
                }
            }

            bool temp = TrainingArenaSettingManager.Instance.isTutorialOn;
            if (temp.Equals(false)) { SaveLevel(); }
            
        }
    }
    #endregion

    #region PLAYER_CAMERA
    public void MoveCamera()
    {
        Vector3 velocity = Vector3.zero;
        Kamera.position = Vector3.SmoothDamp(Kamera.position, PlayerController.Player.position, ref velocity, 0.1f);
        RambuTandaSeru.position = new Vector3(RambuTandaSeru.position.x, RambuTandaSeru.position.y, PlayerController.Player.position.z);
    }
    #endregion

    #region GET_CARD_TROOP
    public int getCard = 0;
    public GameObject prefab_card;
    public RectTransform CardDeck, content_deck;
    public List<troop_card_i> temp_troop_cards = new List<troop_card_i>();

    [System.Serializable]
    public struct troop_card_i
    {
        public int troop_index;
        public int total;
    }

    public void mendapatkan_kartu_gratis(int a)
    {
        bool exist = false;
        troop_card_i baru;

        for (int i = 0; i < temp_troop_cards.Count; i++)
        {
            if (temp_troop_cards[i].troop_index.Equals(a))
            {
                baru = temp_troop_cards[i];
                baru.total += 1;
                temp_troop_cards[i] = baru;
                exist = true;
                break;
            }
        }

        if (!exist)
        {
            baru.troop_index = a; baru.total = 1;
            temp_troop_cards.Add(baru);
        }
    }

    public void CalculatingCardPrize()
    {
        BeginAndEnd BAE;
        if (!CountOnce)
        {
            temp_troop_cards.ForEach(tci => { 
                TroopData td = PlayerDataManager.Instance.GetTroopInPlayerDataBasedIndex(tci.troop_index);

                bool first = false;
                PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList.ForEach(TODL => {
                    if (TODL.Name.Equals(td.Name))
                    {
                        PlayerData.UnitsOwnedData ownedData = TODL;
                        if (ownedData.IsUnlocked)
                        {
                            ownedData.DuplicateCard += tci.total;
                        }
                        else ownedData.IsUnlocked = first = true;
                        TODL = ownedData;
                    }
                });

                GameObject temp = Instantiate(prefab_card, content_deck);
                temp.transform.localScale = Vector3.one;
                BAE = temp.GetComponent<BeginAndEnd>();
                BAE.image.sprite = td.RankIcons[0];
                if (first)
                {
                    BAE.textMesh.text = "NEW (" + tci.total + ")";
                    BAE.textMesh.faceColor = Color.yellow;
                }
                else
                {
                    BAE.textMesh.text = tci.total.ToString();
                }
            });
            if (!CountOnce) CountOnce = true;
        }

        //  ScoreMenu.SetActive(false);
        //CardDeck.gameObject.SetActive(true);
    }
    #endregion

    public static void PlayerDapatEnergiTambahan(int plus, TextMeshProUGUI textMesh)
    {
        TrainingArena_PlayerController _PlayerController = TrainingArena_PlayerController.GetThisPlayerController();
        if (_newTotalPlayerEnergy == 0)
        {
            _PlayerController.Energy += plus;
            _newTotalPlayerEnergy = _PlayerController.Energy;
        }
        textMesh.text = "+" + plus;
        textMesh.DOFade(255, 1f).OnComplete(() => textMesh.DOFade(0, 1f));
    }

    public static void PlayerDapatInvisibleDenganWaktu(int plus)
    {
        TrainingArena_PlayerController _PlayerController = TrainingArena_PlayerController.GetThisPlayerController();
        _PlayerController.DurationPowerActivated = plus;
    }

    public void Debug_EnterQuiz()
    {
        PlayerController.Debug_EnterQuiz();
    }

    #region DANGER ROCKET NOTIFICATION
    string _currentRocketSequence = "";
    public void TurnOnRambuTandaSeru(string Rocket_Sequence = "")
    {
       // Debug.Log(Rocket_Sequence);
        _currentRocketSequence = Rocket_Sequence;
        isRambuTandaSeruOn = true;
    }

    int StateRocket = 500, DefaultStateTandaSeru = 0;
    public void TurnOffTandaSeru()
    {
        StateRocket = DefaultStateTandaSeru;
    }

    public void RambuTandaSeruStartBlinking()
    {
        if (isRambuTandaSeruOn)
        {
            if (StateRocket == 1)
            {
                for (int i = 0; i < RambuTandaSeru.childCount; i++)
                {
                    Transform temp = RambuTandaSeru.GetChild(i);
                    if (_currentRocketSequence[i] == '1') { temp.gameObject.SetActive(true); }
                }
            }

            if (StateRocket >= DefaultStateTandaSeru)
            {
                StateRocket = 0;
                isRambuTandaSeruOn = false;
            }
            else StateRocket++;
        }
        else
        {
            for (int i = 0; i < RambuTandaSeru.childCount; i++)
            {
                RambuTandaSeru.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region PLAYER MOVEMENT CONTROL
    public void GoUp()
    {

        if (PlayerController.CheckPowerInvisible() != 1 && PlayerController.CheckWall() != "up" && !PlayerController.isJumping)
        {
            PlayerController.SoundController.PlayerRun();
            PlayerController.SetTargetPositionVertical(1);
        }
    }

    public void GoDown()
    {
        if (PlayerController.CheckWallDown() != "down" && PlayerController.CheckPowerInvisible() != 1 && !PlayerController.isJumping)
        {
            PlayerController.SetTargetPositionVertical(-1);
        }
    }

    public void GoLeft()
    {
        if (PlayerController.CheckWall() != "left" && PlayerController.CheckPowerInvisible() != 1 && !PlayerController.isJumping)
        {
            PlayerController.SetTargetPositionHorizontal(-1);
        }
    }

    public void GoRight()
    {
        if (PlayerController.CheckWall() != "right" && PlayerController.CheckPowerInvisible() != 1 && !PlayerController.isJumping)
        {
            PlayerController.SetTargetPositionHorizontal(1);
        }
    }

    #endregion

    private void Start()
    {
        DOTween.KillAll();
        Initializing();
    }

    void Initializing()
    {
        AudioListener.volume = 0;
        bool temp = TrainingArenaSettingManager.Instance.isTutorialOn;
        quizManager.Hard();
        StartCoroutine(_Initializing(temp));
    }

    IEnumerator _Initializing(bool temp)
    {
        yield return new WaitForSeconds(4);
        quizManager.ConfirmOK();

        if (temp == true)
        {
            UI_SliderEnergy.gameObject.SetActive(false);
            UI_TextCurrentLevel.transform.parent.gameObject.SetActive(false);
        }

        BlackFilm.DOFade(0, 1).OnComplete(() => {
            if (temp == true) { PlayerController.isEnergyStartDecreasing = false; StartTutorial(); }
            else if (temp != true) { PlayerController.isEnergyStartDecreasing = true; }
            BlackFilm.gameObject.SetActive(false);
            AudioListener.volume = 1;
            AudioManager.Instance.PlaySFX(3);
            
        });
    }

    void Update()
    {
        if (PauseGame) { Time.timeScale = 0; return; }
        else { Time.timeScale = 1; }

        MoveCamera();
        UserInterfaceDisplay();
        RunQuiz();

        if (PlayerController.hasPassedCheckpoint)
        {
            /*if (!PlayerController.DontDestroyTerrainBeforeRocketTerrain) spawnTerrain.DestroyAndRespawnedTerrain();
            else { spawnTerrain.DestroyAndRespawnedTerrain(1); PlayerController.DontDestroyTerrainBeforeRocketTerrain = false; }*/
            TerrainManager.CheckSet(PlayerController.SpawnedSetName);
            PlayerController.hasPassedCheckpoint = false;
        }
        //temp_troop_cards.Add(mendapatkan_kartu_gratis);
    }

    public void TutorialTurnOnOFF(bool temp)
    {
        TrainingArenaSettingManager.Instance.isTutorialOn = temp;
    }

    public void SetTerrainTrainingArena(TerrainIDList e)
    {
        TrainingArenaSettingManager.Instance.SetList = e;
    }

    public void LoadTheGame(string temp)
    {
        SceneManager.LoadScene(temp);
    }
}

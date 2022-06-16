using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using DragonBones;

public class GameMenuController : MonoBehaviour
{
    public HeroSelectionController heroSelectionController;
    public GameObject HighScore;
    public GameObject Menu;
    public GameObject SquadPanel;
    public GameObject DebugPanel;

    public RectTransform Titik_a, Titik_b, titik_default;

    RectTransform currentScene;
    TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        OnButtonInitialized();
        AnotherStart();
    }

    public void InputName()
    {
        PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentName = inputField.text;
    }

    public void SetStudyRoom()
    {
        QuizDataManager.Instance.SetCurrentKategoriSoal(QuizData.KategoriSoal_Enum.Study);
    }
    public void SetTreasureRoom()
    {
        QuizDataManager.Instance.SetCurrentKategoriSoal(QuizData.KategoriSoal_Enum.Treasure);
    }

    public void SetTerrainTrainingArena(TerrainIDList e)
    {
        TrainingArenaSettingManager.Instance.SetList = e;
    }

    public virtual void AnotherStart() { }
    public void MoveWithTween(RectTransform Temp)
    {
        RectTransform temp2 = currentScene;
        currentScene = null;
        temp2.position = Titik_b.position;

        Temp.localScale = Vector2.zero;
        Temp.position = titik_default.position;
        Temp.DOScale(Vector3.one, 0.4f);
        currentScene = Temp;

    }

    private void Update()
    {
        Time.timeScale = 1;
        DisplayCurrency();
        AnotherUpdate();
    }
    public virtual void AnotherUpdate() { }

    public void LoadPreviousGO()
    {
        BtnBackManager.instance.LoadPreviousGO();
    }

    public void AddCurrentGOsToActiveGOs(BeforeAfterGameObject e)
    {
        BtnBackManager.instance.AddCurrentGOsToActiveGOs(e);
    }

    #region DEBUG
    public void IncreaseCoin(InputField e)
    {
        try { CurrencyManager.Instance.IncrementCoin(int.Parse(e.text)); }
        catch { }
    }

    public void IncreaseDiamond(InputField e)
    {
        try { CurrencyManager.Instance.IncrementDiamond(int.Parse(e.text)); }
        catch { }
    }

    public void IncreaseGem(InputField e)
    {
        try { CurrencyManager.Instance.IncrementGem(int.Parse(e.text)); }
        catch { }
    }
    #endregion

    #region MAINMENU
    public Button btnAdventureWorld;
    public Button btnTrainingArena;
    public Button btnStudyRoom;
    public Button btnTreasureRoom;
    public Button btnHighScore;
    public Button btnSquad;
    public Button btnKarakter;
    public Button btnDebug;

    public void OnButtonInitialized()
    {
        btnAdventureWorld.onClick.AddListener(() => {
            SceneManager.LoadScene("map");
        });

        btnTrainingArena.onClick.AddListener(() => {
            Button.ButtonClickedEvent _onClick =  new Button.ButtonClickedEvent();
            _onClick.AddListener(() => {
                heroSelectionController.GetHeroAnimName();
                SceneManager.LoadScene("MM");
            });

            AudioManager.Instance.PlaySFX(4, _onClick);
        });

        btnStudyRoom.onClick.AddListener(() => {
            SetStudyRoom();
            SceneManager.LoadScene("QuizRoom");
        });

        btnTreasureRoom.onClick.AddListener(() => {
            SetTreasureRoom();
            SceneManager.LoadScene("QuizRoom");
        });

        btnHighScore.onClick.AddListener(() => {
            Menu.SetActive(false);
            HighScore.SetActive(true);
        });

        btnSquad.onClick.AddListener(() => {
            BeforeAfterGameObject e = new BeforeAfterGameObject();
            e.go_to = SquadPanel;
            e.current = Menu;

            Menu.SetActive(false);
            AddCurrentGOsToActiveGOs(e);
            SquadPanel.SetActive(true);
        });

        btnKarakter.onClick.AddListener(() => { heroSelectionController.OnChangeHero(); });

        btnDebug.onClick.AddListener(() => {
            BeforeAfterGameObject e = new BeforeAfterGameObject();
            e.current = Menu;
            e.go_to = DebugPanel;

            Menu.SetActive(false);
            AddCurrentGOsToActiveGOs(e);
            DebugPanel.SetActive(true);
        });
    }
    #endregion

    #region DisplayCurrencyHandler
    public TextMeshProUGUI CoinsTxt;
    public TextMeshProUGUI GemsTxt;
    public TextMeshProUGUI DiamondsTxt;

    void DisplayCurrency()
    {
        CoinsTxt.text = CurrencyManager.Instance.GetPlayerTotalCoins().ToString();
        GemsTxt.text = CurrencyManager.Instance.GetPlayerTotalGems().ToString();
        DiamondsTxt.text = CurrencyManager.Instance.GetPlayerTotalDiamonds().ToString();

    }
    #endregion
}

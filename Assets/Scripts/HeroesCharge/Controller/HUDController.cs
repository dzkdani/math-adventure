using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class HUDController : MonoBehaviour
{
    public GameObject CanvasHud;
    public Button PauseBtn;
    private bool HeroMPFull;

    [Header("Coin")]
    [Space(5)]
    public GameObject CoinHud;
    public TextMeshProUGUI CoinTxt;

    [Header("Item?")]
    [Space(5)]
    public GameObject ItemHud;
    public TextMeshProUGUI ItemTxt;

    [Header("Wave")]
    [Space(5)]
    public GameObject WaveHud;
    public TextMeshProUGUI WaveTxt;

    [Header("Timer")]
    [Space(5)]
    public float Timer;
    public bool IsTimerStart = false;
    public GameObject TimerHud;
    public TextMeshProUGUI TimerTxt;


    [Header("Units")]
    [Space(5)]
    public SpawnHeroUIHandler HeroUIHandler;
    public SpawnTroopUIHandler TroopUIHandler;
    public SelectHeroUIHandler HeroUIPrefab;
    public Transform HeroesContainer;
    private SpawnHeroUIHandler SelectedHeroUI;
    public SelectTroopUIHandler TroopUIPrefab;
    public Transform TroopsContainer;
    private List<TroopData> SelectedTroopsList;
    private List<SpawnTroopUIHandler> SelectedTroopsUIList;
    private SelectUnitsController selectUnitsController;
    private SpawnerPlayerController spawnerPlayerController;

    public static HUDController Instance;
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

    private void Start() 
    {
        InitListeners();

        CanvasHud.SetActive(false);

        selectUnitsController = FindObjectOfType<SelectUnitsController>();
        spawnerPlayerController = FindObjectOfType<SpawnerPlayerController>();
    }
    
    private void InitListeners()
    {
        PauseBtn.onClick.AddListener(delegate
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFXType.ButtonClick);
            //Setting menu
            PauseController.Instance.Pause();
        });
    }
   
    private void Update() 
    {
        if (IsTimerStart)
        {
            if (Timer > 0 && !PauseController.Instance.IsPaused() && !GameOverController.Instance.CheckGameOver())
            {
                Timer -= Time.deltaTime;
                if (SelectedHeroUI.MPBar.value < SelectedHeroUI.MPBar.maxValue)
                {
                    RegenMP();
                }
                DisplayTimer(Timer);       
            }
            else
            {
                DisplayTimer(Timer);    
                if (Timer <= 0)
                {
                    GameOverController.Instance.GameOver();
                }
            }
        }
    }

    #region Display HUD
    public void DisplayHUD()
    {
        DisplayHero();
        DisplayTroops();
    }

    private void DisplayHero()
    {
        SpawnHeroUIHandler spawnHeroUI = Instantiate(HeroUIHandler, HeroesContainer);
        spawnHeroUI.transform.localScale = new Vector3(.6f, .6f, 0);
        spawnHeroUI.HeroIcon.sprite = selectUnitsController.SelectedHero.Icon;
        spawnHeroUI.name = selectUnitsController.SelectedHero.Name;
        spawnHeroUI.HPBar.minValue = 0;
        spawnHeroUI.HPBar.maxValue = selectUnitsController.SelectedHero.BaseHP + (selectUnitsController.SelectedHero.IncreaseHP * selectUnitsController.SelectedHero.Level);
        spawnHeroUI.HPBar.value = spawnHeroUI.HPBar.maxValue;
        
        //for skills
        spawnHeroUI.MPBar.minValue = 0;
        spawnHeroUI.MPBar.maxValue = selectUnitsController.SelectedHero.MaxMP; //max mp
        spawnHeroUI.MPBar.value = spawnHeroUI.MPBar.minValue;
        spawnHeroUI.UseSkillBtn.interactable = false;

        SelectedHeroUI = spawnHeroUI;
    }

    public void DisplayDefeatedHero()
    {
        SelectedHeroUI.HeroIcon.GetComponent<Image>().color = Color.gray;
    }

    private void DisplayTroops()
    {
        SelectedTroopsList = new List<TroopData>();
        SelectedTroopsList = selectUnitsController.SelectedTroopsList;
        SelectedTroopsUIList = new List<SpawnTroopUIHandler>();

        for (var i = 0; i < SelectedTroopsList.Count; i++)
        {
            SpawnTroopUIHandler spawnTroopUI = Instantiate(TroopUIHandler, TroopsContainer);
            spawnTroopUI.transform.localScale = new Vector3(0.6f, 0.6f, 0);
            spawnTroopUI.TroopIcon.sprite = SelectedTroopsList[i].Icon;
            spawnTroopUI.name = SelectedTroopsList[i].Name + "_Player";
            spawnTroopUI.HPBar.minValue = 0;
            spawnTroopUI.HPBar.maxValue = SelectedTroopsList[i].BaseHP + (SelectedTroopsList[i].IncreaseHP * SelectedTroopsList[i].Level);
            spawnTroopUI.HPBar.value = spawnTroopUI.HPBar.maxValue;
            SelectedTroopsUIList.Add(spawnTroopUI);
        }
    }

    public void DisplayDefeatedTroops(string _name)
    {
        for (var i = 0; i < SelectedTroopsUIList.Count; i++)
        {
            if (SelectedTroopsUIList[i].name == _name)
            {
                SelectedTroopsUIList[i].TroopIcon.GetComponent<Image>().color = Color.grey;
            }
        }
    }
    #endregion

    #region Regen HP & MP
    public void UpdateCurHP(float _damage, string _name)
    {
        if (_name == SelectedHeroUI.name)
        {
            SelectedHeroUI.HPBar.value -= _damage;
        }
        else
        {
            for (var i = 0; i < SelectedTroopsUIList.Count; i++)
            {
                if (SelectedTroopsUIList[i].name == _name)
                {
                    SelectedTroopsUIList[i].HPBar.value -= _damage;
                }
            }   
        }
    }

    private void RegenMP()
    {
        SelectedHeroUI.MPBar.value += Time.deltaTime;
        if (SelectedHeroUI.MPBar.value == SelectedHeroUI.MPBar.maxValue)
        {
            SkillReady(); 
        }
    }
    #endregion

    #region Skills
    IEnumerator Flickering()
    {
        while (SelectedHeroUI.MPBar.value == SelectedHeroUI.MPBar.maxValue)
        {
            SelectedHeroUI.transform.GetChild(0).GetComponent<Image>().color = Color.black;
            yield return new WaitForSecondsRealtime(0.5f);
            SelectedHeroUI.transform.GetChild(0).GetComponent<Image>().color = Color.red;
            yield return new WaitForSecondsRealtime(0.5f);   
        }
    }

    public void SkillReady()
    {
        SelectedHeroUI.UseSkillBtn.interactable = true;
        SelectedHeroUI.UseSkillBtn.onClick.AddListener(delegate
        {
            GameObject.Find(SelectedHeroUI.name).GetComponent<UnitController>().UseSkill();
        } );
        StartCoroutine(Flickering());  
    }

    public void SkillUsed()
    {
        StopCoroutine(Flickering());
        SelectedHeroUI.MPBar.value = 0;
        SelectedHeroUI.UseSkillBtn.interactable = false;
    }
    #endregion

    #region Wave and Timer
    public void DisplayWave(int _curWave, int _maxWave)
    {
        WaveTxt.text = "Wave " + (_curWave + 1) + "/" + _maxWave;
    }
    private void DisplayTimer(float _timer)
    {
        int minute = (int)_timer / 60;
        int second = (int)_timer % 60;

        TimerTxt.text = string.Format("{0:00}:{1:00}", minute, second);
    }
    #endregion
}

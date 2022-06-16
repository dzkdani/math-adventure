using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class SelectUnitsController : MonoBehaviour
{
    [Header("Menu")]
    [Space(5)]
    public GameObject SelectHeroesMenu; 
    public GameObject SelectTroopsMenu;
    
    [Header("Containers")]
    [Space(5)]
    public Transform SelectHeroesContainer;
    public Transform SelectTroopsContainer;

    [Header("Prefabs")]
    [Space(5)]
    public SelectHeroUIHandler SelectHeroUIPrefab;
    public SelectTroopUIHandler SelectTroopUIPrefab;

    [Header("Buttons")]
    [Space(5)]
    public Button NextBtn;
    public Button PlayBtn;

    [Header("Max Unit")]
    [Space(5)]
    public int MaximumHeroSelected = 1;
    public int MaximumTroopSelected = 4;

    [Header("UI Handler")]
    [Space(5)]
    public List<SelectHeroUIHandler> SelectHeroUIList;
    public HeroData SelectedHero;
    public List<SelectTroopUIHandler> SelectTroopUIList;
    public List<TroopData> SelectedTroopsList;

    private int curUnitSelected;

    private HUDController hudController;
    private WaveController waveController;
    private SpawnerPlayerController spawnerPlayerController;
    
    private void Start() 
    {
        hudController = FindObjectOfType<HUDController>();
        waveController = FindObjectOfType<WaveController>();
        spawnerPlayerController = FindObjectOfType<SpawnerPlayerController>();

        SelectHeroesMenu.SetActive(true);
        NextBtn.interactable = false;
        PlayBtn.interactable = false;

        InitListeners(); 

        DisplayPlayerHero();
        DisplayPlayerTroop();
    }

    private void InitListeners()
    {
        NextBtn.onClick.AddListener(delegate
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFXType.ButtonClick);
            SelectHeroesMenu.SetActive(false);
            SelectTroopsMenu.SetActive(true);
            curUnitSelected = 0;
        });
        PlayBtn.onClick.AddListener(delegate
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFXType.ButtonClick);
            SelectTroopsMenu.SetActive(false);

            hudController.CanvasHud.SetActive(true);
            hudController.DisplayHUD(); 
            
            spawnerPlayerController.Deploying(SelectedHero.Name, 1);
            for (var i = 0; i < SelectedTroopsList.Count; i++)
            {
                spawnerPlayerController.Deploying(SelectedTroopsList[i].Name, 2);
            }
            waveController.StartWave();
            waveController.SetTotalUnit(SelectedTroopsList.Count+1); //+1 as for the hero
            HUDController.Instance.IsTimerStart = true; 
        });
    }

    private void DisplayPlayerHero()
    {
        SelectHeroUIList = new List<SelectHeroUIHandler>();
        for (int i = 0; i < PlayerDataManager.Instance.PlayerData.HeroesOwnedDataList.Count; i++)
        {
            if (PlayerDataManager.Instance.PlayerData.HeroesOwnedDataList[i].IsUnlocked)
            {
                HeroData heroData = PlayerDataManager.Instance.GetHeroInPlayerDataBasedIndex(i);
                SelectHeroUIHandler selectHeroUISpawn = Instantiate(SelectHeroUIPrefab, SelectHeroesContainer);
                selectHeroUISpawn.HeroIcon.sprite = heroData.Icon;
                heroData.Level = PlayerDataManager.Instance.PlayerData.HeroesOwnedDataList[i].Level;
                selectHeroUISpawn.HeroLevelTxt.text = "Lv. " + heroData.Level;
                selectHeroUISpawn.SelectHeroBtn.onClick.AddListener(delegate
                {
                    SelectHero(selectHeroUISpawn, heroData);
                });
                SelectHeroUIList.Add(selectHeroUISpawn);
            }
        }
    }
    private void SelectHero(SelectHeroUIHandler _selectHeroUI, HeroData _selectedHero)
    {
        _selectHeroUI.DisabledImg.enabled = true;
        if (_selectHeroUI.SelectedIcon.enabled)
        {
            _selectHeroUI.SelectedIcon.enabled = false;
            curUnitSelected--;
            SelectedHero = null;
        }
        else
        {
            _selectHeroUI.SelectedIcon.enabled = true;
            curUnitSelected++;
            SelectedHero = _selectedHero;
        }
        CheckPlayerHasChosenHero();
    }

    private void CheckPlayerHasChosenHero()
    {
        if (curUnitSelected >= MaximumHeroSelected)
        {
            NextBtn.interactable = true;
            for (int i = 0; i < SelectHeroUIList.Count; i++)
            {
                if (!SelectHeroUIList[i].SelectedIcon.enabled)
                {
                    SelectHeroUIList[i].SelectHeroBtn.interactable = false;
                    SelectHeroUIList[i].DisabledImg.enabled = true;
                }
            }
        }
        else
        {
            NextBtn.interactable = false;
            for (int i = 0; i < SelectHeroUIList.Count; i++)
            {
                if (!SelectHeroUIList[i].SelectedIcon.enabled)
                {
                    SelectHeroUIList[i].SelectHeroBtn.interactable = true;
                    SelectHeroUIList[i].DisabledImg.enabled = false;
                }
            }
        }
    }

    private void DisplayPlayerTroop()
    {
        SelectTroopUIList = new List<SelectTroopUIHandler>();
        for (int i = 0; i < PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList.Count; i++)
        {
            if (PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList[i].IsUnlocked)
            {
                TroopData troopData = TroopManager.Instance.GetTroopData(PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList[i].Name);
                SelectTroopUIHandler selectTroopUISpawn = Instantiate(SelectTroopUIPrefab, SelectTroopsContainer);
                selectTroopUISpawn.TroopIcon.sprite = troopData.Icon;
                troopData.Level = PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList[i].Level;
                selectTroopUISpawn.TroopLevelTxt.text = "Lv. " + troopData.Level;
                selectTroopUISpawn.SelectTroopBtn.onClick.AddListener(delegate
                {
                    SelectTroop(selectTroopUISpawn, troopData);
                });
                SelectTroopUIList.Add(selectTroopUISpawn);
            }
        }
    }
    private void SelectTroop(SelectTroopUIHandler _selectTroopUIHandler, TroopData _selectedTroops)
    {
        _selectTroopUIHandler.DisabledImg.enabled = true;
        if (_selectTroopUIHandler.SelectedIcon.enabled)
        {
            _selectTroopUIHandler.SelectedIcon.enabled = false;
            curUnitSelected--;
            SelectedTroopsList.Remove(_selectedTroops);
        }
        else
        {
            _selectTroopUIHandler.SelectedIcon.enabled = true;
            curUnitSelected++;
            SelectedTroopsList.Add(_selectedTroops);
        }
        CheckPlayerHasChosenTroop();
    }
    private void CheckPlayerHasChosenTroop()
    {
        if (curUnitSelected >= MaximumTroopSelected)
        {
            PlayBtn.interactable = true;
            for (int i = 0; i < SelectTroopUIList.Count; i++)
            {
                if (!SelectTroopUIList[i].SelectedIcon.enabled)
                {
                    SelectTroopUIList[i].SelectTroopBtn.interactable = false;
                    SelectTroopUIList[i].DisabledImg.enabled = true;
                }
            }
        }
        else
        {
            PlayBtn.interactable = false;
            for (int i = 0; i < SelectTroopUIList.Count; i++)
            {
                if (!SelectTroopUIList[i].SelectedIcon.enabled)
                {
                    SelectTroopUIList[i].SelectTroopBtn.interactable = true;
                    SelectTroopUIList[i].DisabledImg.enabled = false;
                }
            }
        }
    }
}

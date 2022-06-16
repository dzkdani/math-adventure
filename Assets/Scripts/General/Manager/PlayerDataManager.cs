using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;  
    public PlayerData PlayerData;

    public bool IsDoneIntro;
    public bool IsResetData;  
    public int LevelMax, LevelCapMax, RangeLevelPerCap;

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
        LoadStatusIntro();
    }

    void OnApplicationQuit()
    {

    }
    void OnApplicationPause(bool pause)
    {
        if (pause)
        {

        }
    }
    void OnApplicationFocus(bool focusStatus) 
    {
        if (focusStatus)
        {
            
        }    
    }

    public void SaveStatusIntro()
    {
        int a = 0;
        if (IsDoneIntro) a = 1;
        PlayerPrefs.SetInt(PlayerData.PlayerProfileData.StudentName+"_isDoneIntro", a);
    }

    public void LoadStatusIntro()
    {
        int a = PlayerPrefs.GetInt(PlayerData.PlayerProfileData.StudentName + "_isDoneIntro");
        switch (a)
        {
            case 1 : IsDoneIntro = true; break;
            default: IsDoneIntro = false; break;
        }
    }

    public void UpdatePlayerCurrency()
    {
        APIManager.Instance.PostCurrency(PlayerPrefs.GetString("person_id"), "COIN", PlayerData.PlayerCurrencyData.TotalCoins.ToString());
        APIManager.Instance.PostCurrency(PlayerPrefs.GetString("person_id"), "GEM", PlayerData.PlayerCurrencyData.TotalGems.ToString());
        APIManager.Instance.PostCurrency(PlayerPrefs.GetString("person_id"), "DIAMOND", PlayerData.PlayerCurrencyData.TotalDiamonds.ToString());
        APIManager.Instance.PostCurrency(PlayerPrefs.GetString("person_id"), "KEY", PlayerData.PlayerCurrencyData.TotalKey.ToString());
    }

    public bool OnSubscription()
    {
        return DateTime.Parse(PlayerData.PlayerSubscriptionData.SubscriptionEndDate) >= System.DateTime.Now; 
    }

    public TroopData GetTroopInPlayerDataBasedIndex(int a) 
    {
        TroopData troopData = TroopManager.Instance.GetTroopData(PlayerData.TroopsOwnedDataList[a].Name);
        return troopData;
    }

    public HeroData GetHeroInPlayerDataBasedIndex(int a)
    {
        string[] sd = PlayerData.HeroesOwnedDataList[a].Name.Split('_');
        HeroData heroData = HeroManager.Instance.GetHeroData(sd[0]);
        return heroData;
    }

    public string GetHeroNameInPlayerDataBasedIndex(int i)
    {
        string[] sd = PlayerData.HeroesOwnedDataList[i].Name.Split('_');
        return sd[0];
    }

    public bool IsSelectFirstHero()
    {
        foreach (var ownedHero in PlayerData.HeroesOwnedDataList)
        {
            if (ownedHero.IsUnlocked)
            {
                return false;
            }
        }
        return true;
    }

    public void ClearedStage(StageData _clearedStage, int _stars) 
    {
        for (int i = 0; i < PlayerData.PlayerStageSelectionDataList.Count; i++)
        {
            if (PlayerData.PlayerStageSelectionDataList[i].StageId == _clearedStage.StageId)
            {
                PlayerData.PlayerStageSelectionDataList[i].IsCleared = true;
                PlayerData.PlayerStageSelectionDataList[i].TotalStars = _stars;
                PlayerData.PlayerStageSelectionDataList[i+1].IsUnlocked = true;
            }
        }
    }

    public void AddKey(int _correctAnswer)
    {
        if (_correctAnswer > 0)
        {
            PlayerData.PlayerCurrencyData.TotalKey += _correctAnswer;
            if (PlayerData.PlayerCurrencyData.TotalKey >= 5)
            {
                PlayerData.PlayerCurrencyData.TotalKey = 5;
            }
        }
        else
        {
            return;
        }
    }

    public int GetKey() { return PlayerData.PlayerCurrencyData.TotalKey; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Kelas
{
    Kelas3,
    Kelas4,
    Kelas5,
    Kelas6
}

[CreateAssetMenu(fileName = "PlayerData", menuName = "Datas/Players/PlayerData")]
public class PlayerData : ScriptableObject  
{
    public ProfileData PlayerProfileData;
    public AccountData PlayerAccountData;
    public CurrencyData PlayerCurrencyData;
    public SubscriptionData PlayerSubscriptionData;
    public List<StageSelectionData> PlayerStageSelectionDataList;
    public List<UnitsOwnedData> HeroesOwnedDataList; 
    public List<UnitsOwnedData> TroopsOwnedDataList;

    [System.Serializable]
    public class ProfileData 
    {
        public string StudentName; 
        public Kelas StudentClass;
        public string StudentSchool;
        public string ParentWhatsapp;
        public string StudentWhatsapp;
    }

    [System.Serializable]
    public class AccountData
    {
        public string person_category_id;
        public string category;
    }

    [System.Serializable]
    public class SubscriptionData 
    {
        public string SubscriptionEndDate;
        public int RewardPoint;
        public string ReferralCode;
    }

    [System.Serializable]
    public class StageSelectionData
    {
        public string StageId;
        public int TotalStars;
        public bool IsUnlocked;
        public bool IsCleared;
        public bool IsBossStage;
    }

    [System.Serializable] 
    public class CurrencyData
    {
        public int TotalCoins;
        public int TotalGems;
        public int TotalPoints;
        public int TotalDiamonds;
        public int TotalKey;
    }
    
    [System.Serializable]
    public class UnitsOwnedData
    {
        public string Name;
        public bool IsSelected;
        public bool IsUnlocked;
        public int Level;
        public int LevelCap;
        public int DuplicateCard;
        public bool TimeToRankUp;
    }
}

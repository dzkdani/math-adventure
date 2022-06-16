using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public int MaxCoinCurrency = 99999;
    public int MaxGemCurrency = 9999;
    public int MaxPointCurrency = 999;
    public int MaxDiamondCurrency = 999;

    public Sprite CoinCurrencyIcon;
    public Sprite GemCurrencyIcon;
    public Sprite DiamondCurrencyIcon;

    private PlayerData.CurrencyData playerCurrencyData;
    public static CurrencyManager Instance;

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
        playerCurrencyData = PlayerDataManager.Instance.PlayerData.PlayerCurrencyData;
    }

    #region DIAMOND
    public void IncrementDiamond(int _diamond)
    {
        playerCurrencyData.TotalDiamonds += _diamond;
        if (playerCurrencyData.TotalDiamonds >= MaxDiamondCurrency)
        {
            playerCurrencyData.TotalDiamonds = MaxDiamondCurrency;
        }
        PlayerDataManager.Instance.UpdatePlayerCurrency();
    }
    public void DecrementDiamond(int _diamond)
    {
        playerCurrencyData.TotalDiamonds -= _diamond;
        if (playerCurrencyData.TotalDiamonds <= 0)
        {
            playerCurrencyData.TotalDiamonds = 0;
        }
        PlayerDataManager.Instance.UpdatePlayerCurrency();
    }
    public int GetPlayerTotalDiamonds()
    {
        playerCurrencyData = PlayerDataManager.Instance.PlayerData.PlayerCurrencyData;
        return playerCurrencyData.TotalDiamonds;
    }
    #endregion

    #region COIN
    public void IncrementCoin(int _coin)
    {
        playerCurrencyData.TotalCoins += _coin;
        if (playerCurrencyData.TotalCoins >= MaxCoinCurrency)
        {
            playerCurrencyData.TotalCoins = MaxCoinCurrency;
        }
        PlayerDataManager.Instance.UpdatePlayerCurrency();
    }
    public void DecrementCoin(int _coin)
    {
        playerCurrencyData.TotalCoins -= _coin;
        if (playerCurrencyData.TotalCoins <= 0)
        {
            playerCurrencyData.TotalCoins = 0;
        }
        PlayerDataManager.Instance.UpdatePlayerCurrency();
    }
    public int GetPlayerTotalCoins()
    {
        playerCurrencyData = PlayerDataManager.Instance.PlayerData.PlayerCurrencyData;
        return playerCurrencyData.TotalCoins;
    }
    #endregion

    #region GEM
    public void IncrementGem(int _gem)
    {
        playerCurrencyData.TotalGems += _gem;
        if (playerCurrencyData.TotalGems >= MaxGemCurrency)
        {
            playerCurrencyData.TotalGems = MaxGemCurrency;
        }
        PlayerDataManager.Instance.UpdatePlayerCurrency();
    }
    public void DecrementGem(int _gem)
    {
        playerCurrencyData.TotalGems -= _gem;
        if (playerCurrencyData.TotalGems <= 0)
        {
            playerCurrencyData.TotalGems = 0;
        }
        PlayerDataManager.Instance.UpdatePlayerCurrency();
    }
    public int GetPlayerTotalGems()
    {
        return playerCurrencyData.TotalGems;
    }
    #endregion

    #region POINT
    public void IncrementPoint(int _point)
    {
        playerCurrencyData.TotalPoints += _point;
        if (playerCurrencyData.TotalPoints >= MaxPointCurrency)
        {
            playerCurrencyData.TotalPoints = MaxPointCurrency;
        }
        PlayerDataManager.Instance.UpdatePlayerCurrency();
    }
    public void DecrementPoint(int _point)
    {
        playerCurrencyData.TotalPoints -= _point;
        if (playerCurrencyData.TotalPoints <= 0)
        {
            playerCurrencyData.TotalPoints = 0;
        }
        PlayerDataManager.Instance.UpdatePlayerCurrency();
    }
    public int GetPlayerTotalPoints()
    {
        return playerCurrencyData.TotalPoints;
    }
    #endregion
}

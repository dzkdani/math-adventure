using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingArenaSettingManager : MonoBehaviour
{
    public static TrainingArenaSettingManager Instance;
    public TerrainIDList SetList;
    public string CurrHeroSelect;
    public bool POWERUP, isKOTAKON, DebugHighScore, isShowSetName, isTutorialOn;

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
}

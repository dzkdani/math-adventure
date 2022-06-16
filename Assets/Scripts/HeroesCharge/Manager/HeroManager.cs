using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    [SerializeField]
    private List<HeroData> heroDataList;

    public static HeroManager Instance;

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

    public HeroData GetHeroData(string _heroName)
    {
        HeroData heroData = null;
        for (int i = 0; i < heroDataList.Count; i++)
        {
            if (heroDataList[i].Name == _heroName)
            {
                heroData = heroDataList[i];
                break;
            }
        }
        return heroData;
    }
    public int GetHeroDataListCount()
    {
        return heroDataList.Count;
    }

}

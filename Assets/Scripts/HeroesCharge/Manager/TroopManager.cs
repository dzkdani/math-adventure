using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopManager : MonoBehaviour
{
    [SerializeField]
    private List<TroopData> troopDataList;

    public static TroopManager Instance;

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

    public TroopData GetTroopData(string _troopName)
    {
        TroopData troopData = null;
        for(int i = 0; i < troopDataList.Count; i++)
        {
            if (troopDataList[i].Name == _troopName)
            {
                troopData = troopDataList[i];
                break;
            }
        }
        return troopData;
    }
    public int GetTroopDataListCount()
    {
        return troopDataList.Count;
    }

    public string GetNameBasedIndex(int a)
    {
        return troopDataList[a].Name;
    }

    public TroopData GetTroopBasedIndex(int a)
    {
        return troopDataList[a];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private List<StageData> stageDataList;

    [SerializeField]
    private StageData curStageData;

    public static StageManager Instance;

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

    public StageData GetCurStageData()
    {
        return curStageData;
    }
    
    public void SetCurStageData(string _stageId)
    {
        for(int i = 0; i < stageDataList.Count; i++)
        {
            if (stageDataList[i].StageId == _stageId)
            {
                curStageData = stageDataList[i];
                break;
            }
        }
    }
}

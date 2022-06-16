using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Datas/Stage/StageData")] 
public class StageData : ScriptableObject
{
    public string StageId;
    public string StageName;
    public bool IsUnlocked;
    public bool IsCleared;
    public bool IsBossStage;
    public int EnemyLevel;
    public List<Sprite> StageBackgroundList;
    public List<WaveSpawnerData> WaveSpawnerDataList;

    [System.Serializable]
    public class WaveSpawnerData
    {
        public string WaveId;
        public List<WaveTroopData> WaveTroopDataList;
    }
    [System.Serializable]
    public class WaveTroopData
    {
        public string Name;
        public int TotalSpawn;
    }
}

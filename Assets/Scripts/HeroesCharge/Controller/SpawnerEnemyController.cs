using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
 
public class SpawnerEnemyController : MonoBehaviour 
{
    [Header("Param")]
    [Space(5)]
    public Transform[] WaveContainers;
    public EnemyWaveData[] EnemyWaveList;
    [SerializeField]
    private float xPosSpawnLimit;
    [SerializeField]
    private float yPosSpawnLimit;

    private StageData stageData;
    
    private void Start() 
    {
        stageData = StageManager.Instance.GetCurStageData();
        EnemyWaveList = new EnemyWaveData[WaveContainers.Length];

        WaveSpawner();    
    }

    private void WaveSpawner()
    {
        for(int i = 0; i < stageData.WaveSpawnerDataList.Count; i++)
        {
            EnemyWaveList[i] = new EnemyWaveData 
            { 
                TroopList = new List<GameObject>()
            };
            for (int j = 0; j < stageData.WaveSpawnerDataList[i].WaveTroopDataList.Count; j++)
            {
                TroopData troopData = TroopManager.Instance.GetTroopData(stageData.WaveSpawnerDataList[i].WaveTroopDataList[j].Name);
                GameObject spawnedTroop = Instantiate(troopData.Model, WaveContainers[i]);
                
                spawnedTroop.name = troopData.Name + "_Enemy";

                float randPosX = Random.Range(-xPosSpawnLimit, xPosSpawnLimit);
                float randPosY = Random.Range(-yPosSpawnLimit, yPosSpawnLimit);
                Vector2 spawnPos = new Vector2(WaveContainers[i].transform.position.x + randPosX, WaveContainers[i].transform.position.y + randPosY);
                spawnedTroop.GetComponent<SortingGroup>().sortingOrder = ((int)(spawnPos.y * 10) * -1);
                spawnedTroop.transform.position = spawnPos;
                spawnedTroop.GetComponent<UnitController>().InitData(
                    troopData.AttackType,
                    troopData.Projectile,
                    troopData.BaseHP + (stageData.EnemyLevel * troopData.IncreaseHP),
                    troopData.BaseATK + (stageData.EnemyLevel * troopData.IncreaseATK),
                    troopData.BaseDEF + (stageData.EnemyLevel * troopData.IncreaseDEF),
                    troopData.BaseATKSPD,
                    troopData.BaseMOVSPD,
                    troopData.BaseATKRANGE);
                spawnedTroop.GetComponent<UnitController>().InitEnemyModel();
                spawnedTroop.SetActive(false);
                EnemyWaveList[i].TroopList.Add(spawnedTroop);
            }
        }
    }

    [System.Serializable]
    public class EnemyWaveData
    {
        public List<GameObject> TroopList;
    }
}

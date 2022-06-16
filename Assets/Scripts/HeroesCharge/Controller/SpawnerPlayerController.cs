using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
 
public class SpawnerPlayerController : MonoBehaviour
{
    [Header("Param")]
    [Space(5)]
    public Transform HeroContainer; 
    public Transform TroopsContainer; 
    public List<GameObject> PlayerTroopList;
    [SerializeField]
    private float xPosSpawnLimit;
    [SerializeField]
    private float yPosSpawnLimit;

    [Header("UI")]
    [Space(5)]
    [SerializeField]
    private SpawnTroopUIHandler spawnTroopPrefab;
    [SerializeField]
    private List<SpawnTroopUIHandler> spawnTroopList = new List<SpawnTroopUIHandler>();

    public void Deploying(string _unitName, int _unitType)
    {
        switch (_unitType)
        {
            case 1: SpawnHero(_unitName);
                break;
            case 2: SpawnTroops(_unitName);
                break;
            default:
                break;
        }
    }

    private void SpawnHero(string _heroName)
    {
        for (int i = 0; i < PlayerDataManager.Instance.PlayerData.HeroesOwnedDataList.Count; i++)
        {
            if (PlayerDataManager.Instance.GetHeroNameInPlayerDataBasedIndex(i) == _heroName && PlayerDataManager.Instance.PlayerData.HeroesOwnedDataList[i].IsUnlocked)
            {
                for (int j = 0; j < HeroManager.Instance.GetHeroDataListCount(); j++)
                {
                    //HeroData heroData = HeroManager.Instance.GetHeroData(PlayerDataManager.Instance.PlayerData.HeroesOwnedDataList[i].Name);
                    HeroData heroData = PlayerDataManager.Instance.GetHeroInPlayerDataBasedIndex(i);
                    if (heroData != null)
                    {
                        GameObject spawnedHero = Instantiate(heroData.Prefab, HeroContainer);
                        spawnedHero.name = heroData.Name;
                        spawnedHero.transform.position = HeroContainer.position;
                        spawnedHero.GetComponent<UnitController>().InitData(
                            heroData.Type,
                            heroData.Projectile,
                            heroData.BaseHP + (PlayerDataManager.Instance.PlayerData.HeroesOwnedDataList[i].Level * heroData.IncreaseHP),
                            heroData.BaseATK + (PlayerDataManager.Instance.PlayerData.HeroesOwnedDataList[i].Level * heroData.IncreaseATK),
                            heroData.BaseDEF + (PlayerDataManager.Instance.PlayerData.HeroesOwnedDataList[i].Level * heroData.IncreaseDEF),
                            heroData.BaseATKSPD,
                            heroData.BaseMOVSPD,
                            heroData.BaseATKRANGE);
                        spawnedHero.GetComponent<UnitController>().InitPlayerModel();
                        spawnedHero.GetComponent<UnitController>().IsCurWave = true;
                        spawnedHero.GetComponent<UnitController>().IsHero = true;
                        break;
                    }
                }
                break;
            }
        }
    }

    private void SpawnTroops(string troopName)
    {
        for (int i = 0; i < PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList.Count; i++)
        {
            if (PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList[i].Name == troopName && PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList[i].IsUnlocked)
            {
                for (int j = 0; j < TroopManager.Instance.GetTroopDataListCount(); j++)
                {
                    TroopData troopData = TroopManager.Instance.GetTroopData(PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList[i].Name);
                    if (PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList[i].Name == troopData.Name)
                    {
                        GameObject spawnedTroop = Instantiate(troopData.Model, TroopsContainer);
                        spawnedTroop.name = troopData.Name + "_Player";

                        float randPosX = Random.Range(-xPosSpawnLimit, xPosSpawnLimit);
                        float randPosY = Random.Range(-yPosSpawnLimit, yPosSpawnLimit);
                        Vector2 spawnPos = new Vector2(TroopsContainer.transform.position.x + randPosX, TroopsContainer.transform.position.y + randPosY);
                        spawnedTroop.transform.position = spawnPos;
                        spawnedTroop.GetComponent<SortingGroup>().sortingOrder = ((int)spawnPos.y * -1);
                        
                        spawnedTroop.GetComponent<UnitController>().InitData(
                            troopData.AttackType,
                            troopData.Projectile,
                            troopData.BaseHP + (PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList[i].Level * troopData.IncreaseHP),
                            troopData.BaseATK + (PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList[i].Level * troopData.IncreaseATK),
                            troopData.BaseDEF + (PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList[i].Level * troopData.IncreaseDEF),
                            troopData.BaseATKSPD,
                            troopData.BaseMOVSPD,
                            troopData.BaseATKRANGE);
                        spawnedTroop.GetComponent<UnitController>().InitPlayerModel();
                        spawnedTroop.GetComponent<UnitController>().IsCurWave = true;
                        PlayerTroopList.Add(spawnedTroop);
                        break;
                    }
                }
                break;
            }
        }
    }
}

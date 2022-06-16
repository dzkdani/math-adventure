using UnityEngine;
 
public class WaveController : MonoBehaviour
{
    [Header("WAVE")]
    [Space(5)]
    public int TotalWave; 
    public int TotalEnemiesCurWave;
    public int CurWave;

    [Header("UNIT")]
    [Space(5)]
    [SerializeField]
    private int RemainingUnit; 

    private StageData stageData;

    private SpawnerEnemyController spawnerEnemyController;
    private SpawnerPlayerController spawnerPlayerController;
    private CameraController cameraController;
    
    private void Start() {
        stageData = StageManager.Instance.GetCurStageData();
        spawnerEnemyController = FindObjectOfType<SpawnerEnemyController>();
        spawnerPlayerController = FindObjectOfType<SpawnerPlayerController>();
        cameraController = FindObjectOfType<CameraController>();
    }

    public void StartWave()
    {
        TotalWave = stageData.WaveSpawnerDataList.Count;
        TotalEnemiesCurWave = spawnerEnemyController.EnemyWaveList[CurWave].TroopList.Count;
        for (int i = 0; i < spawnerEnemyController.EnemyWaveList[CurWave].TroopList.Count; i++)
        {
            spawnerEnemyController.EnemyWaveList[CurWave].TroopList[i].SetActive(true);
            spawnerEnemyController.EnemyWaveList[CurWave].TroopList[i].GetComponent<UnitController>().IsCurWave = true;
        }
        HUDController.Instance.DisplayWave(CurWave, TotalWave);
    }

    public void SetTotalUnit(int _totalUnit) 
    {
        RemainingUnit = _totalUnit;
    }

    public void UnitDefeated()
    { 
        RemainingUnit -= 1;  
        CheckCurrentRemainingUnit();
    }


    private void CheckCurrentRemainingUnit()
    {
        if (RemainingUnit == 0)
        {
            GameOverController.Instance.GameOver();
        }
        else
        {
            if (CurWave == TotalWave && TotalEnemiesCurWave <= 0)
            {
                GameOverController.Instance.StageClear();
            }
        }
        
    }

    public void CheckCurrentWaveCleared()
    {
        TotalEnemiesCurWave--;
        if (TotalEnemiesCurWave == 0)
        {
            if (CurWave < TotalWave-1)
            {
                CurWave++;
                cameraController.UpdateCameraPosition();
                cameraController.IsMovingCamera = true;
            }
            else
            {
                GameOverController.Instance.StageClear();
            }
        }
    }
}

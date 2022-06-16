using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectController : MonoBehaviour
{
    [SerializeField]
    private List<SelectStageUIHandler> StageUIList;
    [SerializeField]
    private RectTransform GateCanvas;

    public GameObject TroopOwnedCheckContainer;
    public Button btnKota1, btnKota2, btnKota3;
    public RectTransform TownOpen1, TownOpen2, TownOpen3, Town;

    private void Init()
    {
        currentScene = Town;
        btnKota1.onClick.AddListener(() => { MoveWithTween(TownOpen1); });
        btnKota2.onClick.AddListener(() => { MoveWithTween(TownOpen2); });
        btnKota3.onClick.AddListener(() => { MoveWithTween(TownOpen3); });
    }

    private void Awake() {
        StageUIList.AddRange(FindObjectsOfType<SelectStageUIHandler>());
    }
    
    private void Start() 
    {
        Init();
        
        int ownedTroop = 0;
        PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList.ForEach(troop => 
        {
            if (troop.IsUnlocked) ownedTroop++;
        });
        
        if(ownedTroop<4)TroopOwnedCheckContainer.SetActive(true);

        
        GetStageDatas();
    }

    private void GetStageDatas()
    {
        foreach (var stage in PlayerDataManager.Instance.PlayerData.PlayerStageSelectionDataList)
        {
            foreach (var stageBtn in StageUIList)
            {
                if (stageBtn.StageName == stage.StageId)
                {
                    stageBtn.SelectStageBtn.onClick.AddListener(delegate {SetStageData(stageBtn.StageName);});
                    if (stage.IsUnlocked)
                    {
                        stageBtn.IconLocked.enabled = false;
                        stageBtn.SelectStageBtn.interactable = true;
                        if (stage.IsCleared)
                        {
                            stageBtn.IconCleared.enabled = true;
                            stageBtn.StarImg.SetActive(true);
                            if (stage.TotalStars <= 1)
                            {
                                stageBtn.StarImg.transform.GetChild(0).gameObject.SetActive(true);
                            }
                            if (stage.TotalStars <= 2)
                            {
                                stageBtn.StarImg.transform.GetChild(1).gameObject.SetActive(true);
                            }
                            if (stage.TotalStars <= 3)
                            {
                                stageBtn.StarImg.transform.GetChild(2).gameObject.SetActive(true);
                            }
                        }
                        else
                        {
                            stageBtn.IconCleared.enabled = false;
                        }
                    }
                    else
                    {
                        stageBtn.IconLocked.enabled = true;
                    }
                }
            }
        }
    }

    private void SetStageData(string _stageName)
    {
        StageManager.Instance.SetCurStageData(_stageName);
        if (!_stageName.Contains("Boss"))
        {
            SceneManager.LoadScene("HeroesCharge");
        }
        else
        {
            CheckBossGateUnlocked();
        }
    }

    private void CheckBossGateUnlocked()
    {
        if (PlayerDataManager.Instance.GetKey() < 5)
        {
            //belum bisa main
            //kasih liat gerbang
            MoveWithTween(GateCanvas);
            //gembok kebuka berdasarkan kunci yang dipunya
            Debug.Log(PlayerDataManager.Instance.GetKey());
            Debug.Log("You Shall Not Pass");
        }
        else
        {
            SceneManager.LoadScene("HeroesCharge"); 
        }
    }

    public void BackToGameMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }

    #region DOTWEEN
    public RectTransform Titik_a;
    public RectTransform Titik_b;
    public RectTransform titik_default;
    
    RectTransform currentScene;
    public void MoveWithTween(RectTransform Temp)
    {
        RectTransform temp2 = currentScene;
        currentScene = null;
        temp2.position = Titik_b.position;

        Temp.localScale = Vector2.zero;
        Temp.position = titik_default.position;
        Temp.DOScale(Vector3.one, 0.4f);
        currentScene = Temp;
    }
    #endregion
}


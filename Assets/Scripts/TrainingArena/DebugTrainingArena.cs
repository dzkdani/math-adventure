using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DebugTrainingArena : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject InputFieldMenu;
    public GameObject MainMenu;

    public Toggle[] toggles;

    public void PlayAudioButton(ExtendedButtonOnClickPassingVariable a)
    {
        AudioManager.Instance.PlaySFX(a._integer[0], a._onClick);
    }

    private void Start()
    {
        TrainingArenaSettingManager.Instance.isTutorialOn = false;
        if (TrainingArenaSettingManager.Instance.DebugHighScore && PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentName.Equals(""))
        {
            MainMenu.SetActive(false);
        }
        else
        {
            InputFieldMenu.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        //TrainingArenaSettingManager.Instance.isKOTAKON = toggles[0].isOn;
        //TrainingArenaSettingManager.Instance.POWERUP = toggles[1].isOn;
        //TrainingArenaSettingManager.Instance.isShowSetName = toggles[2].isOn;

        TrainingArenaSettingManager.Instance.isKOTAKON = false;
        TrainingArenaSettingManager.Instance.POWERUP = true;
        TrainingArenaSettingManager.Instance.isShowSetName = false;
    }

    public void TutorialTurnOn()
    {
        TrainingArenaSettingManager.Instance.isTutorialOn = true;
    }

    public void InputName()
    {
        PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentName = inputField.text;
    }

    public void SetTerrainTrainingArena(TerrainIDList e)
    {
        TrainingArenaSettingManager.Instance.SetList = e;
    }

    public void LoadTheGame(string temp)
    {
        SceneManager.LoadScene(temp);
    }
}

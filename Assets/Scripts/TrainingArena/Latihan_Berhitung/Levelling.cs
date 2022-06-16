using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Levelling : MonoBehaviour
{
    public QuizManager Manager;
    public TextMeshProUGUI te, Level;
	public Image ExpBar;
    public int LevelValue = 1, Exp = 0;
    public int MaxExp;
    public bool StuckLevel = false;
    public static bool Easy, Medium, Hard = false;

    static int type_of_prize;

    void initLevel()
    {
        List<int> temp = HasilBelajarManager.Instance.GetLevel();
        LevelValue = temp[0];
        if (LevelValue <= 0) { LevelValue = 1; }
        Exp = temp[1];
        if (Exp <= 0) { Exp = 0; }
        UpdateLevelAndQuiz();
    }

    void UpdateLevelAndQuiz()
    {
        MaxExp = HasilBelajarManager.Instance.GetExpMaxOfThisLevel(LevelValue);
        StuckLevel = HasilBelajarManager.Instance.hasMaxLevelBeenReached();

        bool temp = TrainingArenaSettingManager.Instance.isTutorialOn;
        if (temp.Equals(true)) { Level.text = "1"; LevelValue = 1; }
        //else Level.text = LevelValue.ToString();

        //ExpBar.fillAmount = (float)Exp / MaxExp;

        /*if (LevelValue > HasilBelajarManager.Instance.GetMaxLevel())
        {
            LevelValue = 5;
            HasilBelajarManager.Instance.MaxLevelBeenReached(true);
        }*/

        if (AnswerChecker.RightAnswer == true)
        {
            if (temp.Equals(false))
            {
                if (Easy == true)
                {
                    Exp += HasilBelajarManager.Instance.GetEasy();
                }
                else if (Medium == true)
                {
                    Exp += HasilBelajarManager.Instance.GetMedium();
                }
                else if (Hard == true)
                {
                    Exp += HasilBelajarManager.Instance.GetHard();
                }
            }
            

            Hadiah_Quiz();
            AnswerChecker.RightAnswer = false;
        }
        else if (AnswerChecker.WrongAnswer == true)
        {
            AnswerChecker.WrongAnswer = false;
        }
    }

    private void Awake()
    {
        initLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Gameplay123")
        {
            return;
        }

        UpdateLevelAndQuiz();
    }

    public static void Get_Type_Of_Prize(int a) {
        type_of_prize = a;
    }

    public void Hadiah_Quiz()
    {
        int a = type_of_prize;
        switch (a)
        {
            case 1:
              ///  GameManager.mendapatkan_kartu_gratis();
                //break;
            case 2:
            case 3:
                if (!Manager.player.invisible_PowerActivated)
                {
                    if (Easy == true)
                    {
                        int i = 3;
                        TrainingArena_GM.PlayerDapatInvisibleDenganWaktu(i);
                    }
                    else if (Medium == true)
                    {
                        int i = 4;
                        //GameManager.Notifikasi_Immunity_Tambahan(i);
                        TrainingArena_GM.PlayerDapatInvisibleDenganWaktu(i);
                    }
                    else if (Hard == true)
                    {
                        int i = 6;
                        //GameManager.Notifikasi_Immunity_Tambahan(i);
                        TrainingArena_GM.PlayerDapatInvisibleDenganWaktu(i);
                    }
                    Manager.player.invisible_PowerActivated = true;
                }
                break;
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
                if (Easy == true)
                {
                    int i = 12;
                   // GameManager.Notifikasi_Energi_Tambahan(i, te);
                    TrainingArena_GM.PlayerDapatEnergiTambahan(i, te);
                }
                else if (Medium == true)
                {
                    int i = 18;
                    //GameManager.Notifikasi_Energi_Tambahan(i, te);
                    TrainingArena_GM.PlayerDapatEnergiTambahan(i, te);
                }
                else if (Hard == true)
                {
                    int i = 24;
                    //GameManager.Notifikasi_Energi_Tambahan(i, te);
                    TrainingArena_GM.PlayerDapatEnergiTambahan(i, te);
                }
                break;
        }
        Get_Type_Of_Prize(0);
    }
}

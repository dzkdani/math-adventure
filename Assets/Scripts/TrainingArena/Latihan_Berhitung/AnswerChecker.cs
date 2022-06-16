using UnityEngine;
using DG.Tweening;

public class AnswerChecker : MonoBehaviour
{
    public QuizManager Manager;

    public static bool Confirmed;
    public static bool Oked;

    //public Animator stateCondition;
    public GameObject Lose;
    public GameObject Win;
    public GameObject NumKey;
	
    public static bool RightAnswer;
    public static bool WrongAnswer;

    Vector3 ukuran_umum = new Vector3(1.1f, 1.1f, 1);
    bool isMyAnswerRight;
    
	// Start is called before the first frame update
    public void StartQuiz()
    {
        TimeManager.TimeOut = false;
        Confirmed = false;
        Oked = false;
        RightAnswer = false;
        WrongAnswer = false;
        isMyAnswerRight = false;

        Win.transform.localScale = Vector3.zero;
        Win.SetActive(false);

        Lose.transform.localScale = Vector3.zero;
        Lose.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeManager.TimeOut == true)
        {
            ConfirmAnswer();
        }
    }

    public void ConfirmAnswer()
    {
        if (!Confirmed)
        {

            if (Manager.KolomIsiJawaban.AnswerField.text == CSVFileConverter.myAnswer)
            {

                isMyAnswerRight = RightAnswer = true;
                NumKey.SetActive(false);
                Win.SetActive(true);
                Win.transform.DOScale(ukuran_umum, 0.2f);
            }
            else
            {
                WrongAnswer = true;
                NumKey.SetActive(false);
                Lose.SetActive(true);
                Lose.transform.DOScale(ukuran_umum, 0.2f);
            }
            Confirmed = true;
        }
    }

    public void ConfirmOK(GameObject e)
    {
        NumKey.SetActive(true);
        e.SetActive(false);
        Oked = true;

        Manager.timeManager.KillThreads();

        bool temp = TrainingArenaSettingManager.Instance.isTutorialOn;
        if (temp.Equals(false)) { Leveled(); }
    }

    public void Leveled()
    {
        if (Manager.levelling.Exp >= Manager.levelling.MaxExp && isMyAnswerRight)
        {
            int Exp = 0;
            Manager.levelling.Exp -= Manager.levelling.MaxExp;
            
            if (Levelling.Easy == true)
            {
                Exp += HasilBelajarManager.Instance.GetEasy();
            }
            else if (Levelling.Medium == true)
            {
                Exp += HasilBelajarManager.Instance.GetMedium();
            }
            else if (Levelling.Hard == true)
            {
                Exp += HasilBelajarManager.Instance.GetHard();
            }
            Exp -= Manager.levelling.Exp; 
            HasilBelajarManager.Instance.OnCorrectAnswer(Manager.levelling.LevelValue, Exp);

            Manager.levelling.LevelValue += 1;
            
            if (Manager.levelling.LevelValue <= HasilBelajarManager.Instance.GetMaxLevel())
            {
                HasilBelajarManager.Instance.ChangeKategori(Manager.levelling.LevelValue);
                HasilBelajarManager.Instance.ScoreIncreased(Manager.levelling.Exp);
            }
            else
            {
                Manager.levelling.LevelValue = 5;
                HasilBelajarManager.Instance.MaxLevelBeenReached(true);
            }

            CurrencyManager.Instance.IncrementCoin(10);
            Manager.PlayerDapatCoinTambahan(10);
            isMyAnswerRight = false;
        }
        else if(isMyAnswerRight)
        {
            if (isMyAnswerRight)
            {
                HasilBelajarManager.Instance.OnCorrectAnswer(Manager.levelling.LevelValue, Manager.levelling.Exp);
                isMyAnswerRight = false;
            }

        }
    }
}

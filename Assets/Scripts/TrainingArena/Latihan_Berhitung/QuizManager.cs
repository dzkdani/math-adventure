using System.Diagnostics;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public TrainingArena_PlayerController player;
    public CSVFileConverter Soal;
	public AnswerChecker PemeriksaJawaban;
	public InputNumber KolomIsiJawaban;
    public Levelling levelling;
    public TimeManager timeManager;

    public GameObject Calculator, PanelDifficulty;
    public TextMeshProUGUI coin_text;
    public CanvasGroup coin_canvasgroup;

    public Slider TimeBar;
    public Text TimeExecuted;
    public Text TimeExecuted2;

    private void Start()
    {
        HasilBelajarManager.Instance.UpdateLatestKategori();
    }

    void ResetDifficulty(){
        Levelling.Easy = false;
        Levelling.Medium = false;
        Levelling.Hard = false;
    }

	// Start is called before the first frame update
    public void Easy()
    {
        ResetDifficulty();
		Levelling.Easy = true;
        StartQuiz();
    }

    public void Medium()
    {
        ResetDifficulty();
        Levelling.Medium = true;
        StartQuiz();
    }

    public void Hard()
    {
        ResetDifficulty();
        Levelling.Hard = true;
        StartQuiz();
    }

    public void PlayerDapatCoinTambahan(int plus)
    {
        coin_text.text = "+" + plus;
        coin_canvasgroup.DOFade(255, 1f).OnComplete(() => coin_canvasgroup.GetComponent<CanvasGroup>().DOFade(0, 1f));
    }

    void StartQuiz() {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        TimeExecuted.text = "";
        TimeExecuted2.text = "";

        Calculator.gameObject.SetActive(true);
		PemeriksaJawaban.StartQuiz();
		KolomIsiJawaban.StartQuiz();
        Soal.StartQuiz();
        PanelDifficulty.gameObject.SetActive(false);

        timeManager.RunQuizTimer();
        //sw.Stop();
        //UnityEngine.Debug.Log("Time needed : " + sw.Elapsed.TotalMilliseconds);
    }

    public void ConfirmOK() { PemeriksaJawaban.ConfirmOK(Calculator); }

    private void Update()
    {
        TimeBar.value = timeManager.gameTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class QuizDataLoader : MonoBehaviour
{
    QuizImageLoader quizImageLoader;
    [SerializeField] List<SoalData> CurrLoadedSoals = new List<SoalData>();
    [SerializeField] List<QuizData> AllQuizDatas = new List<QuizData>();

    public static QuizDataLoader Instance;
    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        quizImageLoader = GetComponent<QuizImageLoader>();
        CurrLoadedSoals = APIManager.Instance.Soals;

        AllQuizDatas.ForEach(q => q.Clear()); //load question online, save correct answer progression later 
    }

    public List<QuizData> GetQuizDatas() { return AllQuizDatas; }
    public QuizData GetQuizData(int index) { return AllQuizDatas.ElementAt(index); }
    public void LoadSoal(QuizData.Paket_Enum _kodePaket)
    {
        APIRequests.sukses = false;

        QuizData qd = null;
        AllQuizDatas.ForEach(x => {
            if (x.Paket.Equals(_kodePaket)){
                qd = x;
                //Debug.Log("KodePaket A : " + _kodePaket);
                //Debug.Log("KodePaket B : " + x.Paket);
            }
        });

        qd.Topik = QuizDataManager.Instance.GetCurrentTopik();
        qd.Kelas = QuizDataManager.Instance.GetCurrentClass();
        qd.Semester = QuizDataManager.Instance.GetCurrentSemester();
        qd.KategoriSoal = QuizDataManager.Instance.GetCurrentQuizKategoriSoal();

        qd.Clear();

        CurrLoadedSoals.Clear();

        APIManager.Instance.Soals.ForEach(x => {
            if (x.semester_label.Equals(QuizDataManager.Instance.GetCurrentSemester().ToString()))
            {
                if (x.topic_id.Equals(QuizDataManager.Instance.GetCurrentTopik())){
                   if(x.packet_label.Equals(_kodePaket.ToString())) CurrLoadedSoals.Add(x);
                }
            } 
        });

        Task task = InjectSoalData(_kodePaket);

        Task continuationTask = task.ContinueWith((encryptTask) =>
        {
            APIRequests.sukses = true;
            Debug.Log("sukses = "+ APIRequests.sukses);
        });
    }

    private async Task InjectSoalData(QuizData.Paket_Enum _kodePaket) //find how to add sprite into list
    {
        for (int i = 0; i < CurrLoadedSoals.Count; i++)
        {   
            QuizData.QuestionData soal = new QuizData.QuestionData();

            switch (CurrLoadedSoals[i].answer)
            {
                case "1": soal.Answer = "A"; break;
                case "2": soal.Answer = "B"; break;
                case "3": soal.Answer = "C"; break;
                case "4": soal.Answer = "D"; break;
            }

            if (soal.QuestionReference == null) soal.QuestionReference = new List<Sprite>();
            if (soal.ExplanationReference == null) soal.ExplanationReference = new List<Sprite>();

            CurrLoadedSoals[i].details.ForEach(async x => 
            {
                Sprite sprite = await quizImageLoader.LoadSprite(x.file_url);
                if (x.is_question_image == "0") soal.QuestionReference.Add(sprite);
                else if (x.is_question_image == "1") soal.ExplanationReference.Add(sprite);
            });

            AllQuizDatas.Find(q => q.Paket == _kodePaket).QuestionList.Add(soal);
        }
    }
}

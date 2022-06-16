using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dangl.Calculator;

public class HasilBelajarManager : MonoBehaviour
{
    public static HasilBelajarManager Instance;
    public HasilBelajarData RaporData;
    public TextAsset File;
    public HasilBelajarData.KategoriData CurrentKategoriData;
    public List<Row> rowList = new List<Row>();
    
    public string TA;

    string[] _ta;
    string[][] Soal;

    [System.Serializable]
    public class Row
    {
        public string Level;
        public string Index;
        public string Soal;
        public string Jawaban;
        public float TimeEasy;
        public float TimeMedium;
        public float TimeHard;

    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        Soal = CsvParser2.Parse(File.text);
        LoadSoal();

        UpdateLatestKategori();
    }

    /*public void SaveToTemporary() {
        RaporData.kategoriData_Saved = CurrentKategoriData;
    }

    public void LoadLastKategori()*/

    public void UpdateLatestKategori()
    {
        ResetData();
        TA = PlayerPrefs.GetString("TA_savedata");
        _ta = TA.Split('_');

        int level = 1;
        try
        {
            level = int.Parse(_ta[1]);
            SetLevel(level, int.Parse(_ta[0]));

            switch (_ta[2])
            {
                case "true":
                    MaxLevelBeenReached(true);
                    level = 27;
                    break;
                default: MaxLevelBeenReached(false); break;
            }

            for (int i = 0; i < RaporData.kategoriData.Count; i++)
            {
                int nomor = i + 3;
                RaporData.kategoriData[i].soalTergarap = int.Parse(_ta[nomor]);

                if (RaporData.kategoriData[i].MinLevel <= level && level <= RaporData.kategoriData[i].MaxLevel)
                {
                    for (int x = RaporData.kategoriData[i].MinLevel; x <= RaporData.kategoriData[i].MaxLevel; x++)
                    {
                        if (level.Equals(x))
                        {
                            RaporData.kategoriData[i].skor += RaporData.LevelTerakhir.skor;
                            break;
                        }
                        RaporData.kategoriData[i].skor += GetExpMaxOfThisLevel(x);
                    }

                    RaporData.LevelTerakhir.currentKategori = i;
                }
                else if (RaporData.kategoriData[i].MaxLevel < level)
                {
                    int level_count = RaporData.kategoriData[i].MaxLevel - RaporData.kategoriData[i].MinLevel;
                    level_count += 1;

                    int exp = GetExpMaxOfThisLevel(RaporData.kategoriData[i].MaxLevel);
                    RaporData.kategoriData[i].skor = level_count * exp;
                    CheckBintang(i);
                }
            }
        }
        catch {

        }
        

        //  APIManager.Instance.PostTrainingArenaSaveData("0","1",false,"0","0","0","0","0");

        CurrentKategoriData = RaporData.kategoriData[RaporData.LevelTerakhir.currentKategori];
        if (CurrentKategoriData.Equals(null)) {
            CurrentKategoriData = RaporData.kategoriData[0];
            RaporData.LevelTerakhir.currentKategori = 0;
        }
    }

    public void OnCorrectAnswer(int level, int exp)
    {
        if (!hasMaxLevelBeenReached())
        {
            if (level >= CurrentKategoriData.MinLevel && level <= CurrentKategoriData.MaxLevel)
            {
                ScoreIncreased(exp);
                CountSoalIncreased();
            }
        }
    }

    public void ScoreIncreased(int exp) { if (!hasMaxLevelBeenReached()) CurrentKategoriData.skor += exp; }

    public void CountSoalIncreased() { if (!hasMaxLevelBeenReached()) CurrentKategoriData.soalTergarap++; }

    public void CheckBintang(int currentKategori)
    {
        HasilBelajarData.KategoriData temp = RaporData.kategoriData[currentKategori];
        if(temp.soalTergarap != 0)
        {
            if (temp.soalTergarap <= temp.sangatBaik)
            {
                temp.bintang = 3;
            }
            else if (temp.soalTergarap <= temp.Baik)
            {
                temp.bintang = 2;
            }
            else if (temp.soalTergarap > temp.Cukup)
            {
                temp.bintang = 1;
            }
        }
    }

    public void ChangeKategori(int level)
    {
        if (!hasMaxLevelBeenReached())
        {
            if (level >= CurrentKategoriData.MinLevel && level <= CurrentKategoriData.MaxLevel)
            {

            }
            else
            {
                RaporData.kategoriData[RaporData.LevelTerakhir.currentKategori] = CurrentKategoriData;
                RaporData.LevelTerakhir.currentKategori++;
                CurrentKategoriData = RaporData.kategoriData[RaporData.LevelTerakhir.currentKategori];
            }
        }
    }

    //private void OnApplicationQuit()
    //{
    //    RaporData.kategoriData[RaporData.LevelTerakhir.currentKategori] = CurrentKategoriData;
    //}

    public bool hasMaxLevelBeenReached(){ return RaporData.ParameterQuizPlayer.PlayerLevelHasReachedTheMax; }
    public void MaxLevelBeenReached(bool status) { RaporData.ParameterQuizPlayer.PlayerLevelHasReachedTheMax = status; }

    public int GetMaxLevel(){ return RaporData.ParameterQuizPlayer.LevelMax; }

    public int GetCurrentKategori() { return RaporData.LevelTerakhir.currentKategori; }

    public int GetEasy() { return RaporData.ParameterQuizPlayer.RewardQuizEasy;}

    public int GetMedium()  { return RaporData.ParameterQuizPlayer.RewardQuizMedium;}

    public int GetHard() { return RaporData.ParameterQuizPlayer.RewardQuizHard;}

    public int GetExpMaxOfThisLevel(int level)
    {
        if (level >= RaporData.ParameterQuizPlayer.EXPMaxStartToChangeAtLevel)
            return RaporData.ParameterQuizPlayer.EXPMaxTrainingArenaQuiz[1];
        else
            return RaporData.ParameterQuizPlayer.EXPMaxTrainingArenaQuiz[0];
    }

    public void SetLevel(int level, int exp) {
        RaporData.LevelTerakhir.level = level;
        RaporData.LevelTerakhir.skor = exp;
    }

    public void SaveProgress(int level, int exp)
    {
        SetLevel(level, exp);
        RaporData.kategoriData[RaporData.LevelTerakhir.currentKategori] = CurrentKategoriData;

        string _skor = exp.ToString();
        string _level = level.ToString();
        bool _isTamat = hasMaxLevelBeenReached();
        string _cat1 = RaporData.kategoriData[0].soalTergarap.ToString();
        string _cat2 = RaporData.kategoriData[1].soalTergarap.ToString();
        string _cat3 = RaporData.kategoriData[2].soalTergarap.ToString();
        string _cat4 = RaporData.kategoriData[3].soalTergarap.ToString();
        string _cat5 = RaporData.kategoriData[4].soalTergarap.ToString();

        APIManager.Instance.PostTrainingArenaSaveData(_skor, _level, _isTamat, _cat1, _cat2, _cat3, _cat4, _cat5);
    }

    public List<int> GetLevel()
    {
        List<int> temp = new List<int>(2);
        temp.Add(RaporData.LevelTerakhir.level);
        temp.Add(RaporData.LevelTerakhir.skor);

        return temp;
    }

    public string Hitung(string myQuestion)
    {
        string formula = myQuestion;
        string answer = "";

        char[] formu = formula.ToCharArray();
        for (int i = 0; i < formu.Length; i++)
        {
            if (formu[i] == 'x') formu[i] = '*';
            else if (formu[i] == ':') formu[i] = '/';
        }
        formula = new string(formu);
        var hasil = Calculator.Calculate(formula);
        answer = "" + (int)hasil.Result;

        return answer;
    }

    public void LoadSoal()
    {
        rowList.Clear();
        for (int i = 1; i < Soal.Length; i++)
        {
            Row row = new Row();
            row.Level = Soal[i][0];
            row.Index = Soal[i][1];
            row.Soal = Soal[i][2];
            row.Jawaban = Hitung(row.Soal);
            row.TimeEasy = (float.Parse(Soal[i][4]) * 0.1f);
            row.TimeMedium = (float.Parse(Soal[i][5]) * 0.1f);
            row.TimeHard = (float.Parse(Soal[i][6]) * 0.1f);

            rowList.Add(row);
        }
    }
    
    void ResetData()
    {
        RaporData.LevelTerakhir.level = 1;
        RaporData.LevelTerakhir.skor = 0;
        RaporData.LevelTerakhir.currentKategori = 0;

        RaporData.ParameterQuizPlayer.EXPMaxStartToChangeAtLevel = 10;
        RaporData.ParameterQuizPlayer.LevelMax = 26;
        RaporData.ParameterQuizPlayer.RewardQuizEasy = 3;
        RaporData.ParameterQuizPlayer.RewardQuizMedium = 4;
        RaporData.ParameterQuizPlayer.RewardQuizHard = 6;
        RaporData.ParameterQuizPlayer.PlayerLevelHasReachedTheMax = false;

        RaporData.ParameterQuizPlayer.EXPMaxTrainingArenaQuiz[0]= 12;
        RaporData.ParameterQuizPlayer.EXPMaxTrainingArenaQuiz[1] = 24;

        RaporData.kategoriData.Clear();

        HasilBelajarData.KategoriData temp1 = new HasilBelajarData.KategoriData();
        temp1.nama = "Berhitung Dasar 1";
        temp1.skor = temp1.soalTergarap = temp1.bintang = 0;
        temp1.MaxLevel = 4;
        temp1.MinLevel = 1;
        temp1.sangatBaik = 9;
        temp1.Baik = temp1.Cukup = 12;

        HasilBelajarData.KategoriData temp2 = new HasilBelajarData.KategoriData();
        temp2.skor = temp2.soalTergarap = temp2.bintang = 0;
        temp2.MaxLevel = 9;
        temp2.MinLevel = 5;
        temp2.sangatBaik = 11;
        temp2.Baik = temp2.Cukup = 15;
        temp2.nama = "Berhitung Dasar 2";

        HasilBelajarData.KategoriData temp3 = new HasilBelajarData.KategoriData();
        temp3.skor = temp3.soalTergarap = temp3.bintang = 0;
        temp3.MaxLevel = 16;
        temp3.MinLevel = 10;
        temp3.sangatBaik = 30;
        temp3.Baik = temp3.Cukup = 44;
        temp3.nama = "Berhitung Lanjut 1";

        HasilBelajarData.KategoriData temp4 = new HasilBelajarData.KategoriData();
        temp4.skor = temp4.soalTergarap = temp4.bintang = 0;
        temp4.MaxLevel = 22;
        temp4.MinLevel = 17;
        temp4.sangatBaik = 26;
        temp4.Baik = temp4.Cukup = 38;
        temp4.nama = "Berhitung Lanjut 2";

        HasilBelajarData.KategoriData temp5 = new HasilBelajarData.KategoriData();
        temp5.skor = temp5.soalTergarap = temp5.bintang = 0;
        temp5.MaxLevel = 26;
        temp5.MinLevel = 23;
        temp5.sangatBaik = 18;
        temp5.Baik = temp5.Cukup = 26;
        temp5.nama = "Berhitung Terampil";

        RaporData.kategoriData.Add(temp1);
        RaporData.kategoriData.Add(temp2);
        RaporData.kategoriData.Add(temp3);
        RaporData.kategoriData.Add(temp4);
        RaporData.kategoriData.Add(temp5);
    }
}

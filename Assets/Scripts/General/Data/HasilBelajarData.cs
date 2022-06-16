using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HasilBelajarData", menuName = "Datas/Players/HasilBelajarData", order = 0)]
public class HasilBelajarData : ScriptableObject {

    public LevelLatihanBerhitung LevelTerakhir;
    public ParameterLatihanBerhitung ParameterQuizPlayer;
   // public KategoriData kategoriData_Saved;
    public List<KategoriData> kategoriData;

    [System.Serializable]
    public class LevelLatihanBerhitung
    {
        public int level;
        public int skor;
        public int currentKategori;
    }

    [System.Serializable]
    public class ParameterLatihanBerhitung
    {
        public int[] EXPMaxTrainingArenaQuiz;
        public int EXPMaxStartToChangeAtLevel;
        public int LevelMax;
        public int RewardQuizEasy;
        public int RewardQuizMedium;
        public int RewardQuizHard;
        public bool PlayerLevelHasReachedTheMax;
    }

    [System.Serializable]
    public class KategoriData
    {
        [Header("Nama Kategori")]
        public string nama;
        [Space(1)]
        public int skor;
        public int bintang;
        [Space(1)]
        [Header("Letak Stage")]
        public int MinLevel;
        public int MaxLevel;
        [Space(1)]
        [Header("Jumlah Soal yang Telah Dikerjakan Benar")]
        public int soalTergarap;
        [Space(1)]
        [Header("Limit Jumlah Soal Benar")]
        public int sangatBaik;
        public int Baik;
        public int Cukup;
    }
}
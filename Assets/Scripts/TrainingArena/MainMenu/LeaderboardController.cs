using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class LeaderboardController : MonoBehaviour
{
    public TextMeshProUGUI Highest_Score;
    public GameObject LeaderboardContainer;
    public GameObject viewport, prefab;
    public Scrollbar scrollbar;
    public int MaxScoreView = 25;
    //public List<TextMeshProUGUI> NameList;
    //public List<TextMeshProUGUI> ScoreList;

    public Button BackBtn;

    private int playerId;
    private int curIndex;
    private List<kamus> sortedScoreDict = new List<kamus>();
    //private Dictionary<string, int> sortedScoreDict = new Dictionary<string, int>();
    //private MainMenuController mainMenuController;


    [Header("DEBUG_MODE")]
    [Space(5)]
    public bool IsDebugMode;
    public int DebugPlayerId;

    class kamus {
        string name;
        int score;

        public void Add(string name, int score) { this.name = name; this.score = score;  }
        public string GetName() { return name; }
        public int GetScore() { return score; }
    }

    void Awake()
    {
        //mainMenuController = FindObjectOfType<MainMenuController>();
        Init();    
    }
    void Start()
    {
        if (!IsDebugMode)
        {
            playerId = PlayerPrefs.GetInt("PlayerId");
        }
        else
        {
            ResetData();
            playerId = DebugPlayerId;
        }

        SortScore();
        DisplayLeaderboard();

       // scrollbar.value = 1;
       //LeaderboardContainer.SetActive(false);
    }

    private void Update()
    {
      /*  try
        {
            display_Highest();
        }
        catch { }*/
    }

    private void display_Highest()
    {
        Highest_Score.text = sortedScoreDict.First().GetScore().ToString();
    }

    private void Init()
    {
        BackBtn.onClick.AddListener(delegate
        {
           // scrollbar.value = 1;
           // LeaderboardContainer.gameObject.SetActive(false);
        });
    }
    private void DisplayLeaderboard()
    {
        //Kalo Dictionary
        //foreach (KeyValuePair<string, int> entry in sortedScoreDict)
        foreach (kamus entry in sortedScoreDict)
        {
            if (curIndex >= MaxScoreView)
            {
                break;
            }
            
            GameObject temp = Instantiate(prefab);
            temp.transform.SetParent(LeaderboardContainer.transform);
            temp.transform.localScale = new Vector3(1, 1, 1);
            temp.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = entry.GetName();
            temp.transform.Find("PlayerScore").GetComponent<TextMeshProUGUI>().text = entry.GetScore().ToString();
            //Kalo Dictionary
            /*NameList[curIndex].text = entry.Key;
            ScoreList[curIndex].text = entry.Value.ToString();*/
            curIndex++;
        }
    }
    private void SortScore()
    {
        kamus temp = new kamus();
        if (!IsDebugMode)
        {
            for (int i = 0; i < playerId; i++)
            {
                temp = new kamus();
                if (PlayerPrefs.HasKey("PlayerName_" + i))
                {
                    Debug.Log(PlayerPrefs.GetString("PlayerName_" + i));

                    temp.Add(PlayerPrefs.GetString("PlayerName_" + i), PlayerPrefs.GetInt("PlayerScore_" + i));

                    //kalo dictionary
                    //sortedScoreDict.Add(PlayerPrefs.GetString("PlayerName_" + i), PlayerPrefs.GetInt("PlayerScore_" + i));
                }
                else
                {
                    temp.Add("Anonymous", 0);
                    //kalo dictionary
                    //sortedScoreDict.Add("Anonymous", 0);
                }
                sortedScoreDict.Add(temp);
            }
        }
        else
        {
            for (int i = 0; i < playerId; i++)
            {
                int randScore = Random.Range(1, 100);
                temp = new kamus();
                temp.Add("PlayerName_" + i, randScore);
                sortedScoreDict.Add(temp);

                //kalo dictionary
                //sortedScoreDict.Add("PlayerName_" + i, randScore);
            }
        }

        sortedScoreDict = sortedScoreDict.OrderByDescending(x => x.GetScore()).ToList();
        //kalo dicitionary
        //sortedScoreDict = sortedScoreDict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
    }
    private void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }
}

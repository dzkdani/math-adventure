using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
 
public class GameOverController : MonoBehaviour
{
    [Header("STAGE CLEAR")]
    [Space(5)]
    [SerializeField]
    private GameObject stageClearContainer;
    [SerializeField]
    private Button stageClearQuitGameBtn;

    [Header("GAME OVER")]
    [Space(5)]
    [SerializeField]
    private GameObject gameOverContainer;
    [SerializeField]
    private Button gameOverQuitGameBtn;

    [Header("PARAMETER")]
    [Space(5)]
    [HideInInspector]
    public bool IsGameOver;

    public static GameOverController Instance;
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
        
        InitListenter();
    }
    private void InitListenter()
    {
        stageClearQuitGameBtn.onClick.AddListener(delegate
        {
            BtnBackManager.instance.W = null;
            SceneManager.LoadScene("GameMenu");
        });

        gameOverQuitGameBtn.onClick.AddListener(delegate
        {
            BtnBackManager.instance.W = null;
            SceneManager.LoadScene("GameMenu");
        });
    }

    public void StageClear()
    {
        IsGameOver = true;
        BtnBackManager.instance.W = null;
        stageClearContainer.SetActive(true);

        //counting stars
        int stars = 0;
        if (HUDController.Instance.Timer > 30)
        {
            stars = 3;
        }
        else if (HUDController.Instance.Timer > 15)
        {
            stars = 2;
        }
        else
        {
            stars = 1;
        }

        PlayerDataManager.Instance.ClearedStage(StageManager.Instance.GetCurStageData(), stars);
    }

    public void GameOver()
    {
        IsGameOver = true;
        BtnBackManager.instance.W = null;
        gameOverContainer.SetActive(true);
    }
    
    public bool CheckGameOver() { return IsGameOver; }
}

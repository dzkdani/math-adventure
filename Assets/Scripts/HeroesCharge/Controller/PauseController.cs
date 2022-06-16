using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 
public class PauseController : MonoBehaviour
{
    [SerializeField] 
    private bool Paused;
    [SerializeField]
    private GameObject PauseContainer;
    public Button resumeBtn;
    public Button quitBtn;

    public static PauseController Instance;
    private void Awake() 
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
    }

    private void Start() {
        Paused = false;
        resumeBtn.onClick.AddListener(delegate { Pause(); });
        quitBtn.onClick.AddListener(delegate { Quit(); });
    }

    public void Pause()
    {
        if (Paused)
        {
            Paused = false;
            PauseContainer.SetActive(false);
        }
        else
        {
            Paused = true;
            PauseContainer.SetActive(true);
        }
    }

    public bool IsPaused() { return Paused; }

    public void Quit() { 
        BtnBackManager.instance.W = null;
        SceneManager.LoadScene("GameMenu"); 
    }
}

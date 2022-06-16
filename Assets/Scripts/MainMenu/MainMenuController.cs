using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;  

public class MainMenuController : MonoBehaviour
{
    [Header("Menu Components")]
    [Space(5)]
    //public GameObject NotifMenu;
    public GameObject TryoutMenu;
    public GameObject ProfilMenu;
    public GameObject BerlanggananMenu;
    public GameObject HasilBelajarMenu;
    public GameObject MainMenu;
    public GameObject VoucherMenu;

    [Header("Button Components")]
    [Space(5)]
    [SerializeField] Button PlayBtn;
    [SerializeField] Button ProfileBtn;
    [SerializeField] Button NotifBtn;
    [SerializeField] Button TryoutBtn;
    [SerializeField] Button BerlanggananBtn;
    [SerializeField] Button HasilBelajarBtn;
    [SerializeField] Button IntroBtn;

    public static MainMenuController Instance;
    
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Init();
    }

    private void OnEnable() {
        Init();
    }

    private void Start()
    {
        InitDisplayRaportLatihanBerhitung();
    }

    void Init()
    {
        MainMenu.SetActive(true);
        ProfilMenu.SetActive(false);
        //NotifMenu.SetActive(false);
        //TryoutMenu.SetActive(false);
        BerlanggananMenu.SetActive(false);
        HasilBelajarMenu.SetActive(false);

        PlayBtn.onClick.AddListener( () => {OnPlayButton();});
        ProfileBtn.onClick.AddListener( () => {OnMenuOpened(ProfilMenu);});
        NotifBtn.onClick.AddListener( () => {  }); //notif menu coming soon
        TryoutBtn.onClick.AddListener( () => {  }); //tryout menu coming soon
        IntroBtn.onClick.AddListener( () => { SceneManager.LoadScene("Intro"); }); //for watch intro again
        BerlanggananBtn.onClick.AddListener( () => {OnMenuOpened(BerlanggananMenu);});
        HasilBelajarBtn.onClick.AddListener( () => {OnMenuOpened(HasilBelajarMenu);});
    }

    public void OnPlayButton()
    { 
        // subscribtion check
        // if (!PlayerDataManager.Instance.OnSubscription()) 
        // {
        //     OnMenuOpened(VoucherMenu);
        // }
        // else
        // {
        //     SceneManager.LoadScene("GameMenu");
        // }
        SceneManager.LoadScene("GameMenu");
    }

    public void OnMenuOpened(GameObject _menu)
    {
        if (_menu != null)
        {
            if (!_menu.activeInHierarchy)
            {
                _menu.SetActive(true);
                MainMenu.SetActive(false);
                TweenMenu(_menu);
            }
        }
        else
        {
            Debug.Log("Menu Panel Not Found");
        }
    }

    public void OnBackToMainMenu()
    {
        if (!MainMenu.activeInHierarchy)
        {
            MainMenu.SetActive(true);
            TweenMenu(MainMenu);
        }
        else
            return;
    }

    void TweenMenu(GameObject _menu)
    {
        _menu.GetComponent<RectTransform>().localScale = Vector3.zero;
        _menu.GetComponent<RectTransform>().DOScale(Vector3.one, 0.5f);
    }

    #region HASIL_BELAJAR
    public List<Text> StatusCountingSpeed;
    public Slider PencapaianLevel;

    public void PlaySFX(ExtendedButtonOnClickPassingVariable e)
    {
        AudioManager.Instance.PlaySFX(e._integer[0], e._onClick);
    }

    void InitDisplayRaportLatihanBerhitung()
    {
        string status = "";
        int Kategori = HasilBelajarManager.Instance.GetCurrentKategori();

        for (int i = 0; i < HasilBelajarManager.Instance.RaporData.kategoriData.Capacity; i++)
        {
            if (HasilBelajarManager.Instance.hasMaxLevelBeenReached() || i < Kategori) HasilBelajarManager.Instance.CheckBintang(i);
            int bintang = HasilBelajarManager.Instance.RaporData.kategoriData[i].bintang;
            switch (bintang)
            {
                case 0:
                    status = "";
                    break;
                case 1:
                    status = "Cukup";
                    break;
                case 2:
                    status = "Baik";
                    break;
                case 3:
                    status = "Sangat Baik";
                    break;
            }
            StatusCountingSpeed[i].text = status;
        }

        if (HasilBelajarManager.Instance.hasMaxLevelBeenReached()) PencapaianLevel.value = 26;
        else PencapaianLevel.value = HasilBelajarManager.Instance.GetLevel()[0];
    }
    #endregion
}

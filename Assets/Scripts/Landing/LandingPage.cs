using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LandingPage : MonoBehaviour
{
    [Header("Landing Components")]
    [Space(5)]
    [SerializeField] GameObject loginCheckPanel;
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject registerPanel;
    [SerializeField] Button loginBtn;
    [SerializeField] Button registerBtn;
    [SerializeField] Button forgotPassBtn;
    [SerializeField] TMP_InputField userInput;
    [SerializeField] TMP_InputField passInput;
    [SerializeField] TextMeshProUGUI loginLog;

    public Login login;
    public Register register;

    public static LandingPage Instance;

    private void Awake() {
        if (Instance == null) Instance = this;
    }

    private bool HasToken() => PlayerPrefs.HasKey("token");

    private IEnumerator CheckToken()
    {
        loginCheckPanel.SetActive(true);
        PlayerPrefs.DeleteKey("person_id");
        yield return StartCoroutine(APIManager.Instance.api.URL("account").Get<Response<AccountData>>(APIManager.Instance.ResponseHandler, ResponseType.account));
        if (PlayerPrefs.HasKey("person_id")) StartCoroutine(APIManager.Instance.LandingPageHandler());
        else Init();
    }

    private void Start() 
    {
        if(HasToken()) StartCoroutine(CheckToken());
        else Init();
    }

    private void OnEnable() {
        Init();
    }

    private void Init()
    {
        loginCheckPanel.SetActive(false);

        loginLog.text = "";

        loginPanel.SetActive(true);
        registerPanel.SetActive(false);

        loginBtn.interactable = true;
        loginBtn.onClick.RemoveAllListeners();
        loginBtn.onClick.AddListener(delegate 
        {
            AudioManager.Instance.PlaySFX(4);
            login.OnLogin(userInput.text, passInput.text);
        });

        registerBtn.onClick.RemoveAllListeners();
        registerBtn.onClick.AddListener(delegate { OpenRegisterPanel(); });

        forgotPassBtn.onClick.RemoveAllListeners();
        forgotPassBtn.onClick.AddListener(delegate {OnForgotPassword();});
    }

    private void OnForgotPassword()
    {
        userInput.text = "";
        passInput.text = "";
        Debug.Log("Lupa Password?");
    }

    private void TweenUI(GameObject _panel)
    {
        _panel.GetComponent<RectTransform>().localScale = Vector3.zero;
        _panel.GetComponent<RectTransform>().DOScale(Vector3.one, 0.5f);
    }

    private void OpenRegisterPanel()
    {
        AudioManager.Instance.PlaySFX(4);
        loginPanel.SetActive(false);
        userInput.text = "";
        passInput.text = "";
        registerPanel.SetActive(true);

        TweenUI(registerPanel);        
    }

    public void CloseRegisterPanel()
    {
        AudioManager.Instance.PlaySFX(5);
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);

        TweenUI(loginPanel);
    }

    
}

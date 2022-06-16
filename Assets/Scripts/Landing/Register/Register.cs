using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Register : MonoBehaviour
{
    [Header("Input Text Placeholder")]
    [Space(5)]
    [SerializeField] const string REGISTERNAMA = "masukkan nama lengkapmu....";
    [SerializeField] const string REGISTERSEKOLAH = "masukkan asal sekolahmu....";

    [Header("Register Data")]
    [Space(5)]
    [SerializeField] string namaLengkap;
    [SerializeField] Kelas kelas;
    [SerializeField] int kelasID;
    [SerializeField] string sekolah;
    [SerializeField] string sekolahID;
    [SerializeField] string username;
    [SerializeField] string password;

    [Header("Register Component")]
    [Space(5)]
    [SerializeField] Button backBtn;
    [SerializeField] GameObject inputPanel;
    [SerializeField] GameObject kelasPanel;
    [SerializeField] GameObject userPassPanel;
    [SerializeField] TMP_Dropdown sekolahDropdown;
    [SerializeField] TMP_InputField userInputField;
    [SerializeField] TMP_InputField passInputField;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI inputText;
    [SerializeField] Button lanjutBtn;
    [SerializeField] Image progressImg;
    [SerializeField] TextMeshProUGUI RegisterLog;

    [Header("Progress Register")]
    [Space(5)]
    [SerializeField] List<Sprite> progressSprites;
    [SerializeField] List<Button> kelasBtns;
    [SerializeField] int currProgress;
    [SerializeField] bool isUserFormat;
    [SerializeField] bool isPassFormat;

#region Init
    private void OnEnable() 
    {
        StartCoroutine(APIManager.Instance.GetCategoryListRegister());
        Init();
    }

    private void Init()
    {
        isUserFormat = false;
        isPassFormat = false;

        inputPanel.SetActive(true);
        kelasPanel.SetActive(false);
        userPassPanel.SetActive(false);

        inputField.gameObject.SetActive(true);
        sekolahDropdown.gameObject.SetActive(false);

        inputField.text = null;
        inputField.placeholder.GetComponent<TextMeshProUGUI>().text = REGISTERNAMA;

        lanjutBtn.gameObject.SetActive(true);
        lanjutBtn.onClick.RemoveAllListeners();
        lanjutBtn.onClick.AddListener(delegate { OnNamaRegistered(); } );
        lanjutBtn.interactable = false;

        inputField.onEndEdit.RemoveAllListeners();
        inputField.onEndEdit.AddListener(delegate {OnInputNama(inputField.text);});

        sekolahDropdown.ClearOptions();
        List<string> dropdownBaseOption = new List<string>();
        dropdownBaseOption.Add("Pilih Sekolah");
        dropdownBaseOption.Add("Sekolah Lainnya");
        sekolahDropdown.AddOptions(dropdownBaseOption);
        
        sekolahDropdown.onValueChanged.RemoveAllListeners();
        sekolahDropdown.onValueChanged.AddListener(delegate {OnSelectSekolah(sekolahDropdown.value);});

        RegisterLog.text = null;

        currProgress = 0;
        progressImg.sprite = progressSprites[currProgress];

        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(delegate {OnBack();});

        sekolahDropdown.value = 0;
    }

    private void OnBack()
    {
        AudioManager.Instance.PlaySFX(5);
        switch (currProgress)
        {
            case 0: OnRegisterCanceled(); 
                break;
            case 1: Init();
                break;
            case 2: OnNamaRegistered();
                break;
            case 3: OnKelasRegistered();
                break;
        }
        progressImg.sprite = progressSprites[currProgress];
    }

    private void TweenUI(GameObject _panel)
    {
        _panel.GetComponent<RectTransform>().localScale = Vector3.zero;
        _panel.GetComponent<RectTransform>().DOScale(Vector3.one, 0.5f);
    }
#endregion

#region Nama
    private void OnNamaRegistered()
    {
        AudioManager.Instance.PlaySFX(4);
        lanjutBtn.onClick.RemoveAllListeners();
        lanjutBtn.onClick.AddListener(delegate { OnKelasRegistered(); } );
        lanjutBtn.interactable = false;

        inputField.onEndEdit.RemoveAllListeners();

        inputPanel.SetActive(false);
        kelasPanel.SetActive(true);
        userPassPanel.SetActive(false);

        foreach (Button kelasBtn in kelasBtns)
        {
            kelasBtn.onClick.AddListener(delegate { OnSelectKelas(kelasBtn); } );
            kelasBtn.GetComponent<Image>().color = Color.white;
        }

        TweenUI(kelasPanel);

        RegisterLog.text = null;

        currProgress = 1;
        progressImg.sprite = progressSprites[currProgress];
    }

    public void OnInputNama(string _input)
    {
        AudioManager.Instance.PlaySFX(6);
        if (_input.Length > 0)
        {
            lanjutBtn.interactable = true;
            namaLengkap = _input;
        }
    }
#endregion

#region Kelas
    private void OnKelasRegistered()
    {
        lanjutBtn.onClick.RemoveAllListeners();
        lanjutBtn.onClick.AddListener(delegate { OnSekolahRegistered(); } );
        lanjutBtn.interactable = false;

        inputPanel.SetActive(true);
        kelasPanel.SetActive(false);
        userPassPanel.SetActive(false);

        inputField.gameObject.SetActive(false);
        sekolahDropdown.gameObject.SetActive(true);

        sekolahDropdown.value = 0;

        TweenUI(inputPanel);

        RegisterLog.text = null;

        currProgress = 2;
        progressImg.sprite = progressSprites[currProgress];

        sekolahDropdown.AddOptions(APIManager.Instance.CategoryDataListBy("school", "label"));
    }

    public void OnSelectKelas(Button kelasBtn)
    {
        AudioManager.Instance.PlaySFX(4);
        if (kelasBtn.GetComponent<Image>().color == Color.green)
        {
            return;
        }
        else
        {
            foreach (Button klsBtn in kelasBtns)
            {
                klsBtn.GetComponent<Image>().color = Color.white;
            }

            kelasBtn.GetComponent<Image>().color = Color.green;

            switch (kelasBtn.name)
            {
                case "Kelas3" : kelas = Kelas.Kelas3;
                                kelasID = 3001;
                break;
                case "Kelas4" : kelas = Kelas.Kelas4;
                                kelasID = 3002;
                break;
                case "Kelas5" : kelas = Kelas.Kelas5;
                                kelasID = 3003;
                break;
                case "Kelas6" : kelas = Kelas.Kelas6;
                                kelasID = 3004;
                break;
            }

            AudioManager.Instance.PlaySFX(6);

            lanjutBtn.interactable = true;
        }
    }
#endregion

#region Sekolah
    private void OnSekolahRegistered()
    {
        AudioManager.Instance.PlaySFX(4);
        lanjutBtn.onClick.RemoveAllListeners();
        lanjutBtn.onClick.AddListener(delegate { OnUserPassRegistered(); } );
        lanjutBtn.interactable = false;

        inputPanel.SetActive(false);
        kelasPanel.SetActive(false);
        userPassPanel.SetActive(true);

        userInputField.onEndEdit.RemoveAllListeners();
        userInputField.onEndEdit.AddListener(delegate {OnInputUsername(userInputField.text);});
        passInputField.onEndEdit.RemoveAllListeners();
        passInputField.onEndEdit.AddListener(delegate {OnInputPassword(passInputField.text);});

        TweenUI(userPassPanel);

        RegisterLog.text = null;

        currProgress = 3;
        progressImg.sprite = progressSprites[currProgress];
    }

    private void OnSelectSekolah(int dropdownIndex)
    {
        AudioManager.Instance.PlaySFX(4);
        if (dropdownIndex != 0 && dropdownIndex != 1)
        {
            sekolah = "";
            sekolahID = APIManager.Instance.CategoryDataBy<int>("id", null, sekolahDropdown.options[dropdownIndex].text, null).ToString();

            AudioManager.Instance.PlaySFX(6);

            lanjutBtn.interactable = true;
        }
        else if (dropdownIndex == 1)
        {
            OnSelectSekolahOther();
        }
        else 
        {
            sekolah = null;
            lanjutBtn.interactable = false;
        }
    }

    private void OnSelectSekolahOther()
    {
        AudioManager.Instance.PlaySFX(6);
        inputField.text = null;
        inputField.placeholder.GetComponent<TextMeshProUGUI>().text = REGISTERSEKOLAH;
        inputField.gameObject.SetActive(true);
        sekolahDropdown.gameObject.SetActive(false);

        inputField.onEndEdit.AddListener(delegate {OnRegisterSekolahOther(inputField.text);});
    }

    public void OnRegisterSekolahOther(string _sekolahOther)
    {
        if (_sekolahOther.Length > 0)
        {
            sekolahID = "";
            sekolah = _sekolahOther;
            lanjutBtn.interactable = true;
        }
        else
            lanjutBtn.interactable = false;
    }
#endregion

#region UserPass
    private void OnUserPassRegistered()
    {
        AudioManager.Instance.PlaySFX(4);
        StartCoroutine(PostRegister());
    }

    public void OnInputUsername(string _input)
    {
        AudioManager.Instance.PlaySFX(6);
        if (_input.Length > 7) //no min 7 digit? kasih +62 didepan?
        {
            isUserFormat = true;
            username = _input;
        }
        else
        {
            isUserFormat = false;
            RegisterLog.text = "masukkan no wa yang benar ya!";
        }

        if (isUserFormat && isPassFormat)
        {
            RegisterLog.text = "";
            lanjutBtn.interactable = true;
        }
    }

    public void OnInputPassword(string _input)
    {
        AudioManager.Instance.PlaySFX(5);
        if (_input.Length > 5)
        {
            isPassFormat = true;
            password = _input;
        }
        else
        {
            isPassFormat = false;
            RegisterLog.text = "password harus lebih dari 6 huruf";
        }

        if (isUserFormat && isPassFormat)
        {
            RegisterLog.text = "";
            lanjutBtn.interactable = true;
        }
    }
#endregion

#region End Register
    private IEnumerator PostRegister()
    {
        AudioManager.Instance.PlaySFX(4);
        RegisterLog.text = "Berhasil mendaftar!";
        yield return StartCoroutine(APIManager.Instance.PostRegister(namaLengkap, kelasID, sekolahID, sekolah, username, password));
        RegisterLog.text = "";
    }

    public void OnRegisterCanceled()
    {
        AudioManager.Instance.PlaySFX(5);
        RegisterLog.text = null;
        namaLengkap = null;
        sekolah = null;
        username = null;
        password = null;
        
        inputField.text = null;
        foreach (Button button in kelasBtns)
        {
            button.GetComponent<Image>().color = Color.white;
        }
        userInputField.text = null;
        passInputField.text = null;

        LandingPage.Instance.CloseRegisterPanel();
    }
#endregion
}

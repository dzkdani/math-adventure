using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileController : MonoBehaviour
{
    PlayerData playerData;
    Kelas kelas;

    [Header("Display")]
    [Space(5)]
    [SerializeField] private TextMeshProUGUI namaLengkapTxt;
    [SerializeField] private TextMeshProUGUI sekolahTxt;
    [SerializeField] private ToggleGroup kelasDisplay;
    [SerializeField] private TextMeshProUGUI whatsappTxt;
    [SerializeField] private TextMeshProUGUI referralTxt;
    [SerializeField] private TextMeshProUGUI rewardPointTxt;
    [SerializeField] private TextMeshProUGUI subscribeEndTxt;

    [Header("Edit")]
    [Space(5)]
    [SerializeField] private TMP_InputField namaLengkapInput; 
    [SerializeField] private TMP_InputField sekolahInput;
    [SerializeField] private TMP_Dropdown sekolahDropdown;
    [SerializeField] private TMP_InputField whatsappInput;
    [SerializeField] private Button editButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button copyButton;
    [SerializeField] private Button voucherButton;
 
    private void Awake() {
        do
        {
            playerData = PlayerDataManager.Instance.PlayerData;
        } while (playerData == null);
    }

    private void OnEnable() => Init();

    private void OnDisable() => Init();

    void Init()
    {
        DisplayProfile();

        voucherButton.onClick.AddListener(delegate {InputVoucher();});
        copyButton.onClick.AddListener(delegate {CopyReferral();});
        editButton.onClick.AddListener(delegate {EditProfile();});
        cancelButton.onClick.AddListener(delegate {OnEditCanceled();});

        sekolahInput.onEndEdit.AddListener(delegate {sekolahTxt.text = sekolahInput.text;});
        sekolahDropdown.onValueChanged.AddListener(delegate {OnEditSekolah(sekolahDropdown.value);});
        sekolahDropdown.AddOptions(APIManager.Instance.categoryList.categories.SingleOrDefault(c => c.group == "school").GetCategoryDataListBy("label"));
    }

    void DisplayProfile()
    {
        namaLengkapTxt.text = playerData.PlayerProfileData.StudentName;
        namaLengkapInput.gameObject.SetActive(false);

        sekolahTxt.text = playerData.PlayerProfileData.StudentSchool;
        sekolahInput.gameObject.SetActive(false);
        sekolahDropdown.gameObject.SetActive(false);

        kelasDisplay.SetAllTogglesOff();
        kelas = playerData.PlayerProfileData.StudentClass;
        foreach (Toggle toggleKelas in kelasDisplay.GetComponentsInChildren<Toggle>())
        {
            if (toggleKelas.name == kelas.ToString())
            {
                toggleKelas.isOn = true;
            }
            else
            { 
                toggleKelas.isOn = false;
            }
            toggleKelas.interactable = false;
        } 

        whatsappTxt.text = playerData.PlayerProfileData.ParentWhatsapp;
        whatsappInput.gameObject.SetActive(false);

        subscribeEndTxt.text = "Aktif sampai "+playerData.PlayerSubscriptionData.SubscriptionEndDate;
        referralTxt.text = playerData.PlayerSubscriptionData.ReferralCode; 
        rewardPointTxt.text = playerData.PlayerSubscriptionData.RewardPoint+" Point";

        editButton.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        editButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);

        cancelButton.gameObject.SetActive(false);
    }

    void InputVoucher() 
    {
        this.gameObject.SetActive(false);
        MainMenuController.Instance.OnMenuOpened(SubscriptionController.Instance.submitVoucher.panel);
    }

    void CopyReferral()
    {
        TextEditor editor = new TextEditor()
        {
            text = referralTxt.text
        };
        editor.SelectAll();
        editor.Copy();
    }

    void EditProfile()
    {
        namaLengkapInput.gameObject.SetActive(true);
        namaLengkapInput.text = namaLengkapTxt.text;

        sekolahDropdown.value = 0;
        sekolahDropdown.gameObject.SetActive(true);

        foreach (Toggle toggleKelas in kelasDisplay.GetComponentsInChildren<Toggle>())
        {
            toggleKelas.interactable = true;
        }

        whatsappInput.gameObject.SetActive(true);
        whatsappInput.text = whatsappTxt.text;

        editButton.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        editButton.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        editButton.onClick.RemoveAllListeners();
        editButton.onClick.AddListener(delegate {OnEditSaved();}); 

        cancelButton.gameObject.SetActive(true);
    }

    void OnEditSekolah(int _value)
    {
        if (_value == 0) return;
        if (_value == 1)
        {
            sekolahDropdown.gameObject.SetActive(false);
            sekolahInput.text = sekolahTxt.text;
            sekolahInput.gameObject.SetActive(true); 
        } 
        else sekolahTxt.text = sekolahDropdown.options[_value].text;
    }

    void OnEditSaved()
    {
        namaLengkapInput.gameObject.SetActive(false);
        namaLengkapTxt.text = namaLengkapInput.text;
        playerData.PlayerProfileData.StudentName = namaLengkapTxt.text;

        sekolahDropdown.gameObject.SetActive(false);
        sekolahInput.gameObject.SetActive(false);
        playerData.PlayerProfileData.StudentSchool = sekolahTxt.text;

        foreach (Toggle toggleKelas in kelasDisplay.GetComponentsInChildren<Toggle>())
        {
            if (toggleKelas.isOn)
            {
                switch (toggleKelas.name) 
                {
                    case "Kelas3":
                        kelas = Kelas.Kelas3;
                        break;
                    case "Kelas4":
                        kelas = Kelas.Kelas4;
                        break;
                    case "Kelas5":
                        kelas = Kelas.Kelas5;
                        break;
                    case "Kelas6":
                        kelas = Kelas.Kelas6;
                        break;
                }
            }
            toggleKelas.interactable = false;
        }
        playerData.PlayerProfileData.StudentClass = kelas;

        whatsappInput.gameObject.SetActive(false);
        whatsappTxt.text = whatsappInput.text;
        playerData.PlayerProfileData.ParentWhatsapp = whatsappTxt.text;

        editButton.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        editButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        editButton.onClick.AddListener(delegate {EditProfile();});

        cancelButton.gameObject.SetActive(false);

        APIManager.Instance.PostUpdateProfile();
    }

    void OnEditCanceled()
    {
        namaLengkapInput.gameObject.SetActive(false);
        namaLengkapTxt.text = playerData.PlayerProfileData.StudentName;

        sekolahInput.gameObject.SetActive(false);
        sekolahDropdown.gameObject.SetActive(false);
        sekolahTxt.text = playerData.PlayerProfileData.StudentSchool;

        foreach (Toggle toggleKelas in kelasDisplay.GetComponentsInChildren<Toggle>())
        {
            toggleKelas.interactable = false;
        }

        switch (PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentClass)
        {
            case Kelas.Kelas3: ResetKelasChange(Kelas.Kelas3);
                break;
            case Kelas.Kelas4: ResetKelasChange(Kelas.Kelas4);
                break;
            case Kelas.Kelas5: ResetKelasChange(Kelas.Kelas5);
                break;
            case Kelas.Kelas6: ResetKelasChange(Kelas.Kelas6);
                break; 
        }
        
        whatsappInput.gameObject.SetActive(false);
        whatsappTxt.text = playerData.PlayerProfileData.ParentWhatsapp;

        editButton.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        editButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        editButton.onClick.AddListener(delegate {EditProfile();});

        cancelButton.gameObject.SetActive(false);
    }

    private void ResetKelasChange(Kelas kelas)
    {
        foreach (Toggle toggleKelas in kelasDisplay.GetComponentsInChildren<Toggle>())
        {
            toggleKelas.interactable = false;
            if (toggleKelas.name == kelas.ToString())
            {
                toggleKelas.isOn = true;
            }
        }
    }

}

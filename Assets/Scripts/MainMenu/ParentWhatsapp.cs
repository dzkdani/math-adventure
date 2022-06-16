using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParentWhatsapp : MonoBehaviour
{
    [Header("Parent WA Components")]
    [Space(5)]
    public GameObject panel;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI whatsapp;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button confirmBtn;
    [SerializeField] Button changeBtn;

#region Init
    private void OnEnable() {
        Init();
    }

    private void OnDisable() {
        Init();
    }

    private void Init()
    {
        title.text = "Konfirmasi nomor whatsapp Orang Tua/Wali kamu!";
        whatsapp.text = PlayerDataManager.Instance.PlayerData.PlayerProfileData.ParentWhatsapp;
        
        confirmBtn.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Konfirmasi";
        changeBtn.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Ubah";
        inputField.gameObject.SetActive(false);
        whatsapp.gameObject.SetActive(true);

        confirmBtn.onClick.RemoveAllListeners();
        confirmBtn.onClick.AddListener(delegate {OnConfirm();});
        changeBtn.onClick.RemoveAllListeners();
        changeBtn.onClick.AddListener(delegate {OnChangeWhatsapp();});

        confirmBtn.interactable = (whatsapp.text != "");
    }
#endregion

    private void OnChangeWhatsapp()
    {
        confirmBtn.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Simpan";
        changeBtn.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Batal";
        inputField.gameObject.SetActive(true);
        whatsapp.gameObject.SetActive(false);
        inputField.text = whatsapp.text;

        confirmBtn.onClick.RemoveAllListeners();
        confirmBtn.onClick.AddListener(delegate {OnChangeWhatsappSaved();});
        changeBtn.onClick.RemoveAllListeners();
        changeBtn.onClick.AddListener(delegate {OnChangeWhatsappCanceled();});
        inputField.onEndEdit.RemoveAllListeners();
        inputField.onEndEdit.AddListener(delegate {confirmBtn.interactable = (inputField.text != "");});
    }

    private void OnChangeWhatsappSaved()
    {
        whatsapp.text = inputField.text;
        PlayerDataManager.Instance.PlayerData.PlayerProfileData.ParentWhatsapp = whatsapp.text;

        APIManager.Instance.PostUpdateProfile();

        confirmBtn.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Konfirmasi";
        changeBtn.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Ubah";
        inputField.gameObject.SetActive(false);
        whatsapp.gameObject.SetActive(true);

        confirmBtn.onClick.RemoveAllListeners();
        confirmBtn.onClick.AddListener(delegate {OnConfirm();});
        changeBtn.onClick.RemoveAllListeners();
        changeBtn.onClick.AddListener(delegate {OnChangeWhatsapp();});
    }

    private void OnChangeWhatsappCanceled()
    {
        confirmBtn.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Konfirmasi";
        changeBtn.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Ubah";
        inputField.gameObject.SetActive(false);
        whatsapp.gameObject.SetActive(true);

        confirmBtn.onClick.RemoveAllListeners();
        confirmBtn.onClick.AddListener(delegate {OnConfirm();});
        changeBtn.onClick.RemoveAllListeners();
        changeBtn.onClick.AddListener(delegate {OnChangeWhatsapp();});
    }

    private void OnConfirm()
    {
        SubscriptionController.Instance.InitDetailsSub();
        panel.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening; 

public class SubmitVoucher : MonoBehaviour
{
    [Header("Submit Voucher Components")]
    [Space(5)]
    public GameObject panel;
    [SerializeField] TextMeshProUGUI log;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button submitBtn;
    [SerializeField] Button berlanggananBtn;
    [SerializeField] Button closeBtn;
    
#region Init
    private void OnEnable() {
        Init();
    }
    private void OnDisable() {
        Init();
    }

    private void Init()
    {
        inputField.text = null;
        log.text = null;
        submitBtn.interactable = true;
        submitBtn.onClick.RemoveAllListeners();
        submitBtn.onClick.AddListener(delegate {OnSubmitVoucher();});
        berlanggananBtn.onClick.RemoveAllListeners();
        berlanggananBtn.onClick.AddListener(delegate {OnBerlangganan();});
        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(delegate {OnClose();});
    }
#endregion
 
    private void OnClose()
    {
        if (!SubscriptionController.Instance.MainMenuPanel.activeInHierarchy)
            SubscriptionController.Instance.MainMenuPanel.SetActive(true);
        else
            return;

        panel.SetActive(false);
    }

    private void OnSubmitVoucher()
    {
        if (inputField.text.Length > 0)
        {
            log.text = "Submitting Voucher";
            submitBtn.interactable = false;
            StartCoroutine(PostVoucher());
        }
        else
        {
            log.text = "Kode Voucher tidak boleh kosong.";
        }
    }

    private IEnumerator PostVoucher()
    {
        yield return StartCoroutine(APIManager.Instance.PostVoucher(inputField.text));
        log.text = APIManager.Instance.voucherLog;
        if (log.text != "")
        {
            if (log.text == "Voucher tidak ditemukan/sudah digunakan.")
            {
                submitBtn.interactable = true;
                log.text = "Masukkan kode voucher yang benar!";
            }
            else
            {
                OnClose();
                MainMenuController.Instance.OnMenuOpened(MainMenuController.Instance.ProfilMenu);    
            }
        }
    }

    private void OnBerlangganan()
    {
        MainMenuController.Instance.OnMenuOpened(SubscriptionController.Instance.BerlanggananPanel);
        SubscriptionController.Instance.InitSubscription();
        panel.SetActive(false);
    }
}

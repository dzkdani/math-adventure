using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubscriptionController : MonoBehaviour
{
    [Header("Subs Components")]
    [Space(5)]  
    public GameObject BerlanggananPanel;
    public GameObject MainMenuPanel;
    public SelectSubscription selectSubscription;
    public ParentWhatsapp parentWhatsapp;
    public DetailSubscription detailSubscription;
    public SubmitVoucher submitVoucher;
    [SerializeField] Button backBtn;

    public static SubscriptionController Instance;

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        backBtn.onClick.AddListener(delegate {OnBack();});
    }

    private void OnEnable() {
        InitSubscription();
    }

    public void OnBack()
    {
        selectSubscription.panel.SetActive(true);
        parentWhatsapp.panel.SetActive(false);
        detailSubscription.panel.SetActive(false);
        submitVoucher.panel.SetActive(false);
        BerlanggananPanel.SetActive(false);
        
        MainMenuController.Instance.OnBackToMainMenu();
    }

    public void InitSubscription()
    {
        if (!selectSubscription.panel.activeInHierarchy)
            selectSubscription.panel.SetActive(true);
        else
            return;
    }

    public void InitParentWhatsapp()
    {
        if (!parentWhatsapp.panel.activeInHierarchy)
            parentWhatsapp.panel.SetActive(true);
        else
            return;
    }

    public void InitDetailsSub()
    {
        if (!detailSubscription.panel.activeInHierarchy)
            detailSubscription.panel.SetActive(true);
        else
            return;
    }

    public void InitSubmitVoucher()
    {
        if (!submitVoucher.panel.activeInHierarchy)
            submitVoucher.panel.SetActive(true);
        else
            return;
    }
}

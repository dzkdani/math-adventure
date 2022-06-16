using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public enum Subscription
{
    SatuBulan,
    TigaBulan,
    EnamBulan 
}

public class SelectSubscription : MonoBehaviour
{
    [Header("Select Subscription Components")]
    [Space(5)]
    public GameObject panel;
    [SerializeField] ToggleGroup subscriptionToggles;
    [SerializeField] Button lanjutBtn;

    [Header("Subscribe Data")]
    [Space(5)]
    public Subscription subscriptionType;

#region Init
    private void OnEnable() {
        Init();
    }

    private void OnDisable() {
        Init();
    }

    void Init()
    {
        foreach (Toggle subsToggle in subscriptionToggles.GetComponentsInChildren<Toggle>())
        {
            subsToggle.isOn = false;   
        }

        lanjutBtn.interactable = false;
        lanjutBtn.gameObject.SetActive(false);
        lanjutBtn.onClick.RemoveAllListeners();
        lanjutBtn.onClick.AddListener(delegate {OnSubsSelected();});
    }
#endregion

    public void SatuBulanSubs(bool _selected)
    {
        if (_selected)
        {
            lanjutBtn.gameObject.SetActive(true);
            lanjutBtn.interactable = true;
            subscriptionType = Subscription.SatuBulan;
        }
        else
        {
            lanjutBtn.gameObject.SetActive(false);
            lanjutBtn.interactable = false;
        }
    }

    public void TigaBulanSubs(bool _selected)
    {
        if (_selected)
        {
            lanjutBtn.gameObject.SetActive(true);
            lanjutBtn.interactable = true;
            subscriptionType = Subscription.TigaBulan;
        }
        else
        {
            lanjutBtn.gameObject.SetActive(false);
            lanjutBtn.interactable = false;
        }
    }

    public void EnamBulanSubs(bool _selected)
    {
        if (_selected)
        {
            lanjutBtn.gameObject.SetActive(true);
            lanjutBtn.interactable = true;
            subscriptionType = Subscription.EnamBulan;
        }
        else
        {
            lanjutBtn.gameObject.SetActive(false); 
            lanjutBtn.interactable = false;
        }
    }

    private void OnSubsSelected()
    {
        string subsType = subscriptionType.ToString();
        SubscriptionController.Instance.InitParentWhatsapp();
        panel.SetActive(false);
    }
}

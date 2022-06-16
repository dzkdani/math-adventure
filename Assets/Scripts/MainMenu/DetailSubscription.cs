using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class DetailSubscription : MonoBehaviour
{
    [Header("Subscribe Componets")]
    [Space(5)]
    [Header("Content Text")]
    [Space(5)]
    public GameObject panel;
    [SerializeField] TextMeshProUGUI nama;
    [SerializeField] TextMeshProUGUI whatsappOrtu;
    [SerializeField] TextMeshProUGUI paket;
    [SerializeField] TextMeshProUGUI biaya;
    [SerializeField] TextMeshProUGUI rewardPoint;
    [SerializeField] TMP_InputField referral;
    [SerializeField] TMP_InputField kodePromo;
    [SerializeField] TextMeshProUGUI diskon;
    [SerializeField] TextMeshProUGUI total;
    [SerializeField] TextMeshProUGUI orderID;
    [SerializeField] Button lanjutBtn;
    [SerializeField] Button confirmBtn;
    [SerializeField] Button changeBtn;
   
    [Header("Title Text")]
    [Space(5)]
    [SerializeField] TextMeshProUGUI rewardPointTitle;
    [SerializeField] TextMeshProUGUI referralTitle;
    [SerializeField] TextMeshProUGUI diskonTitle;
    [SerializeField] TextMeshProUGUI totalTitle;
    [SerializeField] TextMeshProUGUI orderIDTitle;

    [Header("Admin WA Number")]
    [Space(5)]
    [SerializeField] string adminWhasapp;

    [Header("Subscription Image")]
    [Space(5)]
    [SerializeField] Image SubsImg;
    [SerializeField] Sprite[] SubsImgSprites;

    string biayaPaket = "0";
    string invoiceMessege = "";
    string promo = "";

#region Init
    private void OnEnable() {
        Init(SubscriptionController.Instance.selectSubscription.subscriptionType.ToString());
    }

    private void OnDisable() {
        Init(" ");
    }

    public void Init(string _subsType)
    {
        nama.text = PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentName;
        whatsappOrtu.text = PlayerDataManager.Instance.PlayerData.PlayerProfileData.ParentWhatsapp;
        paket.text = _subsType;
        biaya.text = BiayaPaket(_subsType);
        rewardPoint.text = "Reward Point";
        referral.interactable = true;
        referral.text = null;
        kodePromo.interactable = true;
        kodePromo.text = null;
        promo = null;
        diskon.text = null;
        total.text = null;
        orderID.text = null;
        diskon.gameObject.SetActive(false);
        total.gameObject.SetActive(false);
        orderID.gameObject.SetActive(false);

        diskonTitle.gameObject.SetActive(false);
        totalTitle.gameObject.SetActive(false);
        orderIDTitle.gameObject.SetActive(false);

        lanjutBtn.onClick.RemoveAllListeners();
        lanjutBtn.onClick.AddListener(delegate{OnLanjut();});
        lanjutBtn.gameObject.SetActive(true);
        confirmBtn.onClick.RemoveAllListeners();
        confirmBtn.onClick.AddListener(delegate{OnConfirm();});
        confirmBtn.gameObject.SetActive(false);
        changeBtn.onClick.RemoveAllListeners();
        changeBtn.onClick.AddListener(delegate{OnChange();});
        changeBtn.gameObject.SetActive(false);

        GetSubsData();
    }

    private void GetSubsData()
    {
        //get data from database of price and wa admin
    }
#endregion

    private string BiayaPaket(string _subsType)
    {
        switch (_subsType)
        {
            case "SatuBulan": biayaPaket = "100.000";
                              SubsImg.sprite = SubsImgSprites[0];
                break;
            case "TigaBulan": biayaPaket = "150.000";
                              SubsImg.sprite = SubsImgSprites[1];
                break;
            case "EnamBulan": biayaPaket = "200.000";
                              SubsImg.sprite = SubsImgSprites[2];
                break;
        }
        SubsImg.GetComponent<Image>().SetNativeSize();
        return biaya.text = $"IDR {biayaPaket},-";
    }

    private void OnLanjut()
    {
        referral.interactable = false;
        kodePromo.interactable = false;
        promo = kodePromo.text;
        diskon.text = " diskon"; //hitung diskon?
        total.text = " total"; //nanti hitung sendiri isi pake exact valuenya aja
        orderID.text = " order id"; //generate order id?
        diskon.gameObject.SetActive(true);
        total.gameObject.SetActive(true);
        orderID.gameObject.SetActive(true);

        diskonTitle.gameObject.SetActive(true);
        totalTitle.gameObject.SetActive(true);
        orderIDTitle.gameObject.SetActive(true);

        lanjutBtn.gameObject.SetActive(false);
        confirmBtn.gameObject.SetActive(true);
        changeBtn.gameObject.SetActive(true);
    }

    private void OnChange()
    {
        panel.SetActive(false);
        SubscriptionController.Instance.InitSubscription();
    }

    private void OnConfirm()
    {
        invoiceMessege = $"Nama : {nama.text} \nNo Whatsapp Ortu/Wali : {whatsappOrtu.text} \n\nPaket : {paket.text} \nBiaya : {biayaPaket} \n Reward Point : {rewardPoint.text} \nKode Promo : {kodePromo.text} \n\nDiskon : {diskon.text} \nTotal Dibayarkan : {total.text} \nOrder ID : {orderID.text}";
        
        Debug.Log(invoiceMessege);

        invoiceMessege = invoiceMessege.Replace("\n", "%0A"); //%0A untuk newline di wa
        StartCoroutine(OpenWhatsapp(adminWhasapp, invoiceMessege));
    }

    private IEnumerator OpenWhatsapp(string _whatsappNumber, string _invoiceMessege)
    {
        yield return new WaitForSecondsRealtime(2f);
        Application.OpenURL($"https://wa.me/{_whatsappNumber}?text={_invoiceMessege}");
        SubscriptionController.Instance.BerlanggananPanel.SetActive(false);
        SubscriptionController.Instance.MainMenuPanel.SetActive(true);
    }
}

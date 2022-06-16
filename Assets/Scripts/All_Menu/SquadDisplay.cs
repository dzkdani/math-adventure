using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SquadDisplay : MonoBehaviour
{
    public GameObject prefab_card;
    public RectTransform ortu, ini, info_panel;

    public TextMeshProUGUI nama, parts, type, atk, def, hp, coin_req, lv, Rank, cards_num, btn_status;
    public Image pic, place_icon;
    public Sprite[] icon;
    public Button _Upgrade_Troops_or_Heroes;

    List<BeginAndEnd> beginAndEnds = new List<BeginAndEnd>();

    public void btnClose()
    {
        ExchangeTRUE(ini.gameObject, info_panel.gameObject);
    }

    public void ExchangeTRUE(GameObject a, GameObject b)
    {
        bool temp = a.activeSelf;
        a.SetActive(b.activeSelf);
        b.SetActive(temp);
    }

    public void UI_information_troops_or_heroes(BeginAndEnd UE)
    {
        Rank.text = "Rank " + UE.unitData.LevelCap;
        nama.text = UE.unitData.Name;
        lv.text = "LV " + UE.unitData.Level;
        cards_num.text = UE.unitData.DuplicateCard + " cards";
        
        int cost = 12 + (UE.unitData.Level * 2);
        int cost2 = 3 + (UE.unitData.LevelCap);
        int increment = UE.unitData.Level - 1;
        int temp_index;

        if (UE.warning.Equals("ksatriaION"))
        {
            string[] sd = UE.unitData.Name.Split('_');
            nama.text = sd[0];

            if (UE.unitData.Level < PlayerDataManager.Instance.LevelMax) coin_req.text = cost + " diamonds";
            else coin_req.text = "---";
            atk.text = "ATK " + (UE.herod.BaseATK+(UE.herod.IncreaseATK*increment));
            def.text = "DEF " + (UE.herod.BaseDEF + (UE.herod.IncreaseDEF * increment));
            hp.text = "HP " + (UE.herod.BaseHP + (UE.herod.IncreaseHP * increment));
            type.text = UE.herod.Type.ToString();
            pic.sprite = ubah_icon_troops(UE.unitData.LevelCap, UE.herod.RankIcons);
            parts.text = "KSATRIA ION";
            temp_index = 2;
            place_icon.color = Color.cyan;
        }
        else
        {
            if (UE.unitData.Level < PlayerDataManager.Instance.LevelMax) coin_req.text = cost + " coins";
            else coin_req.text = "---";
            atk.text = "ATK " + (UE.troopd.BaseATK + (UE.troopd.IncreaseATK * increment));
            def.text = "DEF " + (UE.troopd.BaseDEF + (UE.troopd.IncreaseDEF * increment));
            hp.text = "HP " + (UE.troopd.BaseHP + (UE.troopd.IncreaseHP * increment));
            type.text = UE.troopd.AttackType.ToString();
            pic.sprite = ubah_icon_troops(UE.unitData.LevelCap, UE.troopd.RankIcons);
            parts.text = "TROOPS";
            temp_index=0;
            place_icon.color = Color.white;
        }

        if(UE.unitData.Level >= PlayerDataManager.Instance.LevelMax)
        {
            coin_req.text = "---";
            _Upgrade_Troops_or_Heroes.interactable = false;
            _Upgrade_Troops_or_Heroes.image.color = Color.gray;
            btn_status.gameObject.SetActive(false);
        }
        else
        {
            _Upgrade_Troops_or_Heroes.interactable = true;
            _Upgrade_Troops_or_Heroes.image.color = Color.white;
            btn_status.gameObject.SetActive(true);

            if (!UE.unitData.TimeToRankUp)
            {

                btn_status.text = "Level UP";
                place_icon.sprite = icon[temp_index];

                if (UE.warning.Equals("ksatriaION"))
                {
                    if (CurrencyManager.Instance.GetPlayerTotalDiamonds() < cost)
                    {
                        _Upgrade_Troops_or_Heroes.interactable = false;
                        _Upgrade_Troops_or_Heroes.image.color = Color.gray;
                        btn_status.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (CurrencyManager.Instance.GetPlayerTotalCoins() < cost)
                    {
                        _Upgrade_Troops_or_Heroes.interactable = false;
                        _Upgrade_Troops_or_Heroes.image.color = Color.gray;
                        btn_status.gameObject.SetActive(false);
                    }
                }

            }
            else
            {
                coin_req.text = cost2 + " cards";
                place_icon.sprite = icon[3];
                place_icon.color = Color.white;
                btn_status.text = "Rank UP";

                if (UE.warning.Equals("ksatriaION"))
                {
                    if (UE.unitData.DuplicateCard < cost2)
                    {
                        _Upgrade_Troops_or_Heroes.interactable = false;
                        _Upgrade_Troops_or_Heroes.image.color = Color.gray;
                        btn_status.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (UE.unitData.DuplicateCard < cost2)
                    {
                        _Upgrade_Troops_or_Heroes.interactable = false;
                        _Upgrade_Troops_or_Heroes.image.color = Color.gray;
                        btn_status.gameObject.SetActive(false);
                    }
                }
            }
        }

    }

    public void klik_card(BeginAndEnd UE)
    {
        UI_information_troops_or_heroes(UE);
        
        _Upgrade_Troops_or_Heroes.onClick.RemoveAllListeners();
        _Upgrade_Troops_or_Heroes.onClick.AddListener(delegate { Level_UP_Troops_or_Heroes(UE); });

        ExchangeTRUE(ini.gameObject, info_panel.gameObject);
    }

    public void Rank_UP_Troops_or_Heroes(BeginAndEnd UE)
    {
        if (UE.unitData.LevelCap < PlayerDataManager.Instance.LevelCapMax)
        {
            int cost2 = 3 + (UE.unitData.LevelCap);
            if (UE.warning.Equals("ksatriaION"))
            {
                if (UE.unitData.DuplicateCard >= cost2)
                {
                    UE.unitData.DuplicateCard -= cost2; UE.unitData.LevelCap++;
                    UE.unitData.TimeToRankUp = false;
                }
            }
            else
            {
                if (UE.unitData.DuplicateCard >= cost2)
                {
                    UE.unitData.DuplicateCard -= cost2; UE.unitData.LevelCap++;
                    UE.unitData.TimeToRankUp = false;
                }
            }

        }
        UI_information_troops_or_heroes(UE);
    }

    public void Level_UP_Troops_or_Heroes(BeginAndEnd UE)
    { 
        if(UE.unitData.Level < PlayerDataManager.Instance.LevelMax)
        {
            if (UE.unitData.TimeToRankUp)
            {
                Rank_UP_Troops_or_Heroes(UE);
            }
            else
            {
                int cost = 12 + (UE.unitData.Level * 2);
                if (UE.warning.Equals("ksatriaION"))
                {
                    if (CurrencyManager.Instance.GetPlayerTotalDiamonds() >= cost)
                    {
                        CurrencyManager.Instance.DecrementDiamond(cost);
                        UE.unitData.Level++;
                        if (UE.unitData.Level % PlayerDataManager.Instance.RangeLevelPerCap == 0) UE.unitData.TimeToRankUp = true;

                    }
                }
                else
                {
                    if (CurrencyManager.Instance.GetPlayerTotalCoins() >= cost)
                    {
                        CurrencyManager.Instance.DecrementCoin(cost);
                        UE.unitData.Level++;
                        if (UE.unitData.Level % PlayerDataManager.Instance.RangeLevelPerCap == 0) UE.unitData.TimeToRankUp = true;

                    }
                }

                UI_information_troops_or_heroes(UE);
            }
         }
    }

    // Start is called before the first frame update
    void Start()
    {

        if(TroopManager.Instance.GetTroopDataListCount() >= 1)
        {
            for(int i = 0; i < PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList.Count; i++)
            {
                PlayerData.UnitsOwnedData troopData = PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList[i];

                GameObject temp = Instantiate(prefab_card, ortu);
                TroopData alpha = TroopManager.Instance.GetTroopData(troopData.Name);
                BeginAndEnd bae = temp.GetComponent<BeginAndEnd>();
                /* if (troopData.IsUnlocked)
                 {
                     temp.GetComponent<BeginAndEnd>().unitData = troopData;
                     temp.GetComponent<BeginAndEnd>().textMesh.text = "Rank " + troopData.LevelCap;
                     temp.GetComponent<BeginAndEnd>().troopd = alpha;
                     temp.GetComponent<Button>().onClick.AddListener(delegate { klik_card(temp.GetComponent<BeginAndEnd>()); });
                     temp.GetComponent<BeginAndEnd>().image.sprite = ubah_icon_troops(troopData.LevelCap, alpha.RankIcons);
                 }
                 else
                 {
                     temp.GetComponent<BeginAndEnd>().image.color = Color.gray;
                     temp.GetComponent<BeginAndEnd>().textMesh.text = " ";
                     temp.GetComponent<BeginAndEnd>().unitData = null;
                     temp.GetComponent<Button>().interactable = false;
                     temp.GetComponent<BeginAndEnd>().image.sprite = ubah_icon_troops(1, alpha.RankIcons);
                 }*/

                bae.unitData = troopData;
                bae.textMesh.text = "Rank " + troopData.LevelCap;
                bae.troopd = alpha;
                temp.GetComponent<Button>().onClick.AddListener(delegate { klik_card(temp.GetComponent<BeginAndEnd>()); });
                bae.image.sprite = ubah_icon_troops(troopData.LevelCap, alpha.RankIcons);

                if (!troopData.IsUnlocked)
                {
                    bae.image.color = Color.gray;
                    bae.textMesh.text = " ";
                    temp.GetComponent<Button>().interactable = false;
                    bae.image.sprite = ubah_icon_troops(1, alpha.RankIcons);
                }

                beginAndEnds.Add(bae);

                temp.transform.localScale = Vector3.one;
            }

        }

        if (HeroManager.Instance.GetHeroDataListCount() >= 1)
        {
            for (int i = 0; i < PlayerDataManager.Instance.PlayerData.HeroesOwnedDataList.Count; i++)
            {
                PlayerData.UnitsOwnedData yuushaData = PlayerDataManager.Instance.PlayerData.HeroesOwnedDataList[i];
                string[] sd = yuushaData.Name.Split('_');

                GameObject temp = Instantiate(prefab_card, ortu);
                HeroData alpha = HeroManager.Instance.GetHeroData(sd[0]);
                BeginAndEnd bae = temp.GetComponent<BeginAndEnd>();
                /* if (yuushaData.IsUnlocked)
                 {
                     temp.GetComponent<BeginAndEnd>().warning = "ksatriaION";
                     temp.GetComponent<BeginAndEnd>().unitData = yuushaData;
                     temp.GetComponent<BeginAndEnd>().textMesh.text = "Rank " + yuushaData.LevelCap;
                     temp.GetComponent<BeginAndEnd>().herod = alpha;
                     temp.GetComponent<Button>().onClick.AddListener(delegate { klik_card(temp.GetComponent<BeginAndEnd>()); });
                     temp.GetComponent<BeginAndEnd>().image.sprite = ubah_icon_troops(yuushaData.LevelCap, alpha.RankIcons);
                 }
                 else
                 {
                     temp.GetComponent<BeginAndEnd>().image.color = Color.gray;
                     temp.GetComponent<BeginAndEnd>().unitData = null;
                     temp.GetComponent<BeginAndEnd>().textMesh.text = " ";
                     temp.GetComponent<Button>().interactable = false;
                     temp.GetComponent<BeginAndEnd>().image.sprite = ubah_icon_troops(1, alpha.RankIcons);
                 }*/

                    bae.warning = "ksatriaION";
                    bae.unitData = yuushaData;
                    bae.textMesh.text = "Rank " + yuushaData.LevelCap;
                    bae.herod = alpha;
                    temp.GetComponent<Button>().onClick.AddListener(delegate { klik_card(temp.GetComponent<BeginAndEnd>()); });
                    bae.image.sprite = ubah_icon_troops(yuushaData.LevelCap, alpha.RankIcons);
                
                if (!yuushaData.IsUnlocked)
                {
                    bae.image.color = Color.gray;
                    bae.textMesh.text = " ";
                    temp.GetComponent<Button>().interactable = false;
                    bae.image.sprite = ubah_icon_troops(1, alpha.RankIcons);
                }

                beginAndEnds.Add(bae);

                temp.transform.localScale = Vector3.one;

            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            UI_klik_card();
            
        }
        catch
        {

        }
    }

    public void UI_klik_card()
    {
        beginAndEnds.ForEach(ea => {
            if (ea.unitData.IsUnlocked)
            {
                ea.GetComponent<Button>().interactable = true;
                ea.textMesh.text = "Rank " + ea.unitData.LevelCap;
                ea.image.color = Color.white;
                if (ea.troopd != null) ea.image.sprite = ubah_icon_troops(ea.unitData.LevelCap, ea.troopd.RankIcons);
                else ea.image.sprite = ubah_icon_troops(ea.unitData.LevelCap, ea.herod.RankIcons);
            }
            else if (!ea.unitData.IsUnlocked)
            {
                ea.GetComponent<Button>().interactable = false;
                ea.image.color = Color.gray;
                ea.textMesh.text = " ";
            }
        });
    }

    public void lock_unlock_all_troops(bool a)
    {
        PlayerDataManager.Instance.PlayerData.HeroesOwnedDataList.ForEach(vs => {
            vs.IsUnlocked = a;
        });

        PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList.ForEach(vs => {
            vs.IsUnlocked = a;
        });
        UI_klik_card();
    }

    private Sprite ubah_icon_troops(int levelcap, Sprite[] Rank_Icons)
    {
        switch (levelcap)
        {
            case 1 : return Rank_Icons[0];
            case 2 : return Rank_Icons[1];
            case 3: return Rank_Icons[2];
            case 4: return Rank_Icons[3];
            default: return null;
        }
    }
}

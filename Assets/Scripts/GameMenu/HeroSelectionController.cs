using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using DragonBones;

public class HeroSelectionController : MonoBehaviour
{
    [Header("Menu Panels")]
    [Space(5)]
    [SerializeField] GameObject MainMenuPanel;
    [SerializeField] GameObject HeroSelectPanel;
    [SerializeField] bool IsFirstTime;
    [SerializeField] bool IsChangeHeroes;

    [Header("Buttons")]
    [Space(5)]
    [SerializeField] Button selectBtn;
    [SerializeField] List<Button> HeroBtns;
    [Header("Hero Details")]
    [Space(5)]
    [SerializeField] Image HeroImg;
    [SerializeField] TextMeshProUGUI HeroFirstNameTxt;
    [SerializeField] TextMeshProUGUI HeroSecondNameTxt;
    [SerializeField] TextMeshProUGUI HeroDescTxt;
    [Header("Hero Skills")]
    [Space(5)]
    [SerializeField] TextMeshProUGUI HeroBasicSkillTxt;
    [SerializeField] Image HeroBasicSkillImg;
    [SerializeField] TextMeshProUGUI HeroSuperSkillTxt;
    [SerializeField] Image HeroSuperSkillImg;
    [Header("Heroes Animations")]
    [Space(5)]
    [SerializeField] List<UnityArmatureComponent> heroesAnims;
    [SerializeField] string animName;

    [Space(10)]
    [SerializeField] string CurrSelectHero;
 
    private void Awake() {
        //make a singleton later
    }

    private void Start() {
        IsChangeHeroes = false;
        IsFirstTime = false;

        OnFirstSelectHero();
    }

    public void OnChangeHero()
    {
        MainMenuPanel?.SetActive(false);
        HeroSelectPanel?.SetActive(true);
        InitHeroSelect();
    }

    private void OnFirstSelectHero()
    {
        if (PlayerDataManager.Instance.IsSelectFirstHero())
        {
            MainMenuPanel?.SetActive(false);
            HeroSelectPanel?.SetActive(true);

            IsFirstTime = true;
            IsChangeHeroes = false;
        
            InitHeroSelect();
        }
        else
        {
            MainMenuPanel?.SetActive(true);
            HeroSelectPanel?.SetActive(false);
            
            IsFirstTime = false;
            IsChangeHeroes = true;

            //play owned hero anim
            InitSelectedHeroAnim(LastSelectedHero());
        }
    }
    
    public void InitHeroSelect()
    {
        foreach (Button heroBtn in HeroBtns)
        {
            heroBtn.onClick.AddListener(delegate 
            {
                OnHeroClicked(heroBtn.gameObject);
            });
        }

        selectBtn.onClick.AddListener(delegate { OnHeroSelected(); } ); 
        selectBtn.interactable = false;

        HeroFirstNameTxt.text = null;
        HeroSecondNameTxt.text = null;
        HeroDescTxt.text = null;
        HeroBasicSkillTxt.text = null;
        HeroSuperSkillTxt.text = null;

        HeroImg.GetComponent<Image>().sprite = null;
        HeroImg.GetComponent<Image>().color = Color.clear;
    }

    private string LastSelectedHero()
    {
        string _lastSelectedHero = "";

        PlayerDataManager.Instance.PlayerData.HeroesOwnedDataList.ForEach( hero => 
        {
            if (hero.IsUnlocked && hero.IsSelected)
            {
                _lastSelectedHero = hero.Name;
            }
        });

        return _lastSelectedHero;
    }

    private void OnHeroClicked(GameObject _HeroUIHandler) 
    {
        //change border color
        foreach (Button heroBtn in HeroBtns)
        {
            heroBtn.GetComponent<Image>().color = Color.white;
            heroBtn.interactable = true;
        }
        _HeroUIHandler.GetComponent<Button>().interactable = false;
        

        HeroImg.rectTransform.DOScale(Vector3.zero, 0.1f);

        //displaydata
        HeroSelectUIHandler selectedHero = _HeroUIHandler.GetComponent<HeroSelectUIHandler>();
        HeroImg.sprite = selectedHero.GetHeroSprite();
        HeroFirstNameTxt.text = selectedHero.GetHeroName();
        HeroSecondNameTxt.text = selectedHero.GetHeroNick();
        HeroDescTxt.text = selectedHero.GetHeroDesc();

        HeroImg.GetComponent<Image>().color = Color.white;
        HeroImg.GetComponent<Image>().SetNativeSize();
        HeroImg.rectTransform.DOScale(0.7f, 0.7f);

        CurrSelectHero = _HeroUIHandler.GetComponent<Button>().name;

        if (IsFirstTime) //if first time select
        {
            selectBtn.interactable = true;
        }
        else if (IsChangeHeroes) //if change hero
        {
            foreach (PlayerData.UnitsOwnedData hero in PlayerDataManager.Instance.PlayerData.HeroesOwnedDataList)
            {
                if (hero.Name == CurrSelectHero && !hero.IsUnlocked)
                {
                    selectBtn.interactable = false;
                }
                else if (hero.Name == CurrSelectHero && hero.IsUnlocked)
                {
                    selectBtn.interactable = true;
                }
            }
        }
    }

    private void OnHeroSelected()
    {
        foreach (PlayerData.UnitsOwnedData hero in PlayerDataManager.Instance.PlayerData.HeroesOwnedDataList)
        {
            hero.IsSelected = false;
            
            if (hero.Name == CurrSelectHero)
            {
                hero.IsUnlocked = true;
                hero.IsSelected = true;        
            }
        }

        IsFirstTime = PlayerDataManager.Instance.IsSelectFirstHero() ? true : false;
        IsChangeHeroes = !IsFirstTime;

        APIManager.Instance.PostLastHeroSelected(CurrSelectHero);
        if (!IsFirstTime) APIManager.Instance.PostNewPlayerCard(APIManager.Instance.AllCards.SingleOrDefault(c => c.name == CurrSelectHero).id.ToString());

        InitSelectedHeroAnim(CurrSelectHero); //display anim selected hero in the middle

        MainMenuPanel?.SetActive(true);
        HeroSelectPanel?.SetActive(false);
    }

    private void InitSelectedHeroAnim(string _selectedHero)
    {
        if (_selectedHero.Contains("Red"))
        {
            if (_selectedHero.Contains("_F"))
            {
                animName = "girlRed";
            }
            else if (_selectedHero.Contains("_M"))
            {
                animName = "boyRed";
            }
            PlayHeroAnim("Red");
        }
        else if (_selectedHero.Contains("Blue"))
        {
            if (_selectedHero.Contains("_F"))
            {
                animName = "girlBlue";
            }
            else if (_selectedHero.Contains("_M"))
            {
                animName = "boyBlue";
            }
            PlayHeroAnim("Blue");
        }
        else if (_selectedHero.Contains("Green"))
        {
            if (_selectedHero.Contains("_F"))
            {
                animName = "girlGreen";
            }
            else if (_selectedHero.Contains("_M"))
            {
                animName = "boyGreen";
            }
            PlayHeroAnim("Green");
        }
        else if (_selectedHero.Contains("Yellow"))
        {
            if (_selectedHero.Contains("_F"))
            {
                animName = "girlYellow";
            }
            else if (_selectedHero.Contains("_M"))
            {
                animName = "boyYellow";
            }
            PlayHeroAnim("Yellow");
        }
    }
    
    private void PlayHeroAnim(string _heroType)
    {
        heroesAnims.ForEach( anim => 
            { 
                if(anim.name.Contains(_heroType)) { anim.gameObject.SetActive(true); } else { anim.gameObject.SetActive(false); } 
                anim.animation.Stop();
                anim.animation.Play(animName + "_idle2");  
            });
    }

    public void GetHeroAnimName()
    {
        if(CurrSelectHero == "")
        {
            foreach (PlayerData.UnitsOwnedData hero in PlayerDataManager.Instance.PlayerData.HeroesOwnedDataList)
            {
                if (hero.IsSelected)
                {
                    CurrSelectHero = hero.Name;
                    break;
                }
            }
        }
        TrainingArenaSettingManager.Instance.CurrHeroSelect = CurrSelectHero;
    }
}

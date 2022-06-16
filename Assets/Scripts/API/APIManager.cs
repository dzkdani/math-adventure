using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum ResponseType
{
    landing,
    account,
    profile,
    voucher,
    categoryList,
    categoryRegister,
    nilai,
    leaderboard,
    soal,
    card,
    playerCard,
    intro,
    currency,
    update,
    last_hero,
    savedata,
    quizdata
}

public class APIManager : MonoBehaviour
{
    [Header("API URL")]
    [Space(5)]
    public API api;
 
    [Header("API Log")]
    [Space(5)]
    public TextMeshProUGUI landingLog;
    public string voucherLog;

    [Header("Response Datas")]
    public KeyValueData last_hero;
    public List<CardData> AllCards = new List<CardData>();
    public List<PlayerCardData> PlayerCards = new List<PlayerCardData>();
    public List<NilaiData> PlayerNilai = new List<NilaiData>();
    public List<CurrencyData> PlayerCurrencies = new List<CurrencyData>();
    public List<SoalData> Soals = new List<SoalData>();
    public List<IntroData> Intros = new List<IntroData>();
    public List<KeyValueData> TopikQuizSaveData = new List<KeyValueData>();

    [Header("API Category List")]
    [Space(5)]
    public CategoryList categoryList;
 
#region Singleton
    public static APIManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
#endregion

#region APIMethods
#region Post
    public void PostLogin(string _user, string _pass) 
    {
        WWWForm loginForm = new WWWForm();
        loginForm.AddField("username", _user); 
        loginForm.AddField("password", _pass);

        StartCoroutine(loginForm.Post<Response<string>>(api.URL("login"), ResponseHandler, ResponseType.landing));
    }
     
    public IEnumerator PostRegister(string _nama, int _kelas, string _sekolah, string _sekolahOther, string _user, string _pass) 
    {
        WWWForm registerForm = new WWWForm();

        //nonused form-data//
        registerForm.AddField("email", "");
        registerForm.AddField("parent_email", "");
        registerForm.AddField("parent_phone", "");

        registerForm.AddField("phone", _user);
        registerForm.AddField("name", _nama);
        registerForm.AddField("school_id", _sekolah);
        registerForm.AddField("school_name", _sekolahOther);
        registerForm.AddField("class_id", _kelas); 
        registerForm.AddField("username", _user);
        registerForm.AddField("password", _pass);
        registerForm.AddField("retype_password", _pass);

        yield return StartCoroutine(registerForm.Post<Response<string>>(api.URL("register"), ResponseHandler, ResponseType.landing));
    }
    
    public IEnumerator PostVoucher(string _code) 
    {
        WWWForm voucherForm = new WWWForm();
        voucherForm.AddField("person_id", PlayerPrefs.GetString("person_id"));
        voucherForm.AddField("code", _code);

        yield return StartCoroutine(voucherForm.Post<Response<string>>(api.URL("voucher"), ResponseHandler, ResponseType.voucher));
    }

    public void PostNilaiData(string nilai_id, string _kategoriSoal, string _kelas, string _semester, string _topik, string _paket, string _score)
    {
        WWWForm nilaiForm = new WWWForm();
        nilaiForm.AddField("id", nilai_id);
        nilaiForm.AddField("person_id", PlayerPrefs.GetString("person_id"));
        nilaiForm.AddField("ref_no", "");
        nilaiForm.AddField("category_id", _kategoriSoal);
        nilaiForm.AddField("class_id", _kelas);
        nilaiForm.AddField("semester_id", _semester);
        nilaiForm.AddField("topic_id", _topik);
        nilaiForm.AddField("packet_id", _paket);
        nilaiForm.AddField("created", DateTime.Now.ToString());
        nilaiForm.AddField("score", _score);

        StartCoroutine(nilaiForm.Post<Response<int>>(api.URL("create_nilai"), ResponseHandler, ResponseType.update));
    }

    public void PostCurrency(string _person_id, string _currency_type, string _amount)
    {
        //COIN, GEM, DIAMOND, KEY
        var currency_id = categoryList.categories.SingleOrDefault(s => s.group == "currency").GetCategoryDataBy<int>("id", _currency_type);
        var id = PlayerCurrencies.SingleOrDefault(c => c.currency_id == currency_id.ToString())?.id.ToString();
        id = id == null ? "" : id;

        WWWForm currForm = new WWWForm();
        currForm.AddField("id", id.ToString());
        currForm.AddField("person_id", PlayerPrefs.GetInt("person_id"));
        currForm.AddField("currency_id", currency_id);
        currForm.AddField("amount", _amount);
        currForm.AddField("notes", "");

        StartCoroutine(currForm.Post<Response<int>>(api.URL("create_currency"), ResponseHandler, ResponseType.update));
    }

    public void PostNewPlayerCard(string _cardID)
    {
        WWWForm newCardForm = new WWWForm();
        newCardForm.AddField("person_id", PlayerPrefs.GetString("person_id"));
        newCardForm.AddField("card_id", _cardID);
        newCardForm.AddField("qty", 1);
        newCardForm.AddField("level", 1);
        newCardForm.AddField("rank", 1);

        StartCoroutine(newCardForm.Post<Response<int>>(api.URL("create_card"), ResponseHandler, ResponseType.playerCard));
    }

    public void PostUpdatePlayerCard(string _id, string _cardID, string _qty, string _lvl, string _rank)
    {
        WWWForm updateCardForm = new WWWForm();
        updateCardForm.AddField("id", _id);
        updateCardForm.AddField("person_id", PlayerPrefs.GetString("person_id"));
        updateCardForm.AddField("card_id", _cardID);
        updateCardForm.AddField("qty", _qty);
        updateCardForm.AddField("level", _lvl);
        updateCardForm.AddField("rank", _rank);

        StartCoroutine(updateCardForm.Post<Response<int>>(api.URL("create_card"), ResponseHandler, ResponseType.playerCard));
    }

    public void PostUpdateProfile() 
    {
        string _schoolID = categoryList.categories.SingleOrDefault(c => c.group == "school").GetCategoryDataBy<string>("id", PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentSchool); 
        string _classID = categoryList.categories.SingleOrDefault(c => c.group == "class").GetCategoryDataBy<string>("id", PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentClass.ToString());
        string _schoolOther = _schoolID.Length > 0 ? "" : PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentSchool;

        WWWForm profileForm = new WWWForm();
        profileForm.AddField("id", PlayerPrefs.GetString("person_id"));
        profileForm.AddField("person_category_id", PlayerDataManager.Instance.PlayerData.PlayerAccountData.person_category_id);
        profileForm.AddField("school_id", _schoolID);
        profileForm.AddField("class_id", _classID);
        profileForm.AddField("name", PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentName);
        profileForm.AddField("email", "");
        profileForm.AddField("phone", PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentWhatsapp);
        profileForm.AddField("school_name", _schoolOther);
        profileForm.AddField("referral", PlayerDataManager.Instance.PlayerData.PlayerSubscriptionData.ReferralCode);
        profileForm.AddField("expired_date", PlayerDataManager.Instance.PlayerData.PlayerSubscriptionData.SubscriptionEndDate);
        profileForm.AddField("category", PlayerDataManager.Instance.PlayerData.PlayerAccountData.category);
        profileForm.AddField("parent_phone", PlayerDataManager.Instance.PlayerData.PlayerProfileData.ParentWhatsapp);

        StartCoroutine(profileForm.Post<Response<string>>(api.URL("edit_profile"), ResponseHandler, ResponseType.profile));
    }

    public void PostLastHeroSelected(string _heroName)
    {
        WWWForm selectedHeroForm = new WWWForm();
        selectedHeroForm.AddField("person_id", PlayerPrefs.GetString("person_id"));
        selectedHeroForm.AddField("detail[0][key]", "last_hero");
        selectedHeroForm.AddField("detail[0][id]", last_hero.id);
        selectedHeroForm.AddField("detail[0][value]", _heroName);

        StartCoroutine(selectedHeroForm.Post<Response<string>>(api.URL("create_key_value_data"), ResponseHandler, ResponseType.update));
    }

    public void PostTrainingArenaSaveData(string _skor, string _level, bool _isTamat, string _cat1, string _cat2, string _cat3, string _cat4, string _cat5)
    {
        string _savedata = $"{_skor}_{_level}_{_isTamat.ToString()}_{_cat1}_{_cat2}_{_cat3}_{_cat4}_{_cat5}";

        WWWForm trainingArenaForm = new WWWForm();
        trainingArenaForm.AddField("person_id", PlayerPrefs.GetString("person_id"));
        trainingArenaForm.AddField("detail[0][key]", "TA_savedata");
        trainingArenaForm.AddField("detail[0][id]", PlayerPrefs.GetInt("TA_savedata_id"));
        trainingArenaForm.AddField("detail[0][value]", _savedata);

        StartCoroutine(trainingArenaForm.Post<Response<string>>(api.URL("create_key_value_data"), ResponseHandler, ResponseType.update));
    }

    public void PostAdventureWorldSaveData(string _data)
    {
        string _savedata = $"{_data}";

        WWWForm trainingArenaForm = new WWWForm();
        trainingArenaForm.AddField("person_id", PlayerPrefs.GetString("person_id"));
        trainingArenaForm.AddField("detail[0][key]", "AW_savedata");
        trainingArenaForm.AddField("detail[0][id]", PlayerPrefs.GetInt("AW_savedata_id"));
        trainingArenaForm.AddField("detail[0][value]", _savedata);

        PlayerPrefs.SetString("AW_savedata", _savedata);

        StartCoroutine(trainingArenaForm.Post<Response<string>>(api.URL("create_key_value_data"), ResponseHandler, ResponseType.update));
    }

    public void PostQuizSaveData(string _kategoriSoal, string _topik, int _paket,  List<int> _correctAnswerList, bool _hasDoneFinal, int _hasEarnedReward, int _hasAnsweredFinal)
    {
        //Study / Treasure
        //3.1 etc..
        //paket 1 2 3 4 5
        string _defaultData = "-1_-1_-1_-1_-1_-1_-1_-1_-1_-1_-1_false_0_0";
        string _data = $"{string.Join("_", _correctAnswerList.ConvertAll(x => x.ToString()))}_{_hasDoneFinal.ToString()}_{_hasEarnedReward.ToString()}_{_hasAnsweredFinal.ToString()}";
        string _savedata = "";
        string _currentTopik = TopikQuizSaveData.SingleOrDefault(t => t.key == _kategoriSoal+"_"+_topik)?.value;
        
        List<string> PaketQuizSaveData = new List<string>();
        if(_currentTopik != null) PaketQuizSaveData.AddRange(_currentTopik.Split('@'));
        else 
        {
            for (var i = 0; i < 5; i++)
            {
                PaketQuizSaveData.Add(_defaultData);
            }
        }
        PaketQuizSaveData[_paket-1] = _data;
        _savedata = string.Join("@", PaketQuizSaveData);

        WWWForm quizDataForm = new WWWForm();
        quizDataForm.AddField("person_id", PlayerPrefs.GetString("person_id"));
        quizDataForm.AddField("detail[0][key]", $"{_kategoriSoal}_{_topik}");
        quizDataForm.AddField("detail[0][id]", TopikQuizSaveData.SingleOrDefault(t => t.key == $"{_kategoriSoal}_{_topik}").id);
        quizDataForm.AddField("detail[0][value]", _savedata);

        StartCoroutine(quizDataForm.Post<Response<string>>(api.URL("create_key_value_data"), ResponseHandler, ResponseType.update));
    }

#endregion
#region Get 
    public IEnumerator GetSoalList(string _kategoriSoal, string _kelas = "", string _semester = "", string _topik = "", string _paket = "")
    {
        _kategoriSoal = categoryList.categories.SingleOrDefault(c => c.group == "question_type").GetCategoryDataBy<string>("id", _kategoriSoal);
        _kelas = _kelas == "" ? "" : categoryList.categories.SingleOrDefault(c => c.group == "class").GetCategoryDataBy<string>("id", _kelas);
        _semester = _semester == "" ? "" : categoryList.categories.SingleOrDefault(c => c.group == "semester").GetCategoryDataBy<string>("id", _semester);
        _topik = _topik == "" ? "" : categoryList.categories.SingleOrDefault(c => c.group == "topic").GetCategoryDataBy<string>("id", _topik);
        _paket = _paket == "" ? "" : categoryList.categories.SingleOrDefault(c => c.group == "packet").GetCategoryDataBy<string>("id", _paket);

        yield return StartCoroutine(api.URL("list_soal", "", "", _kategoriSoal, _kelas, _semester, _topik, _paket).Get<Response<List<SoalData>>>(ResponseHandler, ResponseType.soal));
    }

    public IEnumerator GetIntroList(string _kategoriSoal, string _kelas, string _semester, string _topik, string _paket)
    {
        _kategoriSoal = categoryList.categories.SingleOrDefault(c => c.group == "question_type").GetCategoryDataBy<string>("id", _kategoriSoal);
        _kelas = categoryList.categories.SingleOrDefault(c => c.group == "class").GetCategoryDataBy<string>("id", _kelas);
        _semester = categoryList.categories.SingleOrDefault(c => c.group == "semester").GetCategoryDataBy<string>("id", _semester);
        _topik = categoryList.categories.SingleOrDefault(c => c.group == "topic").GetCategoryDataBy<string>("id", _topik);
        _paket = categoryList.categories.SingleOrDefault(c => c.group == "packet").GetCategoryDataBy<string>("id", _paket);

        yield return StartCoroutine(api.URL("list_intro", null, null, _kategoriSoal, _kelas, _semester, _topik, _paket).Get<Response<List<IntroData>>>(ResponseHandler, ResponseType.intro));
    }

    public IEnumerator GetNilaiData(string _kategoriSoal, string _kelas, string _semester, string _topik, string _paket)
    {
        _kategoriSoal = categoryList.categories.SingleOrDefault(c => c.group == "question_type").GetCategoryDataBy<string>("id", _kategoriSoal);
        _kelas = categoryList.categories.SingleOrDefault(c => c.group == "class").GetCategoryDataBy<string>("id", _kelas);
        _semester = categoryList.categories.SingleOrDefault(c => c.group == "semester").GetCategoryDataBy<string>("id", _semester);
        _topik = categoryList.categories.SingleOrDefault(c => c.group == "topic").GetCategoryDataBy<string>("id", _topik);
        _paket = categoryList.categories.SingleOrDefault(c => c.group == "packet").GetCategoryDataBy<string>("id", _paket);

        yield return StartCoroutine(api.URL("list_nilai", PlayerPrefs.GetString("person_id"), null, _kategoriSoal, _kelas, _semester, _topik, _paket).Get<Response<List<NilaiData>>>(ResponseHandler, ResponseType.nilai));
    }

    public IEnumerator GetPlayerCurrency()
    {
        yield return StartCoroutine((api.URL("player_currency")+PlayerPrefs.GetString("person_id")).Get<Response<List<CurrencyData>>>(ResponseHandler, ResponseType.currency));
    }

    public IEnumerator GetCardList(string _isHero = "")  //0 = not hero, 1 = hero, null = all
    {
        yield return StartCoroutine((api.URL("cards")+_isHero).Get<Response<List<CardData>>>(ResponseHandler, ResponseType.card));
    }

    public IEnumerator GetPlayerCardList()
    {
        yield return StartCoroutine((api.URL("playercards")+PlayerPrefs.GetString("person_id")).Get<Response<List<PlayerCardData>>>(ResponseHandler, ResponseType.playerCard));
    }

    public void GetCategoryList(string _group)
    {
        StartCoroutine((api.URL("category")+_group).Get<Response<List<CategoryData>>>(ResponseHandler, ResponseType.categoryList));
    }

    public IEnumerator GetCategoryListRegister()
    {
        yield return StartCoroutine(api.URL("category_register").Get<Response<List<CategoryData>>>(ResponseHandler, ResponseType.categoryRegister));
    }

    public IEnumerator GetProfile()
    {
        yield return StartCoroutine((api.URL("profile")+PlayerPrefs.GetString("person_id")).Get<Response<ProfileData>>(ResponseHandler, ResponseType.profile));
    }

    public IEnumerator GetLastHeroSelected()
    {
        yield return StartCoroutine(api.URL("last_hero", PlayerPrefs.GetString("person_id")).Get<Response<List<KeyValueData>>>(ResponseHandler, ResponseType.last_hero));
    }

    public IEnumerator GetGameSaveData()
    {
        yield return StartCoroutine(api.URL("savedata", PlayerPrefs.GetString("person_id")).Get<Response<List<KeyValueData>>>(ResponseHandler, ResponseType.savedata));
    }

    public IEnumerator GetQuizSaveData(string _kategoriSoal, string _paket)
    {
        yield return StartCoroutine(api.URL("quizdata", PlayerPrefs.GetString("person_id"), "", _kategoriSoal, "", "", "", _paket).Get<Response<List<KeyValueData>>>(ResponseHandler, ResponseType.quizdata));
    }

#endregion
#endregion

#region APIHandlers
    public void ResponseHandler<T>(T _response, string _typeResponse) 
    {
        switch (_typeResponse)
        {
            case"landing" : 
                Response<string> landing = (Response<string>)Convert.ChangeType(_response, typeof(Response<string>));
                landingLog.text = landing.message;
                PlayerPrefs.SetString("token", landing.data);
                if(landing.data != "") StartCoroutine(LandingPageHandler());
                break;
            case"account" : 
                Response<AccountData> account = (Response<AccountData>)Convert.ChangeType(_response, typeof(Response<AccountData>));
                if(account.data.person_id != null) PlayerPrefs.SetString("person_id", account.data.person_id);
                PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentWhatsapp = account.data.username;
                break;
            case"profile" : 
                Response<ProfileData> profile = (Response<ProfileData>)Convert.ChangeType(_response, typeof(Response<ProfileData>));
                ProfileDataHandler(profile.data);
                break;
            case"voucher" : 
                Response<string> voucher = (Response<string>)Convert.ChangeType(_response, typeof(Response<string>));
                voucherLog = voucher.message;
                StartCoroutine(GetProfile());  
                break;
            case"categoryRegister" : 
                Response<List<CategoryData>> categoryRegister = (Response<List<CategoryData>>)Convert.ChangeType(_response, typeof(Response<List<CategoryData>>));
                CategoryRegisterDatas = categoryRegister.data;
                break;
            case"categoryList" : 
                Response<List<CategoryData>> category = (Response<List<CategoryData>>)Convert.ChangeType(_response, typeof(Response<List<CategoryData>>));   
                CategoryListHandler(category.data[0].group_by, category.data);
                void CategoryListHandler(string _groupBy, List<CategoryData> _categoryDatas) => categoryList.categories.SingleOrDefault(x => x.name == _groupBy).LoadCategory(_categoryDatas);
                break;
            case"nilai" :
                Response<List<NilaiData>> nilaiList = (Response<List<NilaiData>>)Convert.ChangeType(_response, typeof(Response<List<NilaiData>>));
                PlayerNilai = nilaiList.data;
                break;
            case"leaderboard" : Debug.Log("leaderboard");
                break;
            case"soal" : 
                Response<List<SoalData>> soalList = (Response<List<SoalData>>)Convert.ChangeType(_response, typeof(Response<List<SoalData>>));
                Soals = soalList.data;
                break;
            case"card" : 
                Response<List<CardData>> cardList = (Response<List<CardData>>)Convert.ChangeType(_response, typeof(Response<List<CardData>>));
                AllCards = cardList.data;
                break;
            case "playerCard": 
                Response<List<PlayerCardData>> playerCardList = (Response<List<PlayerCardData>>)Convert.ChangeType(_response, typeof(Response<List<PlayerCardData>>));
                PlayerCards = playerCardList.data;
                break;
            case "last_hero": 
                Response<List<KeyValueData>> lastHero = (Response<List<KeyValueData>>)Convert.ChangeType(_response, typeof(Response<List<KeyValueData>>));
                last_hero = lastHero.data[0];
                break;
            case"intro" : 
                Response<List<IntroData>> introList = (Response<List<IntroData>>)Convert.ChangeType(_response, typeof(Response<List<IntroData>>));
                Intros = introList.data;
                break;
            case "currency":
                Response<List<CurrencyData>> currencyList = (Response<List<CurrencyData>>)Convert.ChangeType(_response, typeof(Response<List<CurrencyData>>));
                PlayerCurrencies = currencyList.data;
                CurrencyHandler(PlayerCurrencies);
                break;
            case "savedata":
                Response<List<KeyValueData>> saveDataList = (Response<List<KeyValueData>>)Convert.ChangeType(_response, typeof(Response<List<KeyValueData>>));
                SaveDataHandler(saveDataList.data);
                break;
            case "quizdata":
                Response<List<KeyValueData>> quizDataList = (Response<List<KeyValueData>>)Convert.ChangeType(_response, typeof(Response<List<KeyValueData>>));
                TopikQuizSaveData = quizDataList.data;
                break;
        }
    }

    private float ParseCurrency(string _currencyType) => String.IsNullOrEmpty(PlayerCurrencies.SingleOrDefault(c => c.currency_label == _currencyType)?.amount) ? 0f : float.Parse(PlayerCurrencies.SingleOrDefault(c => c.currency_label == _currencyType)?.amount);

    private void CurrencyHandler(List<CurrencyData> currencyList)
    {
        PlayerDataManager.Instance.PlayerData.PlayerCurrencyData.TotalCoins = (int)ParseCurrency("COIN");
        PlayerDataManager.Instance.PlayerData.PlayerCurrencyData.TotalGems = (int)ParseCurrency("GEM");
        PlayerDataManager.Instance.PlayerData.PlayerCurrencyData.TotalDiamonds = (int)ParseCurrency("DIAMOND");
        PlayerDataManager.Instance.PlayerData.PlayerCurrencyData.TotalKey = (int)ParseCurrency("KEY");
    }

    private void SaveDataHandler(List<KeyValueData> saveDataList)
    {
        PlayerPrefs.SetInt("TA_savedata_id", saveDataList.SingleOrDefault(s => s.key.Contains("TA")).id);
        PlayerPrefs.SetString("TA_savedata", saveDataList.SingleOrDefault(s => s.key.Contains("TA")).value);
        PlayerPrefs.SetInt("AW_savedata_id", saveDataList.SingleOrDefault(s => s.key.Contains("AW")).id);
        PlayerPrefs.SetString("AW_savedata", saveDataList.SingleOrDefault(s => s.key.Contains("AW")).value);
    }

    public IEnumerator LandingPageHandler()
    {
        categoryList.GetCategories();
        yield return StartCoroutine(api.URL("account").Get<Response<AccountData>>(ResponseHandler, ResponseType.account));
        yield return StartCoroutine(GetProfile());
        yield return StartCoroutine(GetCardList());
        yield return StartCoroutine(GetPlayerCardList());
        yield return StartCoroutine(GetPlayerCurrency());
        yield return StartCoroutine(GetGameSaveData());
        yield return StartCoroutine(GetLastHeroSelected());
        LoadPlayerCards();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void ProfileDataHandler(ProfileData _data)
    {
        PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentName = _data.name;
        PlayerDataManager.Instance.PlayerData.PlayerProfileData.ParentWhatsapp = _data.parent_phone;
        PlayerDataManager.Instance.PlayerData.PlayerSubscriptionData.SubscriptionEndDate = _data.expired_date;
        PlayerDataManager.Instance.PlayerData.PlayerSubscriptionData.ReferralCode = _data.referral;
        PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentSchool = _data.school_name == "" ? categoryList.categories.Single(c => c.group == "school").GetCategoryDataBy<string>("label", null, _data.school_id.ToString()) : _data.school_name;

        switch (_data.class_id)
        {
            case "3001": PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentClass = Kelas.Kelas3;
                break;
            case "3002": PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentClass = Kelas.Kelas4;
                break;
            case "3003": PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentClass = Kelas.Kelas5;
                break;
            case "3004": PlayerDataManager.Instance.PlayerData.PlayerProfileData.StudentClass = Kelas.Kelas6;
                break;
        }

        PlayerDataManager.Instance.PlayerData.PlayerAccountData.category = _data.category;
        PlayerDataManager.Instance.PlayerData.PlayerAccountData.person_category_id = _data.person_category_id;
    }

    private void LoadPlayerCards()
    {
        if (PlayerCards != null)
        {
            PlayerDataManager.Instance.PlayerData.HeroesOwnedDataList.ForEach(h =>
            {
                h.IsSelected = false;
                for (int i = 0; i < PlayerCards.Count; i++)
                {
                    if (h.Name == PlayerCards[i].card_name)
                    {
                        h.IsUnlocked = true;
                        h.Level = Int16.Parse(PlayerCards[i].level);
                        h.DuplicateCard = Int16.Parse(PlayerCards[i].qty);
                    }
                }

                if (h.Name == last_hero.value) h.IsSelected = true;
            });
            PlayerDataManager.Instance.PlayerData.TroopsOwnedDataList.ForEach(t =>
            {
                t.IsSelected = false;
                for (int i = 0; i < PlayerCards.Count; i++)
                {
                    if (t.Name == PlayerCards[i].card_name)
                    {
                        t.IsUnlocked = true;
                        t.Level = Int16.Parse(PlayerCards[i].level);
                        t.DuplicateCard = Int16.Parse(PlayerCards[i].qty);
                    }
                }
            });
        }
    }

#region RegisterCategoryList
    List<CategoryData> CategoryRegisterDatas = new List<CategoryData>();
    public List<string> CategoryDataListBy(string _group, string _sortBy)
    {
        List<string> categories = new List<string>();      

        CategoryRegisterDatas.ForEach(categoryData => 
        {
            if(categoryData.group_by == _group && _sortBy == "label") categories.Add(categoryData.label);
            if(categoryData.group_by == _group && _sortBy == "id") categories.Add(categoryData.id.ToString());
        });
        
        return categories;
    }

    public T CategoryDataBy<T>(string _data, string _group = "", string _label = "", string _id = "")
    {
        int tempID = 0;
        string tempLabel = "";
        T t = default(T);
        switch (_data)
        {
            case "id":
                tempID = 0;
                CategoryRegisterDatas.ForEach(categoryData => 
                { 
                    if(categoryData.group_by == _group || 
                       categoryData.label == _label ||
                       categoryData.id.ToString() == _id) tempID = categoryData.id; 

                });
                t = (T)Convert.ChangeType(tempID, typeof(T));
                break;
            case "label":
                tempLabel = "";
                CategoryRegisterDatas.ForEach(categoryData => 
                { 
                    if(categoryData.group_by == _group || 
                       categoryData.label == _label ||
                       categoryData.id.ToString() == _id) tempLabel = categoryData.label; 

                });
                t = (T)Convert.ChangeType(tempLabel, typeof(T));
                break;
        }
        return t;
    }
#endregion

#endregion
}


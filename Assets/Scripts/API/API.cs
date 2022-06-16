using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "API", menuName = "Datas/API")]
public class API : ScriptableObject
{
    public string base_url = "";
    public string intro_id = "961";
    public List<URL> ListAPI = new List<URL>();

    private void OnEnable() { if(ListAPI.Count == 0) ListAPI.AddRange(FindObjectsOfType<URL>()); }

    public string URL(string api, string _personID = "", string _categoryGroupBy = "", string _kategoriSoal = "", string _kelas = "", string _semester = "", string _topik = "", string _paket = "")
    {
        string url = ""; 
        
        ListAPI.ForEach(x => { if(x.urlName == api) url = base_url + x.url; });

        if (api == "list_soal") url = base_url + $"question/list?category_id={_kategoriSoal}&class_id={_kelas}&semester_id={_semester}&topic_id={_topik}&packet_id={_paket}";
        else if (api == "list_intro") url = base_url + $"question-intro/list?category_id={intro_id}&question_category_id={_kategoriSoal}&class_id={_kelas}&semester_id={_semester}&topic_id={_topik}&packet_id={_paket}";
        else if (api == "list_nilai") url = base_url + $"score/list?person_id={_personID}&category_id={_kategoriSoal}&class_id={_kelas}&semester_id={_semester}&topic_id={_topik}&packet_id={_paket}";
        else if (api == "last_hero") url = base_url + $"person-config/list?person_id={_personID}&keyword=last_hero";
        else if (api == "savedata") url = base_url + $"person-config/list?person_id={_personID}&keyword=savedata";
        else if (api == "quizdata") url = base_url + $"person-config/list?person_id={_personID}&keyword={_kategoriSoal}_{_paket}";
    
        return url;
    }
}

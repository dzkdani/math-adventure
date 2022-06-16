using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mono : MonoBehaviour
{
    public static Mono Instance;
    private void Awake() {
        Mono.Instance = this;
    }
}

[CreateAssetMenu(fileName = "Category", menuName = "Datas/Category")]
public class Category : ScriptableObject
{
    [System.Serializable]
    public struct CategoryDataStruct
    {
        public string label;
        public string id;
        public string note;

        public CategoryDataStruct(string _label, string _id, string _note)
        {
            this.label = _label;
            this.id = _id;
            this.note = _note;
        } 
    }
    public string group;    
    public List<CategoryDataStruct> categoryDataStructs = new List<CategoryDataStruct>();

    private void Awake() {
        group = this.name;
    }   

    private void OnEnable() 
    {
        categoryDataStructs.Clear();
    }

    private void OnDisable() {
        categoryDataStructs.Clear();
    }

    public void GetCategoryDatas()
    {
        APIManager.Instance.GetCategoryList(group);
    }

    public void LoadCategory(List<CategoryData> _categoryDatas)
    {
        List<CategoryData> categoryDataList = new List<CategoryData>(_categoryDatas);
        List<string> labels = new List<string>();
        labels = ParseCategoryDataListBy(_categoryDatas, "label");
        List<string> ids = new List<string>();
        ids = ParseCategoryDataListBy(_categoryDatas, "id");
        List<string> notes = new List<string>();
        notes = ParseCategoryDataListBy(_categoryDatas, "notes");

        int count = categoryDataList.Count;
        for (var i = 0; i < count; i++)
        {
            CategoryDataStruct category = new CategoryDataStruct(labels[i], ids[i], notes[i]);
            categoryDataStructs.Add(category);
        }
    }

    private List<string> ParseCategoryDataListBy(List<CategoryData> _categoryDatas, string _sortBy)
    {
        List<string> dataList = new List<string>();      
        _categoryDatas.ForEach(data => 
        {
            if(_sortBy == "label") dataList.Add(data.label);
            if(_sortBy == "id") dataList.Add(data.id.ToString());
            if(_sortBy == "notes") dataList.Add(data.notes);
        });
        return dataList;
    }

    public T GetCategoryDataBy<T>(string _data, string _label = "", string _id = "", string _note = "")
    {
        string tempID = categoryDataStructs.SingleOrDefault(c => c.label == _label).id;
        string tempLabel = categoryDataStructs.SingleOrDefault(c => c.id == _id).label;
        string tempNote = categoryDataStructs.SingleOrDefault(c => c.note == _note).note;
        T t = default(T);
        switch (_data)
        {
            case "id":
                tempID = tempID == null ? "" : tempID;
                t = (T)Convert.ChangeType(tempID, typeof(T));
                break;
            case "label":
                tempLabel = tempLabel == null ? "" : tempLabel;
                t = (T)Convert.ChangeType(tempLabel, typeof(T));
                break;
            case "notes":
                tempNote = tempNote == null ? "" : tempNote;
                t = (T)Convert.ChangeType(tempNote, typeof(T));
                break;
        }
        return t;
    }

    public List<string> GetCategoryDataListBy(string _sortBy)
    {
        List<string> dataList = new List<string>();      
        categoryDataStructs.ForEach(data => 
        {
            if(_sortBy == "label") dataList.Add(data.label);
            if(_sortBy == "id") dataList.Add(data.id.ToString());
        });
        return dataList;
    }
}

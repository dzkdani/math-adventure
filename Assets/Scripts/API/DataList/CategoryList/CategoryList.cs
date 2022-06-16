using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CategoryList", menuName = "Datas/CategoryList")]
public class CategoryList : ScriptableObject
{
    public List<Category> categories = new List<Category>();
    public void GetCategories() => categories.ForEach(category => category.GetCategoryDatas());
}

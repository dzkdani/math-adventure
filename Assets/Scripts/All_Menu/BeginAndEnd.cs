using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeginAndEnd : MonoBehaviour 
{
    public int id;
    public string type;
    public bool bulean;
    public RectTransform rect;
    public TextMeshProUGUI textMesh;
    public Image image;
    public GameObject[] WindowList;
    public string WindowExclude;
    public string warning;

    public TroopData troopd;
    public HeroData herod;
    public PlayerData.UnitsOwnedData unitData;
}

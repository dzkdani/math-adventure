using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TroopData", menuName = "Datas/Troops/TroopData")]
public class TroopData : ScriptableObject
{
    [Header("GENERAL")] 
    [Space(5)]
    public string Name;
    public UnitData.AttackType AttackType;
    public Sprite Icon;
    public GameObject Model; 
    public ProjectileController Projectile;
    public Sprite[] RankIcons;
    public GameObject[] RankModels;

    [Header("DATA")]
    [Space(5)]
    public float Level;
    public float BaseHP;
    public float IncreaseHP;
    public float BaseATK;
    public float IncreaseATK;
    public float BaseDEF;
    public float IncreaseDEF;
    public float BaseATKSPD;
    public float BaseMOVSPD;
    public float BaseATKRANGE;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroData", menuName = "Datas/Heroes/HeroData")]
public class HeroData : ScriptableObject
{
    [Header("GENERAL")]
    [Space(5)]
    public string Name;
    public string Nickname;
    [TextArea]
    public string Description;
    public UnitData.AttackType Type;
    public Sprite Icon;
    public GameObject Prefab;
    public int SummonCost;
    public int SummonCooldown;
    public ProjectileController Projectile;
    public Sprite[] RankIcons;
    public GameObject[] RankModels;


    [Header("DATA")]
    [Space(5)]
    public int Level;
    public float MaxMP;
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

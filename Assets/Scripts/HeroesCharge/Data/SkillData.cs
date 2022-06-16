using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Datas/Skills/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("GENERAL")] 
    [Space(5)]
    public string Name;
    public string Description;
    public int Cooldown;
    public int Range;
    public Sprite Icon;

    [Header("EFFECTS")]
    [Space(5)]
    public List<SkillEffectData> SkillEffectList;
}

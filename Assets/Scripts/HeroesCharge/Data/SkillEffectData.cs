using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillEffectData", menuName = "Datas/Skills/SkillEffectData")]
public class SkillEffectData : ScriptableObject
{
    public SkillType Type;

    public DamageTypeData DamageType;
    public DamageOverTimeTypeData DamageOverTimeType;
    public HealTypeData HealType;
    public BuffTypeData BuffType;
    public NerfTypeData NerfType; 

    [System.Serializable]
    public enum SkillType
    {
        DAMAGE,
        DAMAGEOVERTIME,
        HEAL,
        BUFF,
        NERF
    }
    [System.Serializable]
    public enum StatType
    {
        HP,
        ATK,
        DEF,
        MOVSPD,
        ATKSPD,
    }

    [System.Serializable]
    public class DamageTypeData
    {
        public float DamageMultiplier;
    }
    [System.Serializable]
    public class DamageOverTimeTypeData
    {
        public float DamageMultiplier;
        public int NumberDamageTicks;
    }
    [System.Serializable]
    public class HealTypeData
    {
        public float HealMultiplier;
    }
    [System.Serializable]
    public class BuffTypeData
    {
        public float BuffMultiplier;
        public StatType BuffStat;
        public float Duration;
    }
    [System.Serializable]
    public class NerfTypeData
    {
        public float NerfMultiplier;
        public StatType NerfStat;
        public float Duration;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSelectUIHandler : MonoBehaviour
{
    [SerializeField] string HeroFirstName;
    [SerializeField] string HeroSecondName;
    [SerializeField] string HeroDesc;
    [SerializeField] Sprite HeroImgSprite;
    [SerializeField] HeroData heroData;

    private void Awake() {
        HeroFirstName = heroData?.Name;
        HeroSecondName = heroData?.Nickname;
        HeroDesc = heroData?.Description;
    }

    public Sprite GetHeroSprite() { return HeroImgSprite; }

    public string GetHeroName() { return HeroFirstName; }

    public string GetHeroNick() { return HeroSecondName; }

    public string GetHeroDesc() { return HeroDesc; }
}

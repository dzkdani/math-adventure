using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public float MaxExplosionTime;
    [SerializeField]
    private float ExplosionLifeTime;

    private void OnEnable() {
        ExplosionLifeTime = MaxExplosionTime;
    }

    private void Update() {
        ExplosionLifeTime -= Time.deltaTime;
        if (ExplosionLifeTime <= 0f)
        {
            gameObject.SetActive(false);
            ExplosionLifeTime = MaxExplosionTime;
        }
    }
}

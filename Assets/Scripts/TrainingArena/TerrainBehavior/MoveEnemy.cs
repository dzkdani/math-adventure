using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    public float speed;
   // public Rigidbody2D rb;
    public Transform Enemy;
    public bool left, right, isVertical, isRocket, isSTOP = false;
    public AudioSource audioSource;

    public Vector3 kiri;
    public Vector3 kanan;

    bool isDonePlayingAudio = false;
    int time = 0;

    [SerializeField]
    TrainingArena_GM GM;

    void Start()
    {
        GM = FindObjectOfType<TrainingArena_GM>();
    }

    void Update()
    {
        if (GM.UI_DisplayFinalScore.activeSelf) Destroy(this.gameObject);
    }

    void LateUpdate()
    {
        if (!isVertical) moveCharacter();
        else moveCharacter_h();

        //if (isSTOP.Equals(false))
        //{
        //    if (!isVertical) moveCharacter();
        //    else moveCharacter_h();
        //}
        //else
        //{
        //    time++;
        //    if (time.Equals(100)) { time = 0; isSTOP = false; }
        //}
    }

    void moveCharacter()
    {
        if (right)
        {
            Enemy.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            if (Enemy.localPosition.x > kanan.x) { Enemy.localPosition = kiri; }
        }
        else if (left)
        {
            Enemy.Translate(new Vector3(speed * Time.deltaTime * -1, 0, 0));
            if (Enemy.localPosition.x < kiri.x) { Enemy.localPosition = kanan; }
        }
		
		if (Enemy.position.z < -20){Destroy(this.gameObject);}
    }

    void moveCharacter_h()
    {
        if (right)
        {
            Enemy.Translate(new Vector3(0,0,speed * Time.deltaTime));
           /// if (Enemy.localPosition.z > kanan.z) { Enemy.localPosition = kiri; }
        }
        else if (left)
        {
            Enemy.Translate(new Vector3(0,0, speed * Time.deltaTime * -1));
           /// if (Enemy.localPosition.z < kiri.z) { Enemy.localPosition = kanan; }
        }
    }

    private void OnBecameVisible()
    {
        if (isRocket && !isDonePlayingAudio)
        {
            if (!audioSource.isPlaying)
            {
                isDonePlayingAudio = true;
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
    }

    void OnTriggerEnter(Collider colli) {
        if (colli.gameObject.tag.Equals("reverse"))
        {
            if (colli.gameObject.name.Equals("PointDestroy1"))
            {
                Destroy(this.gameObject);
            }
        }
    }
}

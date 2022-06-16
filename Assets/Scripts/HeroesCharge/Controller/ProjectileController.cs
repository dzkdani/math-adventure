using UnityEngine;
using Spine.Unity;
 
public class ProjectileController : MonoBehaviour
{
    [Header("Projectile Status")]
    [Space(5)]
    public string shootBy;
    public float arrowDamage;
    [SerializeField]
    private float projectileSpeed = 5f;
    [SerializeField]
    private float timeBeforeSelfDestroy = 5f;
    [SerializeField]
    private float curTimeBeforeSelfDestroy;

    [SerializeField]
    private GameObject projectileTarget;
    private SkeletonAnimation skeletonAnimation;
    private SkeletonDataAsset initState;

    private void Awake() {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        initState = skeletonAnimation.skeletonDataAsset;
    }

    void Start()
    {
        curTimeBeforeSelfDestroy = timeBeforeSelfDestroy;
        if (shootBy == "Player")
        {
            return;
        }
        if (shootBy == "Enemy")
        {
            gameObject.transform.Rotate(new Vector3(0, 180, 0));
        }
    }
    void Update()
    {
        curTimeBeforeSelfDestroy -= Time.deltaTime;
        if (curTimeBeforeSelfDestroy <= 0)
        {
            Destroy(this.gameObject);
        }
        if (!GameOverController.Instance.CheckGameOver() && !PauseController.Instance.IsPaused())
        {
            //move towards target
            if (projectileTarget.gameObject)
            {
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), 
                    new Vector2(projectileTarget.transform.position.x, projectileTarget.transform.position.y+1.5f), 
                    projectileSpeed * Time.deltaTime);
            }
            else
            {
                Destroy(this.gameObject);
            }
                
        }
    }
    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (shootBy == "Player") 
        {
            if (collision.CompareTag("Enemy"))
            {
                if (collision.GetComponent<UnitController>() != null)
                {
                    collision.GetComponent<UnitController>().CalculateDamage(arrowDamage);
                }
                Destroy(this.gameObject);
            }
        }
        else if (shootBy == "Enemy")
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.GetComponent<UnitController>() != null)
                {
                    collision.GetComponent<UnitController>().CalculateDamage(arrowDamage);
                }
                Destroy(this.gameObject); 
            }
        }
    }

    public void SetProjectileTarget(GameObject _target)
    {
        projectileTarget = _target;
    }
}

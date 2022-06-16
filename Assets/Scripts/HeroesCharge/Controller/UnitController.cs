using DragonBones;
using System.Collections;
using UnityEngine;
using TMPro;
 
public class UnitController : MonoBehaviour
{
    [Header("Unit Status")]
    [Space(5)]
    public UnitData.AttackType AttackType; 
    public ProjectileController Projectile;

    [SerializeField]
    private float HP;
    [SerializeField]
    private float ATK;
    [SerializeField]
    private float DEF;
    [SerializeField]
    private float ATKSPD;
    [SerializeField]
    private float MOVSPD;
    [SerializeField]
    private float ATKRANGE;

    [Header("Unit Action")]
    [Space(5)]
    public bool IsMoving;
    public bool IsDead;
    public bool IsCurWave;
    public bool IsHero;
    public bool IsSkillReady;
    private bool isAttackAnimState;
    private bool isHurtAnimState;
    private bool isWalkingAnimState;
    private float attackCooldown;
    private WaveController waveController;

    #region UNITY

    void Start()
    {
        waveController = FindObjectOfType<WaveController>();

        IsMoving = false;
        IsSkillReady = false;
    }
    void Update()
    {
        CheckMovement();
        CheckAttack();
        CheckPaused();
    }
    #endregion

    #region INIT
    public void InitData(
        UnitData.AttackType _troopType,
        ProjectileController _projectile,
        float _HP,
        float _ATK,
        float _DEF,
        float _ATKSPD,
        float _MOVSPD,
        float _ATKRANGE
        )
    {
        AttackType = _troopType;
        Projectile = _projectile;
        HP = _HP;
        ATK = _ATK;
        DEF = _DEF;
        ATKSPD = _ATKSPD;
        MOVSPD = _MOVSPD;
        ATKRANGE = _ATKRANGE;
    }
    public void InitPlayerModel()
    {   
        tag = "Player";
    }
    public void InitEnemyModel()
    {
        tag = "Enemy";
    }
    #endregion
    
    #region PAUSE
    private void CheckPaused()
    {
        if (PauseController.Instance.IsPaused() || GameOverController.Instance.CheckGameOver())
        {
            //idle
            transform.GetChild(0).GetComponent<UnityArmatureComponent>().gameObject.SetActive(true);
            transform.GetChild(1).GetComponent<UnityArmatureComponent>().gameObject.SetActive(false);
            transform.GetChild(2).GetComponent<UnityArmatureComponent>().gameObject.SetActive(false);
        }
    }
    #endregion

    #region MOVEMENT
    private void CheckMovement()
    {
        if (IsDead || !IsCurWave || PauseController.Instance.IsPaused() || GameOverController.Instance.CheckGameOver())
        {
            return;
        }
        if (IsMoving && !PauseController.Instance.IsPaused() && !GameOverController.Instance.CheckGameOver())
        {
            if (!isWalkingAnimState)
            { 
                //walk
                transform.GetChild(0).GetComponent<UnityArmatureComponent>().gameObject.SetActive(false);
                transform.GetChild(1).GetComponent<UnityArmatureComponent>().gameObject.SetActive(true);
                transform.GetChild(2).GetComponent<UnityArmatureComponent>().gameObject.SetActive(false);
            }
            if (gameObject.CompareTag("Player"))
            {
                transform.Translate(Vector2.right * MOVSPD * Time.deltaTime);
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                transform.Translate(Vector2.left * MOVSPD * Time.deltaTime);
            }
        }
    }
    #endregion

    #region ATTACK
    private void CheckAttack()
    {
        if (IsDead || !IsCurWave || PauseController.Instance.IsPaused() || GameOverController.Instance.CheckGameOver())
        {
            return;
        }
        GameObject target = FindClosestTarget();
        if (target != null)
        {
            IsMoving = false;
            if (attackCooldown > 0)
            {
                attackCooldown -= Time.deltaTime;
            }
            else
            {
                //Melee Attack
                if (AttackType == UnitData.AttackType.MELEE)
                {
                    transform.GetChild(0).GetComponent<UnityArmatureComponent>().gameObject.SetActive(false);
                    transform.GetChild(1).GetComponent<UnityArmatureComponent>().gameObject.SetActive(false);
                    transform.GetChild(2).GetComponent<UnityArmatureComponent>().gameObject.SetActive(true);

                    if (target.GetComponent<UnitController>() != null)
                    {
                        target.GetComponent<UnitController>().CalculateDamage(ATK);
                    }
                }
                //Range Attack
                else if (AttackType == UnitData.AttackType.RANGE)
                {
                    transform.GetChild(0).GetComponent<UnityArmatureComponent>().gameObject.SetActive(false);
                    transform.GetChild(1).GetComponent<UnityArmatureComponent>().gameObject.SetActive(false);
                    transform.GetChild(2).GetComponent<UnityArmatureComponent>().gameObject.SetActive(true);

                    //going to make this obj pool later
                    ProjectileController spawnProjectile = Instantiate(Projectile, GameObject.Find("Projectiles").transform);
                    spawnProjectile.transform.position = new Vector3(transform.position.x + 2, transform.position.y + 1, 0);
                    spawnProjectile.shootBy = gameObject.tag;
                    spawnProjectile.arrowDamage = ATK;
                    spawnProjectile.SetProjectileTarget(target);
                }
                attackCooldown = ATKSPD;
            }
        }
        else
        {
            IsMoving = true;
        }
    }

    private GameObject FindClosestTarget()
    {
        GameObject[] gos = null;
        if (gameObject.CompareTag("Player"))
        {
            gos = GameObject.FindGameObjectsWithTag("Enemy");
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            gos = GameObject.FindGameObjectsWithTag("Player");
        }
        GameObject target = null;
        float distance = ATKRANGE;
        float thresholdY = 2f;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistanceX = Mathf.Abs(diff.x);
            float curDistanceY = Mathf.Abs(diff.y);
            if (curDistanceX < distance)
            {
                if (curDistanceY < thresholdY)
                {
                    target = go;
                    distance = curDistanceX;                          
                }
                else
                {
                    //dunno how to deal with Y difference
                    target = go;   
                }
            }
        }
        return target;
    }
    public void CalculateDamage(float _ATK)
    {
        if (IsDead || !IsCurWave)
        {
            return;
        }
        isHurtAnimState = true;
        isWalkingAnimState = false;

        //damaged

        float damage = _ATK - DEF;
        if (damage <= 0)
        {
            damage = 0;
        } 
        DisplayDamagePopUp(damage);
        HUDController.Instance.UpdateCurHP(damage, name);
        HP -= damage;
        if (HP <= 0)
        {
            Die();
        }
    }

    private void DisplayDamagePopUp(float _damage)
    {
        GameObject damageTxt = ObjectPoolManager.Instance.GetPooledObjects(OBJPOOL.damagetxt);
        if (damageTxt != null)
        {
            damageTxt.transform.position = new Vector3(gameObject.transform.position.x, 
                gameObject.transform.position.y + 3.5f, 0);
            damageTxt.SetActive(true);
            damageTxt.GetComponent<TextMeshPro>().text = _damage.ToString();
        }
        else
        {
            Debug.Log("dmgTxt Pool not found");
        }
    }
    #endregion

    #region HERO_SKILL
    public void UseSkill()
    { 
        //use skill
        Debug.Log("Skill Cast");
        StartCoroutine(SkillAnimation());
    }

    IEnumerator SkillAnimation()
    {
        //skill anim
        yield return new WaitForSecondsRealtime(0.5f);
        Debug.Log("Skill Used");
        HUDController.Instance.SkillUsed();
        StopCoroutine(SkillAnimation());
    }
    #endregion

    #region DIE
    private void Die() 
    {
        IsDead = true;
        HP = 0;

        if (name.Contains("Enemy"))
        {
            waveController.CheckCurrentWaveCleared();
        }

        if (name.Contains("Player"))
        {
            waveController.UnitDefeated();
            HUDController.Instance.DisplayDefeatedTroops(name);
        }
        
        if(IsHero) 
        {
            HUDController.Instance.DisplayDefeatedHero();
            waveController.UnitDefeated();
        }

        StartCoroutine(DestroyAnimPlayTime());
    }

    IEnumerator DestroyAnimPlayTime()
    {
        PlayDestroyAnim();
        yield return new WaitForSecondsRealtime(0.2f);
        Destroy(gameObject);
    }

    private void PlayDestroyAnim()
    {
        //spawn destroy anim
        GameObject explosion = ObjectPoolManager.Instance.GetPooledObjects(OBJPOOL.explosion);
        if (explosion != null)
        {
            explosion.transform.position = gameObject.transform.position;
            explosion.SetActive(true);
        }
        else
        {
            Debug.Log("Explosion Pool not found");
        }

    }
    #endregion
}

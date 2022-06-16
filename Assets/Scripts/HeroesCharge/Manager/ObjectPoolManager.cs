using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OBJPOOL
{
    explosion,
    damagetxt,
    projectile
}

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance; 
    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField]
    private List<GameObject> damagePopUpObjects = new List<GameObject>();
    [SerializeField]
    private GameObject damagePopUpPoolContainer;
    [SerializeField]
    private int maxDamagePopUpPool;
    [SerializeField]
    private GameObject damagePopUpPrefab; 
    [SerializeField]
    private List<GameObject> projectileObjects = new List<GameObject>();
    [SerializeField]
    private GameObject projectilPoolContainer;
    [SerializeField]
    private int maxProjectilePool;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private List<GameObject> explosionObjects = new List<GameObject>();
    [SerializeField]
    private GameObject explosionPoolContainer;
    [SerializeField]
    private int maxExplosionPool;
    [SerializeField]
    private GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        InitObjects(damagePopUpPrefab, OBJPOOL.damagetxt);
        InitObjects(explosionPrefab, OBJPOOL.explosion);
        //InitObjects(projectilePrefab, OBJPOOL.projectile);
    }

    public void InitObjects(GameObject _obj, OBJPOOL _objType)
    {
        int maxPool = 0;
        switch (_objType)
        {
            case OBJPOOL.projectile:
                maxPool = maxProjectilePool;
                for (var i = 0; i < maxPool; i++)
                {
                    GameObject obj = Instantiate(_obj, projectilPoolContainer.transform);
                    obj.SetActive(false);
                    projectileObjects.Add(obj);
                }
                break;
            case OBJPOOL.explosion:
                maxPool = maxExplosionPool;
                for (var i = 0; i < maxPool; i++)
                {
                    GameObject obj = Instantiate(_obj, explosionPoolContainer.transform);
                    obj.SetActive(false);
                    explosionObjects.Add(obj);
                }
                break;
            case OBJPOOL.damagetxt:
                maxPool = maxDamagePopUpPool;
                for (var i = 0; i < maxPool; i++)
                {
                    GameObject obj = Instantiate(_obj, damagePopUpPoolContainer.transform);
                    obj.SetActive(false);
                    damagePopUpObjects.Add(obj);
                }
                break;
            default: Debug.Log("Unknown Objects");
                break; 
        }
    }

    public GameObject GetPooledObjects(OBJPOOL _objType)
    {
        List<GameObject> pooledObjects = new List<GameObject>();
        switch (_objType)
        {
            case OBJPOOL.projectile:
                pooledObjects = projectileObjects;
                break;
            case OBJPOOL.explosion:
                pooledObjects = explosionObjects;
                break;
            case OBJPOOL.damagetxt:
                pooledObjects = damagePopUpObjects;
                break;
            default:
                break;
        }
        
        foreach (var obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
            else
            {
                obj.SetActive(false);
            }
        }

        return null;
    }
}

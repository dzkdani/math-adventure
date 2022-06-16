using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnBackManager : MonoBehaviour
{
    public static BtnBackManager instance;

    public Button W;

    [SerializeField]
    public struct Jendela
    {
        public GameObject current;
        public GameObject go_to;
        public Button button;
        public bool withTween;
    }

    //A list to keep track of all the scenes you've loaded so far
    List<Jendela> previousGOs = new List<Jendela>();
    public int Scene_Sekarang;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            try
            {
                instance.W = W;
            }
            catch { }
            Destroy(gameObject);
        }
        instance.Hapus_Semua();
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void Hapus_Semua()
    {
        previousGOs.Clear();
    }

    public void AddCurrentGOsToActiveGOs(BeforeAfterGameObject e)
    {
        Jendela jendela;
        jendela.current = e.current;
        jendela.withTween = e.withTween;
        jendela.go_to = e.go_to;
        jendela.button = e.button;

        previousGOs.Add(jendela);
    }

    //Call this method from your button OnClick() event system in the inspector
    public void LoadPreviousScene()
    {
        /*
        0 //landing menu // -> 1 //intro -> 2// MainMenu -> 3//GameMenu
					                                            8 //map
                                                                    9 //hero_charge
					                                            4 //MM
						                                            5 //Gameplay Training Arena
					                                            6 //QuizRoom
						                                            7 //QuizQuestion
        */
        int previousScene = 0;
        bool ignore = false;
        //Check wether you're not back at your original scene (index 0)
        switch (Scene_Sekarang)
        {
            case 0:
            case 2:
                Application.Quit();
                break;
            case 3:
                previousScene = 2;
                break;
            case 4:
            case 8:
            case 6:
                previousScene = 3;
                break;
            default: ignore = true;
                break;
        }
       if(!ignore) SceneManager.LoadScene(previousScene);
    }

    public void LoadPreviousGO()
    {
        Jendela previousGO;
       // if (previousGOs.Count > 1)
        //{
            previousGO = previousGOs[previousGOs.Count - 1]; //Get the last previously loaded scene name from the list
            previousGOs.RemoveAt(previousGOs.Count - 1); //Remove the last previously loaded scene name from the list
            if (!previousGO.withTween)
            {
                if (previousGO.button != null)
                {
                    previousGO.button.onClick.Invoke();
                }
                previousGO.current.SetActive(true);
                previousGO.go_to.SetActive(false);
            }
            else
            {
                GameMenuController a = FindObjectOfType<GameMenuController>();
                a.MoveWithTween(previousGO.current.GetComponent<RectTransform>());
            }
       /* }
        else
        {
            previousGO = previousGOs[0]; //0 will always be your first scene
            if (!previousGO.withTween)
            {
                if (previousGO.button != null)
                {
                    previousGO.button.onClick.Invoke();
                }
                previousGO.current.SetActive(true);
                previousGO.go_to.SetActive(false);
            }
            else
            {
                AllMenu a = FindObjectOfType<AllMenu>();
                a.MoveWithTween(previousGO.current.GetComponent<RectTransform>());
            }
        }**/
    }

    private void Update()
    {
        Scene_Sekarang = SceneManager.GetActiveScene().buildIndex;
        //Debug.Log("previousGOs "+instance.previousGOs.Count);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            try
            {
                W.onClick.Invoke();
            }
            catch {
                if (previousGOs.Count.Equals(0))
                {
                    AudioManager.Instance.PlaySFX(5, ()=>{ LoadPreviousScene(); });
                }
                else
                {
                    AudioManager.Instance.PlaySFX(5, () => { LoadPreviousGO(); });
                };
            }
        }
    }
}


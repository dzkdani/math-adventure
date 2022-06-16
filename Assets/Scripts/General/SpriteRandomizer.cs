using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteRandomizer : MonoBehaviour
{
    public SpriteRenderer this_SpriteRenderer;
    public Image this_image;
    public GameObject this_parent;

    public List<GameObject> gameObjects;
    public List<Sprite> sprites;

    public bool pakai_gameObject;
    public bool pakai_sprite;
    public bool pakai_image;

    int acak = 0;

   // Start is called before the first frame update
    void Start()
    {
        if (pakai_gameObject) { use_GameObject(); }
        //if(pakai_image) { use_Image(); }
        if(pakai_sprite) { use_Sprite(); }
    }

    void use_GameObject()
    {
        acak = Random.Range(0, gameObjects.Count);
        foreach (GameObject ds in gameObjects) ds.SetActive(false);
        gameObjects[acak].SetActive(true);
    }

    void use_Sprite()
    {
        acak = Random.Range(0, sprites.Count);
        this_SpriteRenderer.sprite = sprites[acak];
    }

    void use_Image()
    {
        acak = Random.Range(0, sprites.Count);
        this_image.sprite = sprites[acak];
    }
}

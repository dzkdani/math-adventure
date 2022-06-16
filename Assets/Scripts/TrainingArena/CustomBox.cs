using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DragonBones;

public class CustomBox : MonoBehaviour
{
    public int panjang;
	public bool isDragonBone, is3D;
	public float space = 0.1f;
	public GameObject template;
	public Sprite [] casing;

    //if is3D = true
    public string[] state;
    // w x h
    public Vector3 start_point;
    //space
    public float space_h;

    private void Create3D()
    {
        Vector3 temp_coor = start_point;
        float x = 0,y = 0;
        for (int i = 0; i<state.Length; i++)
        {
            temp_coor.z = start_point.z + y;
            for (int ias= 0; ias < state[i].Length; ias++)
            {
                char status = state[i][ias];
                temp_coor.x = start_point.x + x;
                if (status.Equals('1'))
                {
                    GameObject temp = Instantiate(template, temp_coor, template.transform.rotation);
                    temp.transform.SetParent(transform);
                }
                x += space;
            }
            x = 0;
            y += space_h;
            temp_coor = start_point;
        }
    }

	private void Start()
	{
        if (is3D)
        {
            return;
        }
        else if (isDragonBone)
        {
			string[] ca = { "pengurangan", "penjumlahan", "pembagian", "perkalian" };
			string c = ca[Random.Range(0, ca.Length)];
			int children = transform.childCount;

			for (int i = 0; i < children; ++i)
			{
				transform.GetChild(i).GetComponentInChildren<UnityArmatureComponent>().animation.Play(c);
			}
		}
		else
        {
			int [] ca = {0, 1, 2, 3, 4, 5};
			int c = ca[Random.Range(0, casing.Length)];
			int children = transform.childCount;

			for (int i = 0; i < children; ++i)
			{
				transform.GetChild(i).GetComponentInChildren<SpriteRenderer>().sprite = casing[c];
			}
		}
		
	}

    public void Create(){

        if (is3D)
        {
            Create3D();
            return;
        }

		int c = Random.Range(0, casing.Length);
		if (!isDragonBone){
			template.GetComponentInChildren<SpriteRenderer>().sprite = casing[c];
		}
		float x = 2f;
		float center = (x * panjang)/2;
		
		Vector3 posisi = transform.position + Vector3.zero;
		for(int i = 0; i<panjang; i++){
			GameObject temp = Instantiate(template, posisi, Quaternion.identity);
			temp.transform.SetParent(transform);
			try
			{
				if (!isDragonBone)
				{
					DestroyImmediate(temp.GetComponentInChildren<UnityArmatureComponent>().gameObject);

				}
				else DestroyImmediate(temp.GetComponentInChildren<SpriteRenderer>().gameObject);
			}
            catch { }
			/*Vector3 posisi2 = posisi + new Vector3 (0,(x-0.1f), 0);
			for(int a = 1; a<tinggi; a++){
				GameObject temp2 = Instantiate(template, posisi2, Quaternion.identity);
				temp2.transform.SetParent(temp.transform);
				posisi2 = posisi2 + new Vector3 (0,(x-0.1f),0);
			}*/

			posisi = posisi + new Vector3 ((x-space), 0,0);
		}
		
		BoxCollider BC = gameObject.AddComponent<BoxCollider>();
		gameObject.AddComponent<Rigidbody>();
		
		BC.center = new Vector3(center-1-(0.1f * center / 2), 0, -0.15f);
		BC.size = new Vector3(panjang*x+0.3f, 1*(x+0.4f), (x+0.8f));
		gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
	}
	
	public void Reset(){
		int children = transform.childCount;
        for (int i = 0; i < children; ++i){DestroyImmediate(transform.GetChild(i).gameObject);}
		DestroyImmediate(gameObject.GetComponent<BoxCollider>());
		DestroyImmediate(gameObject.GetComponent<Rigidbody>());
	}
}

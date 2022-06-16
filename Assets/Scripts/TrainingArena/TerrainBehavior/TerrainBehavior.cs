using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBehavior : MonoBehaviour
{
    public int ID;
	public string namaSet;
    //public List<AudioSource> audioSources;
	public List<GameObject> ObjectsToBeReset;
    public PenaPenggambarGrid Pena;  

    [System.Serializable]
    public class PenaPenggambarGrid
    {
        public GameObject Tint;
        public Vector3 TitikAwal;
        public int panjang_x;
        public int panjang_y;
        public float y_temp, x_temp;
        void DrawLine(Vector3 start, Vector3 end, Transform transform)
        {
            GameObject myLine = Instantiate(Tint, Vector3.zero, Quaternion.identity);
            myLine.transform.parent = transform;
            start += transform.position;
            end += transform.position;
            LineRenderer lr = myLine.GetComponent<LineRenderer>();
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
        }

        public void DrawGrid(Transform transform)
        {
            Vector3 temp = TitikAwal - (Vector3.right * 0.20f);
            temp += Vector3.left;
            for (int i = 0; i < y_temp; i++)
            {
                DrawLine(temp, temp + Vector3.forward * panjang_x, transform);
                temp += (Vector3.right * 2.5f);
            }

            temp = TitikAwal - (Vector3.right * 0.20f); temp += Vector3.left;
            for (int i = 0; i < x_temp; i++)
            {
                DrawLine(temp, temp + Vector3.right * panjang_y, transform);
                temp += (Vector3.forward * 2.5f);
            }
        }
    }

	public virtual void Mulai() {}
    public virtual void Pembaruan() { }
    public virtual void Sadar() { }

    private void Update()
    {
        Pembaruan();
    }

    /*private void OnBecameInvisible()
    {
        if(audioSources != null)audioSources.ForEach(x => { x.enabled = false; });
    }

    private void OnBecameVisible()
    {
        if (audioSources != null) audioSources.ForEach(x => { x.enabled = true; });
    }*/

    public void ResetTerrain()
    {
        ObjectsToBeReset.ForEach(x => { x.SetActive(true); });
    }

    private void Awake()
    {
        Sadar();
    }
    void Start()
    {
        if (TrainingArenaSettingManager.Instance.isKOTAKON) Pena.DrawGrid(transform);
        Mulai();
    }
}

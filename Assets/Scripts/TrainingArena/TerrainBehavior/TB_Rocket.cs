using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TB_Rocket : TerrainBehavior
{
	public float max_timer = 1.5f;

	[Serializable]
	public class Arena_TB
	{
		public GameObject go;
		public float jarak;
	}
	public Arena_TB[] Rocket_Prefab;

	public bool isRocketSpawner;
	public List<string> Rocket_Sets = new List<string>();

	public GameObject Spawnpoint;
	public GameObject Checkpoint;

	int variasi;
	string set;
	public float timer = 0;

	public string GetRocketSequence()
	{
		return set;
	}

	// Update is called once per frame
	void rocket_v()
	{
		StartCoroutine(RocketNormal());
	}

	int ReturnUniqueRandom(int min, int max, ref int t)
	{
		int baris = 0;
		while (true)
		{
			baris = UnityEngine.Random.Range(min, max);
			if (baris != t) break;
		}
		t = baris;
		return baris;
	}

	int prev_id_baris = 0;
	IEnumerator RocketNormal()
	{
		Vector3 posisi = new Vector3(Spawnpoint.transform.position.x, 0, Spawnpoint.transform.position.z);

		GameObject[] prefab = new GameObject[5];
		int[] baris_ke = new int[11];

		for (int i = 0; i < 11; i++)
		{
			int baris = ReturnUniqueRandom(0, 5, ref prev_id_baris);
			baris_ke[i] = baris;
		}

		for (int i = 0; i < 5; i++)
		{
			prefab[i] = Instantiate(Rocket_Prefab[i].go, posisi, Quaternion.identity);
			for (int ic = 0; ic < prefab[i].transform.childCount; ic++)
			{
				Transform temp = prefab[i].transform.GetChild(ic);
				if (baris_ke[ic] == i)
				{
					if (set[ic] == '1') temp.gameObject.SetActive(true);
					else temp.gameObject.SetActive(false);
				}
				else
				{
					temp.gameObject.SetActive(false);
				}
			}
			yield return new WaitForSeconds(0.2f);
		}
	}

	public void StartCount()
    {
		timer = 0;
		isRocketSpawner = true;
    }

	public override void Sadar()
	{
		//ID = UnityEngine.Random.Range(0, Rocket_Prefab.Length);
	}
	public override void Mulai()
	{
		try
		{
			variasi = UnityEngine.Random.Range(0, Rocket_Sets.Count);
			set = Rocket_Sets[variasi];
		}
		catch { }
	}

	public override void Pembaruan()
	{
		if (isRocketSpawner)
		{
			if(timer>=max_timer)
			{
				isRocketSpawner = false;
				rocket_v();
				timer = 0;
			}
			timer += Time.deltaTime;
		}
	}
}

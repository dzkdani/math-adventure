using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TB_Gada : TerrainBehavior
{
	public GameObject Gada;
	float Xstart = -4.5f, Xend = 4.5f;

	public List<Transform> Line;

	void ConjureMacesInLine()
	{
		bool isRight = false;
		bool isLeft = false;

		int a = Random.Range(1, 11);
		switch (a)
		{
			case 1: case 2: case 3: case 4: case 5:
				isLeft = true;
				break;

			case 6: case 7: case 8: case 9: case 10:
				isRight = true;
				break;
		}

		for (int i = 0; i < Line.Capacity; i++)
		{
			int totalGada = Random.Range(4, 6);
			SummonMace(Line[i], totalGada, isLeft, isRight);
			if (isLeft) { isLeft = false; isRight = true; }
			else if (isRight) { isLeft = true; isRight = false; }
		}
	}

	void SummonMace(Transform parent, int totalGada, bool isLeft, bool isRight)
	{
		float Xpoint = Xstart;
		float jarak = 9f / totalGada;
		float speed = Random.Range(5, 7);
		for (int i = 0; i<totalGada; i++)
        {
			if (totalGada.Equals(5) && i.Equals(0)) { }
			else {
				GameObject prefab = Instantiate(Gada, transform);
				// audioSources.Add(prefab.GetComponent<AudioSource>());
				prefab.name = "Gada " + i;
				prefab.transform.parent = parent;
				prefab.transform.localPosition = new Vector3(Xpoint, 0.7f, 0);
				MoveEnemy E = prefab.GetComponent<MoveEnemy>();
				E.speed = speed;
				E.kanan.z = E.kiri.z = 0;
				E.kanan.x = Xend;
				E.kiri.x = Xstart;
				E.right = isRight;
				E.left = isLeft;
				Xpoint += jarak;
			}
		}
	}

	public override void Mulai()
	{
		ConjureMacesInLine();
	}
}

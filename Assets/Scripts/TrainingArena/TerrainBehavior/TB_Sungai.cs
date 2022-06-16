using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TB_Sungai : TerrainBehavior
{
    public Transform[] PointJump;
	public List<MoveEnemy> papanKayu;

	void AturArahPapan()
	{
		for (int i = 0; i < papanKayu.Capacity; i++)
		{
			int temp = i + 1;
			int a = UnityEngine.Random.Range(1, 11);

			if (temp % 2 == 0)
			{
				int temp2 = i - 1;
				int b = UnityEngine.Random.Range(1, 11);
				if (a % 2 == 0) papanKayu[i].left = papanKayu[temp2].right = true;
				else papanKayu[i].right = papanKayu[temp2].left = true;

				if (b % 2 == 0) { papanKayu[i].speed = UnityEngine.Random.Range(6, 7); papanKayu[temp2].speed = UnityEngine.Random.Range(4, 6); }
				else { papanKayu[i].speed = UnityEngine.Random.Range(4, 6); papanKayu[temp2].speed = UnityEngine.Random.Range(6, 7); }
			}
			else if(papanKayu.Capacity == 1)
			{
				papanKayu[i].speed = UnityEngine.Random.Range(4, 7);
				if (a % 2 == 0) papanKayu[i].left = true;
				else papanKayu[i].right = true;
			}
		}
	}

	// Start is called before the first frame update
	public override void Mulai()
    {
		AturArahPapan();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

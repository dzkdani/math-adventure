using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpSetting : MonoBehaviour
{
	public int markID;
	public GameObject [] placeHolder;

	private void Start()
	{
		Change_Quiz_Icon();
	}

	void Change_Quiz_Icon()
	{
		if (markID < 4 && markID >= 1)
		{
			//shield
			placeHolder[0].SetActive(true);
		}
		else
		{
			//time - stamina
			placeHolder[1].SetActive(true);
		}
	}
}

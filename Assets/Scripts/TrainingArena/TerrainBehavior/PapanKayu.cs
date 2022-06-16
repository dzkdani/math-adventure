using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapanKayu : MonoBehaviour
{
	public UnityEngine.Transform Kayu;
	public AudioSource audioSource;
	public bool water = false;
	public float speed;
	float AudioLength;

	// Start is called before the first frame update
	void Start()
	{
		AudioLength = audioSource.clip.length;
		audioSource.volume = 0;
	}

	private void Update()
	{

	}

	private void OnCollisionStay(Collision coll)
	{
		if (!water)
		{
			if (coll.gameObject.tag == "Player")
			{
				speed = Kayu.GetComponent<MoveEnemy>().speed;

				if (Kayu.GetComponent<MoveEnemy>().left && coll.gameObject.GetComponent<TrainingArena_PlayerController>().CheckWall() != "left") {speed *= -1;}
			}
		}
	}

	private void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject.tag == "Player")
		{
			if (water) audioSource.Stop();

			if (!audioSource.isPlaying)
			{
				audioSource.volume = 1;
				audioSource.Play();
			}
		}
	}

}

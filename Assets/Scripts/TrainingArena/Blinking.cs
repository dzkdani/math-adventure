using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Blinking : MonoBehaviour
{
	Sequence blink;
	
	void Start(){
		blink = DOTween.Sequence();
		blink.Append(GetComponent<SpriteRenderer>().DOFade(0, 0.6f));
		GetComponent<SpriteRenderer>().color = Color.white;
		blink.SetLoops(-1, LoopType.Restart).SetSpeedBased();
	}
}

using UnityEngine;
using System.Collections;

public class OnlyHighscore : MonoBehaviour {

	int second = 0, psecond = 0;

	void Update () {
		if (GameController.mode != 4 || GameController.Highscore == false )
			renderer.enabled = false;
		else {
			second = (int)Time.time;
			if ( psecond < second ) {
				psecond = second;
				renderer.enabled = !renderer.enabled;
			}
		}
	}
}

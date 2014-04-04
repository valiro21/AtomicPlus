using UnityEngine;
using System.Collections;

public class OnlyInGame : MonoBehaviour {

	int second = 0, psecond = 0;

	void Update () {
		if ( GameController.mode == 0 )
			GetComponent<TextMesh>().text = "CLICK IN THE MIDDLE TO CONTINUE";
		else
			GetComponent<TextMesh>().text = "CLICK IN THE MIDDLE TO SELECT";

		if (GameController.mode == 3)
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

using UnityEngine;
using System.Collections;

public class Reset : MonoBehaviour {

	void OnMouseDown () {
		if (GameController.mode == 0)
			PlayerPrefs.SetInt ("Highscore", 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (GameController.mode != 0)
			GetComponent<GUITexture>().enabled = false;
		else
			GetComponent<GUITexture>().enabled = true;
	}
}

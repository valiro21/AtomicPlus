using UnityEngine;
using System.Collections;

public class Close : MonoBehaviour {

	void OnMouseDown () {
		if (GameController.mode != 0)
			GameController.GameOver (0);
		else
			System.Diagnostics.Process.GetCurrentProcess().Kill();
	}
}

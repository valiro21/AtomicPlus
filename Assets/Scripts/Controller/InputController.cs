using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	static float KeyboardInput () {
		if (Input.GetKey (KeyCode.LeftArrow))
			return 1f;
		else if ( Input.GetKey (KeyCode.RightArrow) )
			return -1f;
		return 0f;
	}

	static float TouchInput () {
		return 0f;
	}

	public static float GetInput () {
		return KeyboardInput () + TouchInput ();
	}
}

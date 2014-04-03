using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	static bool KeyboardInput () {
		if (Input.GetKey (KeyCode.Space))
			return true;
		return false;
	}

	static bool TouchInput () {
		return false;
	}

	public static float GetInput () {
		return (KeyboardInput () | TouchInput ()) ? 1f : -1f;
	}
}

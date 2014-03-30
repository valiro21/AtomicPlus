using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public static long damage = 0;
	public static GameObject Player;

	public static float GetRadiusOfRing ( long Ring ) {
		return MovementController.InitialRadius + Ring * MovementController.InterpolationRadius;
	}

	public static void Kill ( GameObject Player ) {
		GameController.GameOver ();
	}

	public static void DamagePlayer ( ) {
		damage++;
		if (damage == 10)
			Kill ( Player );
	}

	void Awake () {
		Player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
	}
}

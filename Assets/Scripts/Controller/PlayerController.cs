using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public static long damage = 0;
	public static GameObject Player;

	public static float GetRadiusOfRing ( long Ring ) {
		return MovementController.InitialRadius + Ring * MovementController.InterpolationRadius;
	}

	public static void Revive ( ) {
		Player.renderer.enabled = true;
		Player.GetComponent<PlayerMovement> ().Revive ();
	}

	public static void Kill ( ) {
		Player.renderer.enabled = false;
		Player.GetComponent<PlayerMovement> ().Dead = true;
		GameController.GameOver ();
	}

	public static void DamagePlayer ( ) {
		damage++;
		//DrawController.DrawDamage ();
		if (damage == 10)
			Kill ( );
	}

	void Awake () {
		Player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
	}
}

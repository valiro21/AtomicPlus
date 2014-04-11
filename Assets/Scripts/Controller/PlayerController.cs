using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public static long damage = 0;
	public static GameObject Player;
	public static float Angle;
	public static bool Draw = false;

	static float V3Distance ( Vector3 x, Vector3 y ) {
		x.z = 0f; y.z = 0f;
		return Vector3.Distance (x, y);
	}

	public static void AddToAngle (int i, float R) {
		float Off = 1f, dirrection = Player.GetComponent<PlayerMovement>().dirrection;
		float r = PlayerCharacterRadius() * 2 + 0.03f;
		float dx = V3Distance (GameController.Ring, Player.transform.position);
		if (r + R > dx) {
			float cos =  (R*R + dx*dx - r * r)/(2 * R * dx);
			Angle k = new Angle ( Mathf.Acos (cos) , 0);
			Angle k1 = Angle - k + dirrection * Off, k2 = Angle + k + dirrection * Off;

			DrawController.AddToAngles (ref k1, i);
			DrawController.AddToAngles (ref k2, i);
		}
	}

	public static bool IsDead () {
		return Player.GetComponent<PlayerMovement> ().Dead;
	}

	public static float GetRadiusOfRing ( long Ring ) {
		return MovementController.InitialRadius + Ring * MovementController.InterpolationRadius;
	}

	public static void Revive ( ) {
		Player.renderer.enabled = true;
		damage = 0;
		Player.GetComponent<PlayerMovement> ().Revive ();
	}

	public static void Kill ( ) {
		Player.renderer.enabled = false;
		Player.transform.position = new Vector3 (0, 0, 0);
		Player.GetComponent<PlayerMovement> ().Dead = true;
	}

	public static float PlayerCharacterRadius () {
		return Player.transform.localScale.x / 2f;
	}

	/*public static float Contains ( Vector3 x ) {
		if ( Vector3.Distance )
	}*/

	public static void DamagePlayer ( ) {
		damage++;
		//DrawController.DrawDamage ();
		if (damage == 10)
			GameController.GameOver ();
	}

	public static void MovePlayer () {
		Player.GetComponent<PlayerMovement> ().Move ();
	}

	public static void HealPlayer ( ) {
		damage--;
		//DrawController.DrawDamage ();
		if (damage < 0)
			damage = 0;
	}

	public static float GetRadius () {
		return Player.GetComponent<PlayerMovement>().Radius;
	}

	void Awake () {
		Player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if ( GameController.mode == 3 )
			Angle = Player.GetComponent<PlayerMovement> ().DegreeAngle;
		if (damage < 0)
			damage = 0;
		else if (damage > 10)
			damage = 10;
	}
}

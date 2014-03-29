using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public static long damage = 0;

	public static void Kill ( GameObject Player ) {
		GameController.GameOver ();
	}

	public static void DamagePlayer ( ) {
		damage++;
		if (damage == 10)
			Kill ( GameObject.FindGameObjectWithTag ("Player") );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

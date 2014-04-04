using UnityEngine;
using System.Collections;

public class DamageController : MonoBehaviour {

	public static GameObject DmgBar;
	static int damage = 0;
	bool enabled = true;

	public static void UpdateDamage () {
		for (int i = 1; i <= 10; i++)
			DmgBar.transform.FindChild ( "Damage" + i.ToString () ).renderer.enabled = false;

		if ( damage > 0 && damage <= 10 )
			DmgBar.transform.FindChild ( "Damage" + damage.ToString () ).renderer.enabled = true;
	}

	void Awake () {
		DmgBar = GameObject.Find ("DmgBar");
	}

	void Update () {
		if (GameController.mode == 3) {
			if ( enabled == false ) {
				enabled = true;
				foreach ( Transform i in transform )
					i.renderer.enabled = true;
				damage = 0;
				UpdateDamage ();
			}

			if (damage != (int)PlayerController.damage) {
				damage = (int)PlayerController.damage;
				UpdateDamage ();
			}
		}
		else {
			if ( enabled == true ) {
				enabled = false;
				foreach ( Transform i in transform )
					i.renderer.enabled = false;
			}
		}
	}
}

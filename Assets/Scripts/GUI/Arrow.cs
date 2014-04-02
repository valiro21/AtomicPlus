using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public float dirrection = 0;

	void OnMouseDown () {
		if ( dirrection > 0 )
			Menu.RightI ();
		else
			Menu.LeftI ();
	}

	void Update () {
		if (GameController.mode == 3)
			transform.renderer.enabled = false;
		else
			transform.renderer.enabled = true;
		if (dirrection > 0)
			gameObject.transform.position = MovementController.ChangeToAngle ( MovementController.FinalRadius, 180f );
		else
			gameObject.transform.position = MovementController.ChangeToAngle ( MovementController.FinalRadius, 0f );
	}
}

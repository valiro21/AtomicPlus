using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public float dirrection = 0;
	bool ienable = false;

	void OnMouseDown () {
		if ( dirrection > 0 )
			Menu.RightI ();
		else
			Menu.LeftI ();
	}

	void Update () {
		if (GameController.mode == 3 || GameController.mode == 0) {
			foreach ( Transform i in transform )
				i.renderer.enabled = false;
			ienable = false;
		}
		else {
			foreach ( Transform i in transform )
				i.renderer.enabled = true;
			ienable = true;
		}

		if ( gameObject.name == "LeftArrow" || gameObject.name == "RightArrow" ) {
			if ( GameController.mode != 3 &&  GameController.mode != 0 )
				if (dirrection > 0)
					gameObject.transform.position = Angle.PointByRadius ( 180f, MovementController.FinalRadius + 0.1f);
				else
					gameObject.transform.position = Angle.PointByRadius ( 0f, MovementController.FinalRadius + 0.1f);
			else
				gameObject.transform.position = new Vector3 ( 0, 0, 0 );

			if ( ienable )
				if ( dirrection > 0 && Input.GetKey ( KeyCode.RightArrow ) )
					Menu.RightI ();
				else if ( dirrection < 0 && Input.GetKey ( KeyCode.LeftArrow ) )
					Menu.LeftI ();
		}
	}
}

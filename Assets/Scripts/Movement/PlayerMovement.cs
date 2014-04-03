using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {

	public float dirrection = 1f, RadiusLerpSpeed, DegreesPerSecond, InitialRadius, InterpolationRadius, FinalRadius;
	float DegreeAngle = 0f, NewRadius, ChangeRingDirrection = 0f, Radius;
	List<GameObject> SurroundingCircle;
	public bool Dead = true;

	public void Revive () {
		DegreeAngle = 0f;
		Radius = NewRadius = InitialRadius;
		Dead = false;
	}

	void OnCollisionEnter ( Collision collision ) {
		if (GameController.mode == 3) {
			if (collision.collider.tag == "Point") {
				dirrection *= -1f;
				GameController.GetPoint ();
				SpawnController.SpawnPoint ();
			} else if (collision.collider.tag == "Bullet") {
				PlayerController.DamagePlayer ();
			}
		}
	}


	void Start () {
		InitialRadius = MovementController.InitialRadius;
		InterpolationRadius = MovementController.InterpolationRadius;
		FinalRadius = MovementController.FinalRadius;

		gameObject.renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if ( !Dead ) {
			DegreeAngle = MovementController.LerpAngle ( DegreeAngle, dirrection * DegreesPerSecond );

			transform.position = MovementController.ChangeToAngle (Radius, DegreeAngle);

			//get radius changes
			ChangeRingDirrection = InputController.GetInput ();

			Radius += ChangeRingDirrection * RadiusLerpSpeed * Time.deltaTime;
			if ( Radius < InitialRadius )
				Radius = InitialRadius;
			else if ( Radius > FinalRadius )
				Radius = FinalRadius;

			//draw surrounding circle
			//DrawController.DrawArc ( ref SurroundingCircle, DegreeAngle, DegreeAngle - 5f, Radius, 0.05f, 0.05f, Resources.Load ( "Materials/WhiteGUI" ) as Material );
		}
	}
}

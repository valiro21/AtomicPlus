using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float dirrection = 1, RadiusLerpSpeed, DegreesPerSecond, InitialRadius, InterpolationRadius, FinalRadius;
	float DegreeAngle = 0f, NewRadius, ChangeRingDirrection = 0f, Radius;

	public void Reset () {
		DegreeAngle = 0f;
		Radius = NewRadius = InitialRadius;
	}

	void OnCollisionEnter ( Collision collision ) {
		if (collision.collider.tag == "Point") {
			dirrection *= -1f;
			GameController.GetPoint ();
		}
		else if ( collision.collider.tag == "Bullet" ) {
			PlayerController.DamagePlayer ( );
		}
	}





	void Start () {
		InitialRadius = MovementController.InitialRadius;
		InterpolationRadius = MovementController.InterpolationRadius;
		FinalRadius = MovementController.FinalRadius;

		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		DegreeAngle = MovementController.LerpAngle ( DegreeAngle, DegreesPerSecond );

		transform.position = MovementController.ChangeToAngle (Radius, DegreeAngle);

		//get radius changes
		ChangeRingDirrection = InputController.GetInput ();
		if (InitialRadius <= Radius + ChangeRingDirrection * InterpolationRadius && Radius + ChangeRingDirrection * InterpolationRadius <= FinalRadius && MovementController.GetRing (Radius) > 0 )
			NewRadius = Radius + ChangeRingDirrection * InterpolationRadius;

		Radius = MovementController.RadiusLerp (Radius, NewRadius, RadiusLerpSpeed);
	}
}

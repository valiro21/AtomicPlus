using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float dirrection = 1, RadiusLerpSpeed, InitialRadius, InterpolationRadius, DegreesPerSecond;
	float DegreeAngle = 0f, RadianAngle = 0f, FinalRadius, NewRadius, ChangeRingDirrection = 0f, Radius;

	public void Reset () {
		DegreeAngle = RadianAngle = 0f;
		Radius = InitialRadius;
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

	void LerpAngle () {
		DegreeAngle += DegreesPerSecond * Time.deltaTime;
		if (DegreeAngle > 360f)
			DegreeAngle -= 360f;
		else if ( DegreeAngle < 0f )
			DegreeAngle += 360f;
	}

	public long GetRing () {
		if ( (long)( ( Radius - InitialRadius) / InterpolationRadius) == (Radius - InitialRadius) / InterpolationRadius )
			return (long)((Radius - InitialRadius) / InterpolationRadius) + 1;
		return 0;
	}

	void Awake () {
		FinalRadius = InitialRadius + 3 * InterpolationRadius;
		Radius = NewRadius = InitialRadius;
	}
	
	// Update is called once per frame
	void Update () {
		LerpAngle ();
		RadianAngle = DegreeAngle * Mathf.PI / 180f;

		transform.position = new Vector3 (GameController.center.x - Radius * Mathf.Cos (RadianAngle), GameController.center.y + Radius * Mathf.Sin (RadianAngle), transform.position.z);

		//get radius changes
		ChangeRingDirrection = InputController.GetInput ();
		if (InitialRadius <= Radius + ChangeRingDirrection * InterpolationRadius && Radius + ChangeRingDirrection * InterpolationRadius <= FinalRadius && GetRing () > 0 )
			NewRadius = Radius + ChangeRingDirrection * InterpolationRadius;
		if ( NewRadius - Radius > 0.1f )
			Radius = Mathf.Lerp ( Radius, NewRadius, RadiusLerpSpeed * Time.deltaTime );
		else
			Radius = NewRadius;
	}
}

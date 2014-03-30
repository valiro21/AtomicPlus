using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	public float StaraightSpeed = 1.0f;
	public float PulsateSpeed = 0.7f, PulsateWaitTime = 0.1f, PulsateInterpolationRadius = 4f, PulsateOffset = 0.5f;
	public float DrunkSpeed = 0.5f, DrunkDegreesPerSecond = 10f, DrunkMovingAngle = 10f;
	public float SpiralSpeed = 0.1f, SpiralDegreesPerSecond = 70f;
	float EndOfRadius, DrunkOffsetAngle = 0f, DegreeAngle = 0f;
	public long mode;

	float Radius, NewRadius, WaitFrame = 0f;


	void Pulsate () {
		if (Radius == NewRadius) {
			WaitFrame+= Time.deltaTime;
		}

		if ( WaitFrame > PulsateWaitTime ) {
			NewRadius = Radius + PulsateInterpolationRadius;
			WaitFrame = 0;
		}
		Radius = MovementController.RadiusLerp ( Radius, NewRadius, PulsateSpeed, PulsateOffset  );
		transform.position = MovementController.ChangeToAngle ( Radius, DegreeAngle );

	}

	void Spiral () {
		DegreeAngle = MovementController.LerpAngle ( DegreeAngle, SpiralDegreesPerSecond );


		Radius = MovementController.RadiusLerp ( Radius, EndOfRadius, SpiralSpeed  );
		transform.position = MovementController.ChangeToAngle ( Radius, DegreeAngle );
	}

	void Drunk () {
		DrunkOffsetAngle = MovementController.LerpAngle ( DrunkOffsetAngle, DrunkDegreesPerSecond );

		Radius = MovementController.RadiusLerp ( Radius, EndOfRadius, DrunkSpeed  );
		transform.position = MovementController.ChangeToAngle ( Radius, DegreeAngle + DrunkMovingAngle * Mathf.Sin ( DrunkOffsetAngle ) );
	}

	void Straight () {
		Radius = MovementController.RadiusLerp ( Radius, EndOfRadius, StaraightSpeed  );
		transform.position = MovementController.ChangeToAngle ( Radius, DegreeAngle );
	}

	public void Reset ( float radius,  float degree, long fMode ) {
		EndOfRadius = SpawnController.OutOfBounds;
		mode = fMode;
		DegreeAngle = degree;
		Radius = radius;
		DrunkOffsetAngle = 0f;
		NewRadius = Radius;
	}
	
	// Update is called once per frame
	void Update () {
		switch (mode) {
		case 0: Straight ();
			break;
		case 1: Pulsate ();
			break;
		case 2: Drunk ();
			break;
		case 3: Spiral ();
			break;
		}

		if (Radius >= EndOfRadius)
			gameObject.active = false;
	}
}

﻿using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	public float StaraightSpeed = 1.0f;
	public float PulsateSpeed = 0.7f, PulsateSlowDownRadius = 1.5f, PulsateSlowDownSpeed = 0.4f, PulsateInterpolationRadius = 4f;
	public float DrunkSpeed = 0.5f, DrunkDegreesPerSecond = 10;
	public float SpiralSpeed = 0.1f;

	public Angle DrunkMovingAngle = 10f, SpiralDegreesPerSecond = 70f;

	Angle DrunkOffsetAngle = 0f, DegreeAngle = 0f, DegreeAngleTmp;
	float EndOfRadius, DrunksLocalMovingRatio = 0f;
	long i = 0;
	public long mode;
	static Vector3 MistakeVector = new Vector3 ( 0, 0, 0f );

	float Radius, NewRadius;

	void OnCollisionEnter ( Collision collision ) {
		if ( collision.collider.tag == "Point" )
			renderer.enabled = false;
		else if ( collision.collider.tag == "Player" )
			gameObject.active = false;
	}

	void OnCollisionExit ( Collision collision ) {
		if ( collision.collider.tag == "Point" )
			renderer.enabled = true;
	}


	void Pulsate () {
		if (Radius == NewRadius) {
			if ( i % 2 == 0 )
				NewRadius += PulsateInterpolationRadius;
			else
				NewRadius += PulsateSlowDownRadius;
			i++;
		}

		if ( i % 2 == 0 )
			Radius = MovementController.ConstantLerp ( Radius, NewRadius, PulsateSlowDownSpeed );
		else
			Radius = MovementController.ConstantLerp ( Radius, NewRadius, PulsateSpeed  );
		transform.position = DegreeAngle.PointByRadius ( Radius );
		transform.position += MistakeVector;

	}

	void Spiral () {
		DegreeAngle += Time.deltaTime * SpiralDegreesPerSecond;


		Radius = MovementController.ConstantLerp ( Radius, EndOfRadius, SpiralSpeed  );
		transform.position = DegreeAngle.PointByRadius ( Radius );
		transform.position += MistakeVector;
	}

	void Drunk () {
		DrunkOffsetAngle += Time.deltaTime * DrunkDegreesPerSecond;

		Radius = MovementController.ConstantLerp ( Radius, EndOfRadius, DrunkSpeed  );
		DegreeAngleTmp = DegreeAngle + DrunksLocalMovingRatio / Radius * DrunkOffsetAngle.Sin ();
		transform.position = DegreeAngleTmp.PointByRadius ( Radius );
		transform.position += MistakeVector;
	}

	void Straight () {
		Radius = MovementController.ConstantLerp ( Radius, EndOfRadius, StaraightSpeed  );
		transform.position = DegreeAngle.PointByRadius ( Radius );
		transform.position += MistakeVector;
	}

	public void Reset ( float radius,  Angle degree, long Mode ) {
		EndOfRadius = SpawnController.OutOfBounds;
		mode = Mode;
		DegreeAngle = degree;
		Radius = radius;
		DrunkOffsetAngle = 0f;
		NewRadius = Radius;
		DrunksLocalMovingRatio = DrunkMovingAngle * Radius;
		i = 0;
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

		if (GameController.mode != 3)
			gameObject.active = false;
	}
}

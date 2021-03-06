﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {
	public Angle DegreeAngle = 0f;
	public float dirrection = 1f, RadiusLerpSpeed, DegreesPerSecond, InitialRadius, InterpolationRadius, FinalRadius, Radius;
	float NewRadius, ChangeRingDirrection = 0f;
	List<GameObject> SurroundingCircle;
	public bool Dead = true;
	static int x = -1;

	public void Revive () {
		DegreeAngle = 0f;
		Radius = NewRadius = InitialRadius;
		Dead = false;
		x = -1;
	}



	void OnCollisionEnter ( Collision collision ) {
		if (GameController.mode == 3) {
			if (collision.collider.tag == "Point") {
				AudioController.GetPoint ();
				dirrection *= -1f;
				PlayerController.HealPlayer ();
				GameController.GetPoint ();
				SpawnController.SpawnPoint ();
			} else if (collision.collider.tag == "Bullet") {
				AudioController.Damage ();
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
	public void Move () {
		if ( !Dead && GameController.mode == 3 ) {
			DegreeAngle += dirrection * DegreesPerSecond * Time.deltaTime;

			transform.position = DegreeAngle.PointByRadius (Radius);

			//get radius changes
			ChangeRingDirrection = InputController.GetInput ();

			Radius += ChangeRingDirrection * RadiusLerpSpeed * Time.deltaTime;
			if ( Radius < InitialRadius )
				Radius = InitialRadius;
			else if ( Radius > FinalRadius )
				Radius = FinalRadius;

			//draw surrounding circle
		}
		else {
			if ( SurroundingCircle != null ) {
				foreach ( GameObject i  in SurroundingCircle )
					i.GetComponent<LineRenderer>().renderer.enabled = true;
				SurroundingCircle = null;
			}
		}
	}
}

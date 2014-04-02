using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	public static float InitialRadius, InterpolationRadius, FinalRadius;

	public static bool IsInCollider ( Vector3 position ) {
		return GameController.mode == 3 && (PlayerController.Player.collider.bounds.Contains ( position ) | SpawnController.Point.collider.bounds.Contains ( position ) );
	}

	public static long GetRing ( float Radius ) {
		long x = (long)(Radius * 10000f);
		long diff;
		for ( int i = 0; i < 4; i++ ) {
			diff = (long)((InitialRadius + i * InterpolationRadius) * 10000f);
			if ( Mathf.Abs (x - diff) < 2 )
				return i + 1;
		}
		return 0;
	}

	public static float MinimumRadius ( float Radius ) {
		float x = InitialRadius;
		while ( x < Radius )
			x += InterpolationRadius;
		x -= InterpolationRadius;
		return x >= InitialRadius ? x : InitialRadius;
	}

	public static float MaximumRadius ( float Radius ) {
		float x = FinalRadius;
		while ( x > Radius )
			x -= InterpolationRadius;
		x += InterpolationRadius;
		return x <= FinalRadius ? x : FinalRadius;
	}
	
	public static Vector3 ChangeToAngle (float Radius, float DegreeAngle ) {
		float RadianAngle = DegreeAngle * Mathf.PI / 180f;
		return new Vector3 (GameController.Ring.x - Radius * Mathf.Cos (RadianAngle), GameController.Ring.y + Radius * Mathf.Sin (RadianAngle), GameController.Ring.z);
	}

	public static float RepairAngle ( float DegreeAngle ) {
		long ratio = (long)(DegreeAngle / 360f);
		DegreeAngle -= ratio * 360f;

		if (DegreeAngle < 0f)
			DegreeAngle += 360f;
		if (DegreeAngle > 360f)
			DegreeAngle -= 360f;

		return DegreeAngle;
	}

	public static float RadiusLerp ( float Radius, float NewRadius, float Speed ) {
		float rez;
		if ( Radius <= NewRadius ) {
			rez = Radius + Radius * Speed * Time.deltaTime;
			if ( rez > NewRadius )
				rez = NewRadius;
		}
		else {
			rez = Radius - Radius * Speed * Time.deltaTime;
			if ( rez < NewRadius )
				rez = NewRadius;
		}
		return rez;
	}

	public static float ConstantLerp ( float Radius, float NewRadius, float SpeedPerSecond ) {
		float rez = Radius + SpeedPerSecond * Time.deltaTime;
		if (rez > NewRadius)
			rez = NewRadius;
		return rez;
	}

	public static float RadiusLerp ( float Radius, float NewRadius, float Speed, float Offset ) {
		if ( Mathf.Abs (NewRadius - Radius) > Offset )
			return Mathf.Lerp ( Radius, NewRadius, Speed * Time.deltaTime );
		return NewRadius;
	}

	public static float HalfPoint ( float first, float second ) {
		if ( first > second )
			second += 360f;

		return RepairAngle( (first + second ) / 2f );
	}

	public static float LerpAngle (float DegreeAngle, float DegreesPerSecond) {
		DegreeAngle += DegreesPerSecond * Time.deltaTime;
		if (DegreeAngle > 360f)
			DegreeAngle -= 360f;
		else if ( DegreeAngle < 0f )
			DegreeAngle += 360f;

		if (Mathf.Abs (DegreeAngle - 360f) < 1f)
			DegreeAngle = 0f;

		return DegreeAngle;
	}
	
	// Update is called once per frame
	void Awake () {
		MovementValues Values = GameController.ValuesGod.GetComponent<MovementValues> ();
		InitialRadius = Values.InitialRadius;
		InterpolationRadius = Values.InterpolationRadius;
		FinalRadius = Values.FinalRadius;
	}
}

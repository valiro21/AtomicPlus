using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	public static float InitialRadius, InterpolationRadius, FinalRadius;

	public static long GetRing ( float Radius ) {
		if ( (long)( ( Radius - InitialRadius) / InterpolationRadius) == (Radius - InitialRadius) / InterpolationRadius )
			return (long)((Radius - InitialRadius) / InterpolationRadius) + 1;
		return 0;
	}

	public static Vector3 ChangeToAngle (float Radius, float DegreeAngle ) {
		float RadianAngle = DegreeAngle * Mathf.PI / 180f;
		return new Vector3 (GameController.Ring.x - Radius * Mathf.Cos (RadianAngle), GameController.Ring.y + Radius * Mathf.Sin (RadianAngle), GameController.Ring.z);
	}

	public static float RepairAngle ( float DegreeAngle ) {
		long ratio = (long)(DegreeAngle / 360f);
		DegreeAngle -= (float)ratio * 360f;

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

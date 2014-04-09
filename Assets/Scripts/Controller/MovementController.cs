using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	public static float InitialRadius, InterpolationRadius, FinalRadius;

	public static bool IsInCollider ( Vector3 position ) {
		return GameController.mode == 3 && (PlayerController.Player.collider.bounds.Contains ( position ) | SpawnController.Point.collider.bounds.Contains ( position ) );
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

	/*public static float HalfPoint ( float first, float second ) {
		if ( first > second )
			second += 360f;

		return RepairAngle( (first + second ) / 2f );
	}*/
	
	// Update is called once per frame
	void Awake () {
		MovementValues Values = GameController.ValuesGod.GetComponent<MovementValues> ();
		InitialRadius = Values.InitialRadius;
		InterpolationRadius = Values.InterpolationRadius;
		FinalRadius = Values.FinalRadius;
	}
}

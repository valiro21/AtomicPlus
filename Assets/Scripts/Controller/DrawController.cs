using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawController : MonoBehaviour {

	public static GameObject[][][] Lines;
	public static long last = 0, NumberOfArcs = 12;
	public static float RenderAngle = 1f, MultiArcsWidth = 0.05f, MultiArcsHeigth =0.05f, RotationDegreeSpeed = 20f, Offset = 0f;

	public static void DrawMultiLine ( ref GameObject[] objs, List<Vector3> points, float Width, float Heigth, Color color ) {
		if ( objs == null)
			objs = new GameObject[points.Count - 1];

		if (objs.Length < points.Count - 1)
			objs = new GameObject[points.Count - 1];

		for ( long i = 0; i < objs.Length - 1; i++ ) {
			if ( objs[i] == null )
				objs[i] = new GameObject();

			if ( objs[i].GetComponent<LineRenderer>() == null )
				objs[i].AddComponent<LineRenderer> ();
			LineRenderer line = objs[i].GetComponent<LineRenderer> ();

			line.SetWidth(Width, Heigth);
			line.SetVertexCount(2);

			line.SetPosition ( 0, points[(int)i] );
			line.SetPosition ( 1, points[(int)i + 1] );
			
				GameObject Player = PlayerController.Player, Point = SpawnController.Point;

			if ( ( Player.collider.bounds.Contains ( points[(int)i] ) && Player.collider.bounds.Contains ( points[(int)i + 1] ) ) ||
			    ( Point.collider.bounds.Contains ( points[(int)i] ) && Point.collider.bounds.Contains ( points[(int)i + 1] ) ) )
				line.renderer.enabled = false;
			else
				line.renderer.enabled = true;
			line.renderer.material.color = color;
		}
	}

	public static void DrawArc ( ref GameObject[] objs, float StartAngle, float EndAngle, float Radius, float Width, float Heigth, Color color ) {
		float Angle;

		List<Vector3> points;
		points = new List<Vector3>();
		StartAngle = MovementController.RepairAngle (StartAngle);
		EndAngle = MovementController.RepairAngle (EndAngle);
		Angle = StartAngle;

		long i = 0;
		points.Add ( MovementController.ChangeToAngle ( Radius, Angle ) );
		while (Angle != EndAngle) {
			i++;
			Angle += RenderAngle;
			Angle = MovementController.RepairAngle ( Angle );

			if ( Mathf.Abs ( EndAngle - Angle ) < RenderAngle )
				Angle = EndAngle;

			points.Add ( MovementController.ChangeToAngle ( Radius, MovementController.RepairAngle(Angle) ) );
		}
		
		DrawMultiLine ( ref objs, points, Width, Heigth, color );
	}

	public static void DrawMultiArcCircle ( long now, float OffsetDegree, float Radius, long NumberOfArcs, float Width, float Heigth, Color color ) {
		float ArcLengthDegree = 360f / NumberOfArcs, start, end;
		float EmptyLengthDegree = ArcLengthDegree / 5f;
		ArcLengthDegree *= 4f / 5f;

		for ( long i = 0; i < NumberOfArcs; i++ ) {
			start = OffsetDegree + (float)i * ( ArcLengthDegree + EmptyLengthDegree );
			end = OffsetDegree + (float)i * ( ArcLengthDegree + EmptyLengthDegree ) + ArcLengthDegree;

			DrawArc ( ref Lines[now][i],  start, end , Radius, Width, Heigth, color );
		}
	}

	public static void DisableLine ( GameObject obj ) {
		obj.GetComponent<LineRenderer>().renderer.enabled = false;
	}

	public static void EnableLine ( GameObject obj ) {
		obj.GetComponent<LineRenderer>().renderer.enabled = false;
	}

	public static void ReDraw ( float Offset ) {
		long x = 0;
		for ( float i = MovementController.InitialRadius; i <= MovementController.FinalRadius; i+= MovementController.InterpolationRadius, x++ ) {
			float dr = x % 2 == 0 ? 1 : -1;
			DrawMultiArcCircle ( x, dr * Offset, i, NumberOfArcs, MultiArcsWidth, MultiArcsHeigth, Color.cyan );
		}
	}

	void Update () {
		Offset = MovementController.LerpAngle (Offset, RotationDegreeSpeed);
		ReDraw ( Offset );
	}

	void Awake () {
		Lines = new GameObject[4][][];
		for (long i = 0; i < 4; i++) {
			Lines [i] = new GameObject[12][];
			for ( long j = 0; j < 12; j++ )
				Lines[i][j] = new GameObject[2];
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawController : MonoBehaviour {

	public static List<GameObject>[][] Lines;
	public static long NumberOfArcs = 12;
	static int last = 0;
	public static float RenderAngle = 6f, ColliderRenderAngle = 0.1f, MultiArcsWidth = 0.03f, MultiArcsHeigth =0.03f, RotationDegreeSpeed = 20f, Offset = 0f;
	static List<Vector3> tmp = new List<Vector3>();
	static GameObject x;
	static List<float> Angles;

	public static void DrawMultiLine ( GameObject x, ref List<Vector3> tmp, float Width, float Heigth, Color color ) {
		LineRenderer line = x.GetComponent<LineRenderer> ();
		line.SetWidth(Width, Heigth);
		line.SetVertexCount(tmp.Count);
		for ( int j = 0; j < tmp.Count; j++ )
			line.SetPosition ( j, tmp[j] );
		line.renderer.material.color = color;
		line.renderer.enabled = true;
	}

	public static void DrawArc ( ref List<GameObject> list, float StartAngle, float EndAngle, float Radius, float Width, float Heigth, Color color ) {
		float Angle, PrevAngle;

		if (list == null)
			list = new List<GameObject> ();

		Vector3 points;
		StartAngle = MovementController.RepairAngle (StartAngle);
		EndAngle = MovementController.RepairAngle (EndAngle);
		Angle = MovementController.RepairAngle ( StartAngle );

		PrevAngle = EndAngle;

		int a = 0;
		if (Angles == null)
			Angles = new List<float> ();
		if ( a == Angles.Count )
			Angles.Add ( new float() );
		Angles[a] = Angle;

		while (Angle != EndAngle) {
			a++;
			Angle += RenderAngle;
			Angle = MovementController.RepairAngle ( Angle );
			
			if ( Mathf.Abs ( EndAngle - Angle ) < RenderAngle )
				Angle = EndAngle;

			if ( a == Angles.Count )
				Angles.Add ( new float() );
			Angles[a] = Angle;
		}

		tmp.Clear ();
		last = 0;
		for ( int j = 0; j <= a; j++ ) {
			Angle = Angles[j];
			points = MovementController.ChangeToAngle ( Radius, Angle );
			
			if ( !MovementController.IsInCollider ( points ) && j < a ) {
				if ( tmp.Count == 0 && j > 0 ) {
					while ( !MovementController.IsInCollider ( MovementController.ChangeToAngle ( Radius, Angle ) ) )
						Angle = MovementController.RepairAngle (Angle - ColliderRenderAngle);
					points = MovementController.ChangeToAngle ( Radius, Angle );
				}

				tmp.Add ( points );
			}
			else if ( tmp.Count > 0 || j == a) {
				while ( MovementController.IsInCollider ( MovementController.ChangeToAngle ( Radius, Angle ) ) )
					Angle = MovementController.RepairAngle (Angle - ColliderRenderAngle);
				points = MovementController.ChangeToAngle ( Radius, Angle );
				tmp.Add ( points );
					
				if ( last == list.Count ) {
					list.Add ( new GameObject() );
					list[last].AddComponent<LineRenderer>();
				}

				DrawMultiLine ( list[last], ref tmp, Width, Heigth, color );
				last++;

				tmp.Clear ();
			}	
		}
		
		for (int i = (int)last; i < list.Count; i++)
			list [i].GetComponent<LineRenderer>().renderer.enabled = false;
		

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
		Lines = new List<GameObject>[4][];
		for (long i = 0; i < 4; i++)
			Lines [i] = new List<GameObject>[12];
	}
}

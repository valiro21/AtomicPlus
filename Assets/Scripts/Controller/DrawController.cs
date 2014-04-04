using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawController : MonoBehaviour {
	
	public static List<GameObject>[][] Lines;
	public static long NumberOfArcs = 12;
	static int last = 0, qlast = 0;
	public static int  fr = 0, fx = 0;
	public static float RenderAngle = 12f, MultiArcsWidth = 0.03f, MultiArcsHeigth =0.03f, RotationDegreeSpeed = 10f, Offset = 0f;
	static List<Vector3> tmp = new List<Vector3>();
	static GameObject x;
	static List<float> Angles;
	static RaycastHit hit;
	static List<LineRenderer> q = new List<LineRenderer>();
	/*
	public static Vector3 IntersectLineWithCircle ( float radius, Vector3 x, Vector3 y ) {
		float dx = y.x - x.x, dy = y.y - x.y, dr = Mathf.Sqrt ( dx * dx + dy * dy ), D = x.x * y.y - y.x * x.y;
		float delta = radius * radius * dr * dr - D * D;

		if (delta <= 0f)
			return null;
		else {
			Vector3 k = new Vector3();
			k.x = ( D * dy + sgn ( dy ) * dx * Mathf.Sqrt (radius * radius * dr * dr - D * D ) ) / (dr * dr);
			k.y = (-D * dy + Mathf.Abs ( dy ) * dx * Mathf.Sqrt (radius * radius * dr * dr - D * D ) ) / ( dr * dr );

			if ( IsIn2D ( k, x, y  ) )
				return MovementController.ChangeToDegree ( Radius, k);
		}


		x1 = - Mathf.Sqrt
	}

	public static void IntersectBoxAndCircle () {

	}*/

	public static void AddToQueue ( ref LineRenderer x ) {
		if ( qlast == q.Count )
			q.Add ( x );
		else
			q[qlast] = x;

		qlast++;
	}
	
	public static void AddToList (ref List<GameObject> list, ref List<Vector3> tmp, float Width, float Heigth, Material mat) {
		if ( last == list.Count ) {
			list.Add ( new GameObject() );
			list[last].AddComponent<LineRenderer>();
		}
		
		DrawMultiLine ( list[last], ref tmp, Width, Heigth, mat );
		last++;
		tmp.Clear ();
	}
	
	public static void DrawMultiLine ( GameObject x, ref List<Vector3> tmp, float Width, float Heigth, Material mat ) {
		LineRenderer line = x.GetComponent<LineRenderer> ();
		line.SetWidth(Width, Heigth);
		line.SetVertexCount(tmp.Count);
		for ( int j = 0; j < tmp.Count; j++ )
			line.SetPosition ( j, tmp[j] );
		line.castShadows = false;
		line.receiveShadows = false;
		line.renderer.material = mat;
		line.renderer.enabled = false;
		AddToQueue (ref line);
	}
	
	public static void DrawArc ( ref List<GameObject> list, float StartAngle, float EndAngle, float Radius, float Width, float Heigth, Material mat ) {
		float Angle, PrevAngle;
		float renderAngle = RenderAngle / (float)(fr + 1);
		if (PlayerController.Draw)
			renderAngle = 2f;

		if (list == null)
			list = new List<GameObject> ();
		
		Vector3 points = new Vector3 ( 0, 0, 0 ),prevpoints = new Vector3 ( 0, 0, 0 ), ptmp = new Vector3 ();
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
			Angle += renderAngle;
			Angle = MovementController.RepairAngle ( Angle );
			
			if ( Mathf.Abs ( EndAngle - Angle ) < renderAngle )
				Angle = EndAngle;
			
			if ( a == Angles.Count )
				Angles.Add ( new float() );
			Angles[a] = Angle;
		}
		
		tmp.Clear ();
		last = 0;
		a++;
		for ( int j = 0; j <= a; j++ ) {
			if ( j < a ) {
				prevpoints = points;
				Angle = Angles[j];
				points = MovementController.ChangeToAngle ( Radius, Angle );
			}
			
			ptmp = new Vector3 ( points.x, points.y, points.z - 20f );
			bool collision = Physics.Raycast ( ptmp, points - ptmp, Vector3.Distance ( ptmp, points ) ) |
				PlayerController.Player.collider.bounds.Contains ( points ) |
				SpawnController.Point.collider.bounds.Contains ( points );
			if ( !collision && j < a ) {
				if ( j > 0 && Physics.Raycast ( points, prevpoints - points,  out hit, Vector3.Distance ( prevpoints, points ) ) ) {
					if ( tmp.Count == 0 ) {
						tmp.Add ( hit.point );
						tmp.Add ( points );
					}
					else {
						ptmp = hit.point;
						
						if ( Physics.Raycast ( prevpoints, points - prevpoints,  out hit, Vector3.Distance ( prevpoints, points ) ) ) {
							tmp.Add ( hit.point );
							AddToList (ref list, ref tmp, Width, Heigth, mat);
						}
						
						tmp.Add ( ptmp );
						tmp.Add ( points );
					}
				}
				else
					tmp.Add ( points );
			}
			else if ( tmp.Count > 0 ) {
				if ( j == a )
					AddToList (ref list, ref tmp, Width, Heigth, mat);
				else {
					points = MovementController.ChangeToAngle ( Radius, Angle );
					if ( Physics.Raycast ( prevpoints, points - prevpoints,  out hit, Vector3.Distance ( prevpoints, points ) ) )
						points = hit.point;
					tmp.Add ( points );
					
					AddToList (ref list, ref tmp, Width, Heigth, mat);
				}
			}	
		}
		
		for (int i = (int)last; i < list.Count; i++)
			list [i].GetComponent<LineRenderer>().renderer.enabled = false;
		
		
	}
	
	public static void DrawMultiArcCircle ( long now, float OffsetDegree, float Radius, long NumberOfArcs, float Width, float Heigth, Material mat ) {
		float ArcLengthDegree = 360f / NumberOfArcs, start, end;
		float EmptyLengthDegree = ArcLengthDegree / 5f;
		ArcLengthDegree *= 4f / 5f;
		
		for ( long i = 0; i < NumberOfArcs; i++ ) {
			start = OffsetDegree + (float)i * ( ArcLengthDegree + EmptyLengthDegree );
			end = OffsetDegree + (float)i * ( ArcLengthDegree + EmptyLengthDegree ) + ArcLengthDegree;
			
			DrawArc ( ref Lines[now][i],  start, end , Radius, Width, Heigth, mat );
		}
	}
	
	public static void DisableLine ( GameObject obj ) {
		obj.GetComponent<LineRenderer>().renderer.enabled = false;
	}
	
	public static void EnableLine ( GameObject obj ) {
		obj.GetComponent<LineRenderer>().renderer.enabled = false;
	}
	
	public static void ReDraw ( float Offset ) {
		float i = MovementController.InitialRadius + fr * MovementController.InterpolationRadius, dr = fr % 2 == 0 ? 1 : -1;
		DrawMultiArcCircle ( fr, dr * Offset, i, NumberOfArcs, MultiArcsWidth, MultiArcsHeigth, Resources.Load ( "Materials/MovingRings" ) as Material );
	}

	public static void ReDrawFromQueue () {
		for (int i = 0; i < qlast; i++)
			q[i].renderer.enabled = true;
		qlast = 0;
	}
	
	void Update () {
		Offset = MovementController.LerpAngle (Offset, RotationDegreeSpeed );
		PlayerController.MovePlayer();
		for (fr = 0; fr < 4; fr++)
			ReDraw (Offset);
		ReDrawFromQueue ();
		fr = 0;
	}
	
	void Awake () {
		Lines = new List<GameObject>[4][];
		for (long i = 0; i < 4; i++)
			Lines [i] = new List<GameObject>[12];
	}
}

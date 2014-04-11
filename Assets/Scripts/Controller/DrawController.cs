using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawController : MonoBehaviour {
	public static int TotalQ = 200;
	public static int NumberOfCalls = 60;
	static List<Angle>[] Angles = new List<Angle>[12];
	static int[] AnglesLast = new int[11];
	static List<float> Radiuses = new List<float>();
	static GameObject[] q = new GameObject[TotalQ];
	static int qlast;
	static Angle Offset = 0, RotationDegreeSpeed = 20f;

	public static void AddToAngles ( ref Angle x, int i ) {
		if (Angles [i] == null)
			Angles[i] = new List<Angle>();

		if ( AnglesLast[i] < Angles[i].Count - 1 ) {
			Angles[i][AnglesLast[i]] = x;
			AnglesLast[i]++;
		}
		else {
			Angles[i].Add ( x );
			AnglesLast[i]++;
		}
	}
	

	public static void DrawArc ( Angle start, Angle end, Angle RenderAngle, float Radius, float Width, float Heigth, Material mat ) {
		int NumberOfAngles = (int)((float)start.ArcLengthTo ( end ) / (float)RenderAngle);
		int last = 0;
		Angle angle;
		if ( start + NumberOfAngles * RenderAngle != end )
			last=1;

		LineRenderer line = q[qlast].GetComponent<LineRenderer> ();
		line.SetWidth(Width, Heigth);
		line.SetVertexCount(NumberOfAngles + last);
		line.castShadows = false;
		line.receiveShadows = false;
		line.renderer.material = mat;
		line.renderer.enabled = false;
		qlast++;

		for ( int i = 0; i < NumberOfAngles + last; i++ ) {
			angle = start + i * RenderAngle;
			if ( i == NumberOfAngles && last > 0 )
				angle = end;
			line.SetPosition ( i, angle.PointByRadius ( Radius ) );
		}

	}

	public static void DrawArcColliders ( ref List<Angle> x, int size, Angle start, Angle end, Angle RenderAngle, float Radius, float Width, float Heigth, Material mat ) {
		if (size == 0)
			DrawArc ( start, end, RenderAngle, Radius, Width, Heigth, mat );
		else {
			int starte = -2, ende = -2;
			bool f = false;
			for ( int e = 0; e < size - 1; e+= 2 )
				if ( start.IsBetween( x[e], x[e+1] ) )
					start = x[e+1];

			for ( int e = size - 1; e > 0; e-= 2 )
				if ( end.IsBetween ( x[e-1], x[e] ) )
					end = x[e-1];

			for ( int e = 0; e < size - 1; e += 2 ) {
				if ( x[e].IsBetween ( start, end ) && f == false ) {
					starte = e;
					f = true;
				}

				if ( !x[e].IsBetween ( start, end )&& f == true ) {
					ende = e-1;
					f = false;
				}
			}

			if ( starte == ende || ende == -1 )
				DrawArc ( start, end, RenderAngle, Radius, Width, Heigth, mat );
			else {
				if ( ende == -2 )
					ende = size - 1;

				int e = starte;
				Angle f1 = start, f2 = end;
				while ( f1 != x[ende] ) {
					f2 = x[e];
					DrawArc ( f1, f2, RenderAngle, Radius, Width, Heigth, mat );
					e = ( e + 1 ) % size;
					f1 = x[e];
					e = ( e + 1 ) % size;
				}

				DrawArc ( f1, end, RenderAngle, Radius, Width, Heigth, mat );
			}
		}

	}

	public static void DrawMultiArcCircle ( int now, Angle Offset, float Radius, long NumberOfArcs, float Width, float Heigth, Material mat ) {
		Angle ArcLengthDegree = 360f / NumberOfArcs, start, end;
		Angle EmptyLengthDegree = ArcLengthDegree / 5f;
		ArcLengthDegree *= 4f / 5f;
		
		for ( long i = 0; i < NumberOfArcs; i++ ) {
			start = Offset + (float)i * ( ArcLengthDegree + EmptyLengthDegree );
			end = Offset + (float)i * ( ArcLengthDegree + EmptyLengthDegree ) + ArcLengthDegree;
			
			DrawArcColliders ( ref Angles[now], AnglesLast[now], start, end , 1f, Radius, Width, Heigth, mat );
		}
	}

	public static void ReDrawFromQueue () {
		for (int i = 0; i < qlast; i++) {
			q[i].renderer.enabled = true;
			q[i].name = "Active";
		}

		for ( int i = qlast; i < TotalQ; i++ ) {
			q[i].renderer.enabled = false;
			q[i].name = "Inactive";
		}
		qlast = 0;
	}

	void Update () {
		Offset += RotationDegreeSpeed * Time.deltaTime;
		qlast = 0;

		for (int i = 0; i < 4; i++) {
			DrawMultiArcCircle ( i, Offset, Radiuses[i], 12, 0.04f, 0.04f, Resources.Load ("Materials/MovingRings") as Material );
		}

		if ( !PlayerController.IsDead () )
			DrawArcColliders ( ref Angles[4], AnglesLast[4], 0f, 359.9f, 1f, PlayerController.GetRadius(), 0.05f, 0.05f, Resources.Load ( "Materials/WhiteGUI" ) as Material );

		ReDrawFromQueue ();
	}

	void FixedUpdate () {
		PlayerController.MovePlayer ();
		Radiuses [4] = PlayerController.GetRadius ();
		for (int i = 0; i < Radiuses.Count; i++) {
			if ( Angles[i] == null )
				Angles[i] = new List<Angle>();
			
			float radius = Radiuses[i];
			AnglesLast[i] = 0;

			PlayerController.AddToAngle ( i, radius );
			SpawnController.AddToAngle ( i, radius );
			if ( AnglesLast[i] > 2 ) {
				if ( Angles[i][0] < Angles[i][1] && Angles[i][0] > Angles[i][2]) {
					Angle tmp;
					tmp = Angles[i][0]; Angles[i][0] = Angles[i][2]; Angles[i][2] = tmp;
					tmp = Angles[i][1]; Angles[i][1] = Angles[i][3]; Angles[i][3] = tmp;
				}
			}
		}
	}

	/*void GetCollisions () {
		Angle FixedRenderAngle, k;
		RaycastHit hit;
		Vector3 p1, p2;
		float d, d1, radius;

		for ( int i = 0; i < Radiuses.Count; i++ ) {
			if ( Angles[i] == null )
				Angles[i] = new List<Angle>();

			radius = Radiuses[i];
			FixedRenderAngle = 360f / (float)NumberOfCalls;
			AnglesLast[i] = 0;
			Angle lim = 359.9999f, j = 0f, pj;
			bool exception = false;

			for ( int l = 0; l < NumberOfCalls; l++) {
				j+= FixedRenderAngle;
				pj = j - FixedRenderAngle;
				p1 = pj.PointByRadius ( radius );
				p2 = j.PointByRadius ( radius );
				Debug.DrawRay ( p1, p2 - p1, Color.cyan, 3f );
				if ( Physics.Raycast ( p1, p2 - p1, out hit, Vector3.Distance ( p1, p2 ) ) ) {
					d = Vector3.Distance ( p1, p2 );
					d1 = Vector3.Distance ( p1,hit.point );
					k =  pj + (j -pj) * (d1/d); 

					AddToAngles ( ref k, i );
				}

				if ( Physics.Raycast ( p2, p1 - p2, out hit, Vector3.Distance ( p1, p2 ) ) ) {
					if ( AnglesLast[i] == 0 ) exception = true;

					d = Vector3.Distance ( p1, p2 );
					d1 = Vector3.Distance ( p1,hit.point );
					k =  pj + (j -pj) * (d1/d); 
					
					AddToAngles ( ref k, i );
				}
			}

			if ( exception == true ) {
				Angle tmp = Angles[i][AnglesLast[i] - 1];
				for ( int l = AnglesLast[i] - 1; l > 0 ; l-- )
					Angles[i][l] = Angles[i][l-1];
				Angles[i][0] = tmp;
			}
		}
	}*/

	void Start ()  {
		for (long i = 0; i < 4; i++)
			Radiuses.Add ( MovementController.InitialRadius + i * MovementController.InterpolationRadius );
		Radiuses.Add (PlayerController.GetRadius () );
	}

	void Awake () {
		for (long i = 0; i < TotalQ; i++) {
			q[i] = new GameObject ( "Line" );
			q[i].AddComponent<LineRenderer>();
			q[i].renderer.enabled = false;
		}
	}
}

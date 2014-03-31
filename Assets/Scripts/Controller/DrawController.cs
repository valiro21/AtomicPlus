using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawController : MonoBehaviour {

	public static GameObject[] Lines = new GameObject[80];
	public static long last = 0;

	public static void DrawMultiLine ( List<Vector3> points ) {
		Lines [last].transform.position = points [0];
		LineRenderer line = Lines[last].GetComponent<LineRenderer>();
		line.SetWidth(0.05f, 0.05f);
		line.SetVertexCount(points.Count);

		for ( long i = 0; i < points.Count; i++ )
			line.SetPosition ( (int)i, points[(int)i] );
		line.renderer.enabled = true;

		last++;
	}

	public static void DrawArc ( float StartAngle, float EndAngle, float Radius ) {
		float RenderAngle = 0.1f, Angle;

		List<Vector3> points;
		points = new List<Vector3>();
		Angle = StartAngle;

		long i = 0;
		points.Add ( MovementController.ChangeToAngle ( Radius, Angle ) );
		while (Angle != EndAngle) {
			i++;
			Angle += RenderAngle;
			if ( Angle > 360f )
				Angle -= 360f;
			if ( Angle > EndAngle )
				Angle = EndAngle;

			points.Add ( MovementController.ChangeToAngle ( Radius, Angle ) );
		}
		
		DrawMultiLine ( points );
	}

	public static void DrawMultiArcCircle ( float Radius, long NumberOfArcs ) {
		float ArcLengthDegree = 360f / NumberOfArcs;
		float EmptyLengthDegree = ArcLengthDegree / 5f;
		ArcLengthDegree *= 4f / 5f;
		for ( long i = 0; i < NumberOfArcs; i++ )
			DrawArc ( (float)i * ( ArcLengthDegree + EmptyLengthDegree ), (float)i * ( ArcLengthDegree + EmptyLengthDegree ) + ArcLengthDegree, Radius );
	}

	public static void Reset () {
		for ( float i = MovementController.InitialRadius; i <= MovementController.FinalRadius; i+= MovementController.InterpolationRadius )
			DrawMultiArcCircle ( i, 12 );

		last = 0;
	}

	void Awake () {
		for (long i = 0; i < Lines.Length; i++)
			Lines[i] = Instantiate ( Resources.Load ( "Line" ), new Vector3 ( 0, 0, 0 ), Quaternion.Euler ( 0, 0, 0 ) ) as GameObject;
	}
}

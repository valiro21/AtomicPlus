using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {
	public static GameObject Point, Atom;
	static long LastRing = 4, Wait = 1;
	public static float OutOfBounds = 12f;
	static int minplaces, maxplaces, pscore, nplaces, mode, number, psecond, nrmin;
	static Angle[] place = new Angle[5];

	public static Angle GetAngleFromPoint ( float Radius, Vector3 position ) {
		Vector3 tmp = GameController.Ring;
		tmp.x -= Radius;

		float dx = Vector3.Distance ( tmp, position );

		if ( position.y > 0 )
			return new Angle( Mathf.Acos ( (2*Radius*Radius - dx *dx)/(2*Radius*Radius) ), 0);
		else if ( position.y == 0 )
			if (position.x < 0)
				return new Angle (0f);
			else
			return new Angle (180f);
		else
			return new Angle( -Mathf.Acos ( (2*Radius*Radius - dx *dx)/(2*Radius*Radius) ), 2);
	}


	static Vector3 Intersect (float CircleRad, Vector3 LineStart, Vector3 LineEnd)
	{
		if (IsIntersecting(GameController.Ring, CircleRad, LineStart, LineEnd)) 
		{
			//Calculate terms of the linear and quadratic equations
			var M = (LineEnd.y - LineStart.y) / (LineEnd.x - LineStart.x);
			var B = LineStart.y - M * LineStart.x;
			var a = 1 + M * M;
			var b = 2 * M * B;
			var c = B * B - CircleRad * CircleRad;
			// solve quadratic equation
			var sqRtTerm = Mathf.Sqrt(b * b - 4 * a * c);
			var x = ((-b) + sqRtTerm)/(2*a);
			// make sure we have the correct root for our line segment
			if ( LineStart.x < LineEnd.x && ((x < LineStart.x) || (x > LineEnd.x) ) )
			    x = ((-b) - sqRtTerm)/(2*a);
			else if ( LineStart.x > LineEnd.x && ((x > LineStart.x) || (x < LineEnd.x) ) )
				x = ((-b) - sqRtTerm)/(2*a);
			//solve for the y-component
			var y = M * x + B;
			// Intersection Calculated
			return new Vector3(x, y, GameController.Ring.z);
		} else {
			// Line segment does not intersect at one point.  It is either fully outside,
			// fully inside, intersects at two points, is tangential to, or one or more
			// points is exactly on the circle radius.
			return new Vector3 ( 0, 0, 0 );
		}            
	}

	static bool IsIntersecting(Vector3 CirclePos, float CircleRad, Vector3 LineStart, 
	                            Vector3 LineEnd)
	{
		if (IsInsideCircle(CirclePos, CircleRad, LineStart) ^
		    IsInsideCircle(CirclePos, CircleRad, LineEnd)) 
		{ return true; } else return false;
	}

	static bool IsInsideCircle(Vector3 CirclePos, float CircleRad, Vector3 checkPoint)
	{
		if (Mathf.Sqrt(Mathf.Pow((CirclePos.x - checkPoint.x), 2) +
		              Mathf.Pow((CirclePos.y - checkPoint.y), 2)) < CircleRad)
		{ return true; } else return false;
	}

	public static void AddToAngle (int i, float R) {
		Vector3 position = Point.transform.position;
		Angle k = Point.transform.eulerAngles.z;
		k = k.degree % 90f;

		float r1 = Mathf.PI / 4, r2 = -Mathf.PI / 4;
		float l = 0.6f * Mathf.Sqrt (2f) / 4f;
		Vector3 p1 = new Vector3 (position.x - l * Mathf.Sin (r1 - k.radian), position.y - l * Mathf.Cos (r1 - k.radian), GameController.Ring.z);
		Vector3 p2 = new Vector3 (position.x - l * Mathf.Sin (r2 - k.radian), position.y - l * Mathf.Cos (r2 - k.radian), GameController.Ring.z); 
		Vector3 p3 = new Vector3 (position.x + l * Mathf.Sin (r1 - k.radian), position.y + l * Mathf.Cos (r1 - k.radian), GameController.Ring.z); 
		Vector3 p4 = new Vector3 (position.x + l * Mathf.Sin (r2 - k.radian), position.y + l * Mathf.Cos (r2 - k.radian), GameController.Ring.z); 

		Debug.DrawLine ( p4, p1, Color.red );
		Debug.DrawLine (p3, p2, Color.red);
		Debug.DrawLine (p2, p1, Color.green);
		Debug.DrawLine (p4, p3, Color.green);

		Vector3[] x = new Vector3[4];
		Vector3 aux;

		x[0] = Intersect (R, p4, p1);
		x[1] = Intersect (R, p3, p2);
		x[2] = Intersect (R, p2, p1);
		x[3] = Intersect (R, p4, p3);

		for ( int ik = 0; ik < 4; ik++) {
			if ( x[ik] != new Vector3(0, 0, 0) ) {
				aux = x[0];
				x[0]= x[ik];
				x[ik] = aux;
				break;
			}
		}

		for ( int ik = 1; ik < 4; ik++) {
			if ( x[ik] != new Vector3(0, 0, 0) ) {
				aux = x[1];
				x[1]= x[ik];
				x[ik] = aux;
				break;
			}
		}

		if ( x[0] != new Vector3 (0, 0, 0) && x[1] != new Vector3 (0, 0, 0) ) {
			Angle k1 = GetAngleFromPoint ( R, x[0] ), k2 = GetAngleFromPoint ( R, x[1] );
			if ( k1 < k2 ) {
				if ( k2 - k1 > 180f ) {
					DrawController.AddToAngles ( ref k2, i );
					DrawController.AddToAngles ( ref k1, i );
				}
				else {
					DrawController.AddToAngles ( ref k1, i );
					DrawController.AddToAngles ( ref k2, i );
					}
			}
			else {
				if ( k1 - k2 > 180f ) {
					DrawController.AddToAngles ( ref k1, i );
					DrawController.AddToAngles ( ref k2, i );
				}
				else {
					DrawController.AddToAngles ( ref k2, i );
					DrawController.AddToAngles ( ref k1, i );
				}

			}
		}

	}
	
	public static void ResetPoint () {
		LastRing = 4;
		Point.rigidbody.velocity = new Vector3 (0f, 0f, 0f);
		Point.transform.position = new Vector3 (0f, 0f, 50f);
	}

	public static void SpawnPoint () {
		RevivePoint ();

		BackgroundController.ChangeBackground ();
		float Radius;
		long Ring = 4;
		Angle DegreeAngle = Random.Range (0f, 360f);
		while ( Ring == 4 || Ring == LastRing )
			Ring = (long)Random.Range ( 0f, 4f );

		Radius = PlayerController.GetRadiusOfRing (Ring);

		LastRing = Ring;
		Point.rigidbody.velocity = new Vector3 (0f, 0f, 0f);
		Point.transform.position = DegreeAngle.PointByRadius (Radius);
	}

	public static void RevivePoint () {
		foreach ( Transform i in Point.transform )
			i.renderer.enabled = true;
	}

	public static void DestroyPoint () {
		foreach ( Transform i in Point.transform )
			i.renderer.enabled = false;
		Point.transform.position = new Vector3 (0, 0, 0);
	}

	public static void Reset () {
		minplaces = GameController.Difficulty;
		maxplaces = 3;
		nplaces = 0;
		pscore = 0;
		mode = 0;
		number = 0;
		psecond = (int)Time.time;
		nrmin = GameController.Difficulty;
		Wait = 1;
	}

	public static void MoveByOffset ( float OffsetDegree ) {
		for ( int i = 0; i < nplaces; i++ )
			place[i] += OffsetDegree;
	}

	void Awake () {
		Atom = GameObject.Find ("Atom");
		Point = Instantiate ( Resources.Load<GameObject> ("Point"), new Vector3 ( 0, 0, 50 ), Quaternion.Euler (0, 0, 0) ) as GameObject;
		DestroyPoint ();
	}
	
	// Update is called once per frame
	void Update () {
		if ( GameController.mode == 3 ) {
			int second = (int)Time.time;

			if (GameController.score == 10)
				maxplaces = 4;
			else if (GameController.score == 30)
				maxplaces = 5;
			else if (GameController.score == 15)
				minplaces = 2;
			else if (GameController.score == 100)
				minplaces = 3;
			else if ( GameController.score == 40 )
				nrmin = 2;

			if ( GameController.Difficulty == 2 )
				if ( GameController.score == 40 )
					maxplaces = 6;

			if (pscore < GameController.score) {
				Wait = 2;
				pscore = GameController.score;
	
				Angle Angle = PlayerController.Angle;
				nplaces = Random.Range (minplaces, maxplaces + 1);
				for ( int i = 0; i < nplaces; i++ )
					place[i] = Angle + 360f/(float)nplaces * i;


				mode = Random.Range ( 0, 4 );
				number = Random.Range ( nrmin, 4 );
			}

			if (psecond < second && second % Wait == 0) {
				psecond = second;
				Wait = 1;
				if ( mode != 3 || ( mode == 3 && second % 3 == 0 ) )
				for ( int i = 0; i < nplaces; i++ )
					SpawnPool.SpawnObjects ( place[i], 10f, number, mode );
			}
		}
	}
}

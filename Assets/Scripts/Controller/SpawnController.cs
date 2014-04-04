using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {
	public static GameObject Point, Atom;
	static long LastRing = 4, Wait = 1;
	public static float OutOfBounds = 12f;
	static int minplaces, maxplaces, pscore, nplaces, mode, number, psecond, nrmin;
	static float[] place = new float[5];

	public static void ResetPoint () {
		LastRing = 4;
		Point.rigidbody.velocity = new Vector3 (0f, 0f, 0f);
		Point.transform.position = new Vector3 (0f, 0f, 50f);
	}

	public static void SpawnPoint () {
		RevivePoint ();

		BackgroundController.ChangeBackground ();
		float DegreeAngle, RadianAngle, Radius;
		long Ring = 4;
		DegreeAngle = Random.Range (0f, 360f);
		while ( Ring == 4 || Ring == LastRing )
			Ring = (long)Random.Range ( 0f, 4f );

		Radius = PlayerController.GetRadiusOfRing (Ring);
		RadianAngle = DegreeAngle * Mathf.PI / 180f;

		LastRing = Ring;
		Point.rigidbody.velocity = new Vector3 (0f, 0f, 0f);
		Point.transform.position = MovementController.ChangeToAngle (Radius, DegreeAngle);
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
			place[i] = MovementController.RepairAngle ( place[i] + OffsetDegree );
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
	
				float Angle = PlayerController.Angle;
				nplaces = Random.Range (minplaces, maxplaces + 1);
				for ( int i = 0; i < nplaces; i++ )
					place[i] = MovementController.RepairAngle (Angle + 360f/(float)nplaces * i);


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

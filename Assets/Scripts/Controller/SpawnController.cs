using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {
	static GameObject Point, Atom;
	static long LastRing = 4, prevtime = 0;
	public static float OutOfBounds = 12f;

	public static void ResetPoint () {
		LastRing = 4;
		Point.rigidbody.velocity = new Vector3 (0f, 0f, 0f);
		Point.transform.position = new Vector3 (0f, 0f, 50f);
	}

	public static void SpawnPoint () {
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
	

	void Awake () {
		Point = Instantiate ( Resources.Load<GameObject> ("Point"), new Vector3 ( 0, 0, 50 ), Quaternion.Euler (0, 0, 0) ) as GameObject;
		Atom = GameObject.Find ("Atom");
		SpawnPoint ();
	}
	
	// Update is called once per frame
	void Update () {
		if ((long)Time.time - prevtime > 0)
			Atom.GetComponent<SpawnPool>().SpawnObjects ( 30f, 10f, 3, (long)Time.time % 4 );
		prevtime = (long)Time.time;
	}
}

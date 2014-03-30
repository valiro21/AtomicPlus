using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {
	static GameObject Point;
	static long LastRing = 4;

	public static ResetPoint () {
		LastRing = 4
		Point.rigidbody.velocity = new Vector3 (0f, 0f, 0f);
		Point.transform.position = new Vector3 ( 0, 0, 50 );
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
		SpawnPoint ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

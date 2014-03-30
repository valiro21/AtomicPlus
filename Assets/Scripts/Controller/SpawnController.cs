using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

	public GameObject Point;

	void SpawnPoint () {
		float DegreeAngle, RadianAngle, Radius;
		long Ring = 4;
		DegreeAngle = Random.Range (0f, 360f);
		while ( Ring != 4 )
			Ring = (long)Random.Range ( 0, 4 );

		Radius = PlayerController.GetRadiusOfRing (Ring);
		RadianAngle = DegreeAngle * Mathf.PI / 180f;


		Vector3 position = new Vector3 (GameController.Ring.x - Radius * Mathf.Cos (RadianAngle), GameController.Ring.y + Radius * Mathf.Sin (RadianAngle), GameController.Ring.z);

		Instantiate ( Resources.Load<GameObject> ("Point"), position, Quaternion.Euler (0, 0, 0) );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

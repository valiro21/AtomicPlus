using UnityEngine;
using System.Collections;

public class PointMovement : MonoBehaviour {

	public float RotationSpeedPerSecond = 10f;
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler (transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + RotationSpeedPerSecond * Time.deltaTime);
	}
}

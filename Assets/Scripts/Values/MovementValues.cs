using UnityEngine;
using System.Collections;

public class MovementValues : MonoBehaviour {

	public float InitialRadius, InterpolationRadius, FinalRadius;
	
	// Update is called once per frame
	void Awake () {
		FinalRadius = InitialRadius + 3 * InterpolationRadius;
	}
}

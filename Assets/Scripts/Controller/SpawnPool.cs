using UnityEngine;
using System.Collections;

public class SpawnPool : MonoBehaviour {

	public static float NumerOfObjectsInPool = 200;
	public static Object PoolObject = Resources.Load ( "Bullet" );
	
	static GameObject[] PoolObjects;
	

	public static bool AreMonstersAlive () {
		for ( long i = 0; i < (int)NumerOfObjectsInPool; i++)
			if ( PoolObjects[i].active == true )
				return true;
		return false;
	}

	public static void SpawnObject ( float Degree, long mode ) {
		for ( long i = 0; i < (int)NumerOfObjectsInPool; i++) {
			if ( PoolObjects[i].active == false ) {
				PoolObjects[i].transform.position = MovementController.ChangeToAngle ( Menu.Radius, Degree );
				PoolObjects[i].active = true;
				PoolObjects[i].GetComponent<EnemyMovement>().Reset( Menu.Radius, Degree, mode );
				break;
			}
		}
	}

	public static void SpawnObjects ( float Degree, float OffsetDegree, long number, long mode) {
		float MinusOffsetAngle = Degree - OffsetDegree, PlusOffsetAngle = Degree + OffsetDegree;
		if (MinusOffsetAngle < 0f)
			MinusOffsetAngle += 360f;
		if (PlusOffsetAngle > 360f)
			PlusOffsetAngle -= 360f;

		if (number % 2 > 0)
			SpawnObject ( Degree, mode );
		if ( number >= 2 ) {
			SpawnObject ( MinusOffsetAngle, mode );
			SpawnObject ( PlusOffsetAngle, mode );
		}
	}



	void Awake () {
		PoolObjects = new GameObject[(int)NumerOfObjectsInPool];
		
		for ( long i = 0; i < (int)NumerOfObjectsInPool; i++) {
			PoolObjects[i] = Instantiate ( PoolObject, transform.position, transform.rotation ) as GameObject;
			PoolObjects[i].active = false;
		}
	}
}

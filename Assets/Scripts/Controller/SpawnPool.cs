using UnityEngine;
using System.Collections;

public class SpawnPool : MonoBehaviour {

	public float NumerOfObjectsInPool;
	public Object PoolObject;
	public float SpawnEnableLevel;
	
	GameObject[] PoolObjects;
	

	public bool AreMonstersAlive () {
		for ( long i = 0; i < (int)NumerOfObjectsInPool; i++)
			if ( PoolObjects[i].active == true )
				return true;
		return false;
	}

	public void SpawnObject ( float Degree, long mode ) {
		for ( long i = 0; i < (int)NumerOfObjectsInPool; i++) {
			if ( PoolObjects[i].active == false ) {
				PoolObjects[i].transform.position = MovementController.ChangeToAngle ( gameObject.transform.localScale.x, Degree );
				PoolObjects[i].active = true;
				PoolObjects[i].GetComponent<EnemyMovement>().Reset( gameObject.transform.localScale.x, Degree, mode );
				break;
			}
		}
	}

	public void SpawnObjects ( float Degree, float OffsetDegree, long number, long mode) {
		float MinusOffsetAngle = Degree - OffsetDegree, PlusOffsetAngle = Degree + OffsetDegree;
		if (MinusOffsetAngle < 0f)
			MinusOffsetAngle += 360f;
		if (PlusOffsetAngle > 360f)
			PlusOffsetAngle -= 360f;

		if (number % 2 > 0)
			SpawnObject ( Degree, mode );
		if ( number > 2 ) {
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

	void Update () {
	}
}

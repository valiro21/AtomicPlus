using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public static float score = 0, level;
	public static Vector3 LeftRing = new Vector3(-1f, 0,-50f), Ring = new Vector3(0,0,-50f);
	public static GameObject God, ValuesGod;

	public IEnumerator LevelOver () {
		yield return new WaitForSeconds (1f);
	}

	public void StartLevelOver () {
		StartCoroutine ( LevelOver() );
	}

	public static void GameOver () {
		God.GetComponent<GameController> ().StartLevelOver ();
	}

	public static void GetPoint () {
		score++;
	}

	public static void RemakeGameObjectList ( ref List<GameObject> list ) {
		if (list == null)
			list = new List<GameObject> ();
		for (int i = 0; i < list.Count; i++)
			Destroy (list [i]);
		list.Clear ();
	}
	
	void Awake () {
		God = gameObject;
		ValuesGod = GameObject.Find ( "ValuesGod" );
	}

	void Start () {
	}
}

using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static float score = 0, level;
	public static Vector3 Ring = new Vector3(0,0,-50);
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
	
	void Awake () {
		God = gameObject;
		ValuesGod = GameObject.Find ( "ValuesGod" );
	}

	void Start () {
	}
}

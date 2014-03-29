using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static float score, level;
	public static Vector3 center = new Vector3(0,0,0);
	public static GameObject God;

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
	}
}

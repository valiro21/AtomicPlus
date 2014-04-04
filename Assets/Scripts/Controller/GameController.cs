using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	
	public static Vector3 LeftRing = new Vector3(-1f, 0,-50f), Ring = new Vector3(0,0,-50f);
	public static GameObject God, ValuesGod, AudioGod;
	public static int mode = 0, Difficulty = 0, GameMode = 0, score = 0, level = 0, PlayMode = 0; 
	public static bool Highscore = false;

	public static void GameOver ( ) {
		Highscore = false;
		if ( score > PlayerPrefs.GetInt ( "Highscore" ) ) {
			PlayerPrefs.SetInt ("Highscore", score);
			Highscore = true;
		}
		mode = 4;
		PlayerController.Kill ();
		SpawnController.DestroyPoint ();
		AudioController.StartMenuMusic ();
	}

	public static void GameOver ( int NextMode ) {
		Highscore = false;
		if ( score > PlayerPrefs.GetInt ( "Highscore" ) ) {
			PlayerPrefs.SetInt ("Highscore", score);
			Highscore = true;
		}
		mode = NextMode;
		PlayerController.Kill ();
		SpawnController.DestroyPoint ();
		AudioController.StartMenuMusic ();
	}

	public static void GetPoint () {
		score++;
	}
	

	public static void StartGame () {
		//SpawnController.Spawn = Difficulty + 1;
		AudioController.StartGameMusic ();
		PlayerController.Revive ();
		SpawnController.Reset ();
		SpawnController.SpawnPoint ();
		score = 1;
		mode = 3;
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
		AudioGod = GameObject.Find ( "AudioGod" );
		ValuesGod = GameObject.Find ( "ValuesGod" );
	}

	void Start () {
		SpawnController.DestroyPoint ();
	}
}

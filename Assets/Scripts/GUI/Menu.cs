using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour {
	public static float Radius;
	public static GameObject MenuObject;
	static List<GameObject> TextList;
	static int last = 0, i = 0;
	static string[] DifficultyMode, GameplayMode, GameplayHelp, GameOverList;
	static float MistakeRadius =0.1f;

	public static void Alloc ( int x ) {
		if ( TextList == null )
			TextList = new List<GameObject>();

		last = 0;
		for ( int i = 0; i < x; i++ ) {
			if ( i > TextList.Count - 1 ) {
				TextList.Add ( new GameObject ( "Text" ) );
				TextList[i].AddComponent<TextMesh>();
			}
			TextList[i].renderer.enabled = false;
		}

		for ( int i = x; i < TextList.Count; i++ )
			TextList[i].renderer.enabled = false;
	}

	public static void CreateText ( string name, int FontSize, float tposition ) {
		float y;
		if ( tposition >= 0 )
			y= Mathf.Lerp ( MenuObject.transform.position.y, MenuObject.GetComponent<SphereCollider>().radius, tposition );
		else
			y= Mathf.Lerp ( MenuObject.transform.position.y, -MenuObject.GetComponent<SphereCollider>().radius, -tposition );
		TextList[last].transform.position = new Vector3 ( MenuObject.transform.position.x, y, - 20f );
		TextMesh x = TextList[last].GetComponent<TextMesh>();

		x.characterSize = 0.1f;
		x.fontSize = FontSize;
		x.text = name;
		x.anchor = TextAnchor.MiddleCenter;
		x.alignment = TextAlignment.Center;
		x.font = Resources.Load ( "PressStart2P" ) as Font;
		x.font.material = Resources.Load ("PressStart2Pm") as Material;
		x.font.material = Resources.Load ("PressStart2Pm") as Material;
		TextList [last].renderer.material = Resources.Load ("PressStart2Pm") as Material;
		TextList[last].renderer.enabled = true;
		last ++;
	}

	public static void CreateText ( int name, int FontSize, float tposition ) {
		float y;
		if ( tposition >= 0 )
			y= Mathf.Lerp ( MenuObject.transform.position.y, MenuObject.GetComponent<SphereCollider>().radius, tposition );
		else
			y= Mathf.Lerp ( MenuObject.transform.position.y, -MenuObject.GetComponent<SphereCollider>().radius, -tposition );

		TextList[last].transform.position = new Vector3 ( MenuObject.transform.position.x, y, - 20f );
		TextMesh x = TextList[last].GetComponent<TextMesh>();
		
		x.characterSize = 0.1f;
		x.fontSize = FontSize;
		x.text = name.ToString();
		x.anchor = TextAnchor.MiddleCenter;
		x.alignment = TextAlignment.Center;
		x.font = (Font)Resources.Load( "" );
		x.font = Resources.Load ( "PressStart2P" ) as Font;
		x.font.material = Resources.Load ("PressStart2Pm") as Material;
		TextList [last].renderer.material = Resources.Load ("PressStart2Pm") as Material;
		TextList[last].renderer.enabled = true;
		last ++;
	}

	public static void Resize ( float radius ) {
		Radius = radius;
		MenuObject.transform.localScale = new Vector3 ( radius * 2f + MistakeRadius, radius * 2f + MistakeRadius, radius * 2f + MistakeRadius );
	}

	void OnMouseDown () {
		switch (GameController.mode) {
			case 0:
				AudioController.StartMenuMusic ();
				GameController.mode = 1;
				i = 0;
				break;
			case 1:
				GameController.PlayMode = i;
				i = 0;
			GameController.mode = 2;
				break;
			case 2:
				GameController.Difficulty = i + 1;
				i = 0;
				GameController.StartGame ();
				break;
			case 3:
				if (GameController.PlayMode == 1)
					PlayerController.Player.GetComponent<PlayerMovement> ().dirrection *= -1;
				i = 0;
				break;
			case 4:
				if (i == 0)
					GameController.StartGame ();
				else
					GameController.mode = 0;
				i = 0;
				break;
		}
	}

	void GameRender () {
		Resize (MovementController.InitialRadius * 2 / 3);
		Alloc (2);
		CreateText ( "SCORE", 15, 0.7f);
		CreateText ( (int)GameController.score, 45, 0f);
	}

	void StartRender () {
		Resize (MovementController.InitialRadius * 2 / 3);
		Alloc (3);
		CreateText ( "ATOMIC+", 19, 0.2f);
		CreateText ( "HIGHSCORE", 12, -0.6f );
		CreateText ( PlayerPrefs.GetInt ("Highscore"), 15, -0.9f );
	}

	void DifficultySelectRender () {
		Resize (MovementController.InitialRadius + MovementController.InterpolationRadius);
		Alloc (3);
		CreateText ( "DIFFICULTY", 10, 0.4f);
		CreateText ( DifficultyMode[i % 2], 35, -0.2f);
	}

	void GameplaySelectRender  () {
		Resize (MovementController.InitialRadius +  MovementController.InterpolationRadius);
		if ( i % 2 == 0 )
			Alloc (3);
		else
			Alloc (4);

		CreateText ( "GAMEPLAY MODE", 15, 0.7f);
		CreateText ( GameplayMode[i % 2], 35, 0f);
		CreateText ( GameplayHelp[0], 15, -0.6f);
		if ( i % 2 > 0 )
			CreateText ( GameplayHelp[1], 9, -0.9f);
	}

	void GameOverRender () {
		Resize (MovementController.InitialRadius + MovementController.InterpolationRadius);
		Alloc (4);
		CreateText ( "GAMEOVER", 30, 1f );
		CreateText ( "SCORE", 15, 0.5f);
		CreateText ( (int)GameController.score, 40, -0.2f);
		CreateText ( GameOverList[i % 2], 20, -0.9f);
	}

	// Use this for initialization
	void Awake () {
		MenuObject = gameObject;
		GameplayMode = new string[2];
		GameplayMode[0] = "RADIAL";
		GameplayMode[1] = "MANUAL";

		GameplayHelp = new string[2];
		GameplayHelp[0] = "SPACEBAR = RADIUS";
		GameplayHelp[1] = "CLICK IN THE MIDDLE = DIRECTION";

		DifficultyMode = new string[2];
		DifficultyMode[0] = "NORMAL";
		DifficultyMode[1] = "HARDCORE";

		GameOverList = new string[2];
		GameOverList[0] = "PLAY AGAIN";
		GameOverList[1] = "MAIN MENU";
	}

	public static void RightI () {
		i++;
		if ( i > 1 )
			i = 1;
	}

	public static void LeftI () {
		i--;
		if ( i < 0 )
			i = 0;
	}

	// Update is called once per frame
	void Update () {
		MenuObject = gameObject;
		switch (GameController.mode) {
		case 0:
			StartRender ();
			break;
		case 1:
			GameplaySelectRender ();
			break;
		case 2:
			DifficultySelectRender ();
			break;
		case 3:
			GameRender ();
			break;
		case 4:
			GameOverRender ();
			break;
		}
	}
}

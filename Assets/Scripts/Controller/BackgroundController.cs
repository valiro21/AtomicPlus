using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour {

	static GameObject Background;
	static int current, i;

	public static void ChangeBackground () {
		i = current;
		while ( i == current )
			i = Random.Range ( 1, 6 );

		current = i;
		Background.renderer.material.mainTexture = Resources.Load ( "Background/Back" + current.ToString() ) as Texture;
	}

	void Awake () {
		Background = gameObject;
		current = Random.Range (1, 6);
		Background.renderer.material.mainTexture = Resources.Load ( "Background/Back" + current.ToString() ) as Texture;
	}
}

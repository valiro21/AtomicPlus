using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
	
	public static AudioClip DamageSound, GetPointSound, MenuMusic, GameMusic;
	
	public static void Damage () {
		AudioPool.PlayAudioClip ( DamageSound );
	}
	
	public static void GetPoint () {
		AudioPool.PlayAudioClip (GetPointSound);
	}
	
	public static void StartMenuMusic () {
		AudioPool.SetBackgroundMusic (MenuMusic);
	}
	
	public static void StartGameMusic () {
		AudioPool.SetBackgroundMusic (GameMusic);
	}
	
	
	
	void Awake () {
		GameObject ValuesGod = GameController.ValuesGod;
		
		DamageSound = ValuesGod.GetComponent<AudioValues> ().DamageSound;
		GetPointSound = ValuesGod.GetComponent<AudioValues> ().GetPointSound;
		MenuMusic = ValuesGod.GetComponent<AudioValues> ().MenuMusic;
		GameMusic = ValuesGod.GetComponent<AudioValues> ().GameMusic;
	}
	
	void Update () {
	}
}
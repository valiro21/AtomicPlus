using UnityEngine;
using System.Collections;

public class AudioPool : MonoBehaviour {
	
	static AudioSource BackgroundMusic, SingleSound;
	
	public static void PlayAudioClip ( AudioClip Sound ) {
		SingleSound.PlayOneShot ( Sound );
	}
	
	public static void SetBackgroundMusic ( AudioClip Sound ) {
		BackgroundMusic.Stop ( );
		BackgroundMusic.clip = Sound;
		BackgroundMusic.loop = true;
		BackgroundMusic.Play ( );
	}
	
	void Awake () {
		BackgroundMusic = gameObject.AddComponent<AudioSource>(); 
		SingleSound = gameObject.AddComponent<AudioSource>();
		
		SingleSound.volume = 1;
		BackgroundMusic.volume = 1;
	}
}

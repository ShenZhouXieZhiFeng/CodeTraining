using UnityEngine;
using System.Collections;

namespace TurnTheGameOn.RacingGameTemplate{
	public class RG_AudioManager : MonoBehaviour {

		private GameObject emptyObject;
		private GameObject audioContainer;
		private AudioSource raceMusicAudioSource;

		void Start(){
			if (RGT_PlayerPrefs.GetAudio() == "ON") {
				AudioListener.pause = false;
			} else {
				AudioListener.pause = true;
			}
			audioContainer = new GameObject ();
			audioContainer.name = "Audio Container";
			AudioMusic ();
		}

		void Update(){
			if (RGT_PlayerPrefs.audioData.music.Length > 0) {
				if (!raceMusicAudioSource.isPlaying)
					PlayNextAudioTrack ();
			}
		}

		void AudioMusic(){
			if (RGT_PlayerPrefs.audioData.music.Length > 0) {
				emptyObject = new GameObject ("Audio Clip: Music");
				emptyObject.transform.parent = audioContainer.transform;
				emptyObject.AddComponent<AudioSource> ();
				raceMusicAudioSource = emptyObject.GetComponent<AudioSource> ();
				RGT_PlayerPrefs.audioData.currentAudioTrack = 0;
				raceMusicAudioSource.clip = RGT_PlayerPrefs.audioData.music [RGT_PlayerPrefs.audioData.currentAudioTrack];
				raceMusicAudioSource.loop = false;
				raceMusicAudioSource.Play ();
			}
		}

		void PlayNextAudioTrack(){
			if (RGT_PlayerPrefs.audioData.musicSelection == AudioData.MusicSelection.ListOrder) {
				if (RGT_PlayerPrefs.audioData.currentAudioTrack < RGT_PlayerPrefs.audioData.music.Length - 1) {
					RGT_PlayerPrefs.audioData.currentAudioTrack += 1;
				} else {
					RGT_PlayerPrefs.audioData.currentAudioTrack = 0;
				}
			}else if(RGT_PlayerPrefs.audioData.musicSelection == AudioData.MusicSelection.Random){
				RGT_PlayerPrefs.audioData.currentAudioTrack = UnityEngine.Random.Range ( 0, RGT_PlayerPrefs.audioData.music.Length );
			}
			raceMusicAudioSource.clip = RGT_PlayerPrefs.audioData.music [RGT_PlayerPrefs.audioData.currentAudioTrack];
			raceMusicAudioSource.Play ();
		}
		
	}
}
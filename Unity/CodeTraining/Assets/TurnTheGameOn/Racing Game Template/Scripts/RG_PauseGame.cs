using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace TurnTheGameOn.RacingGameTemplate{
	public class RG_PauseGame : MonoBehaviour {

		public EventTrigger.Entry pauseEvent;
		public string pauseButtonName = "Pause Button";
		public GameObject pauseWindow;
		private bool gamePaused;
		private bool configuredButton;
		GameObject pauseButton;

		void Update(){
			if (RGT_PlayerPrefs.inputData.useMobileController) {
				if (!configuredButton) {
					pauseButton = GameObject.Find (pauseButtonName);
					if (pauseButton != null) {
						EventTrigger findEvent = pauseButton.GetComponent<EventTrigger> ();
						if (findEvent != null) {
							findEvent.triggers.Add (pauseEvent);
							findEvent = null;
						}
						configuredButton = true;
						enabled = false;
					}
				}
			}
			else if (Input.GetKeyDown (RGT_PlayerPrefs.inputData.pauseKey) || Input.GetKeyDown (RGT_PlayerPrefs.inputData.pauseJoystick)) {
				PauseButton ();
			}
		}

		[ContextMenu("Pause Button")]
		public void PauseButton(){	
			if(gamePaused){					
				pauseWindow.SetActive (false);
				gamePaused = false;
				Time.timeScale = 1.0f;
				AudioSource[] allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
				for(int i=0; i < allAudioSources.Length; i++){
					if(allAudioSources[i].enabled){
						allAudioSources[i].Play();
					}
				}			
			}else{			
				gamePaused = true;
				Time.timeScale = 0.0f;
				AudioSource[] allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
				for(int i=0; i < allAudioSources.Length; i++){			allAudioSources[i].Pause();			}
				pauseWindow.SetActive (true);				
			}
		}
		
	}
}
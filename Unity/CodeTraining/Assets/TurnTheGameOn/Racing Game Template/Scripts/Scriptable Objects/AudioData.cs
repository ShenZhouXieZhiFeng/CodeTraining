using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioData : ScriptableObject {

	public enum MusicSelection { ListOrder, Random }
	//Set music tracks to be played in random or list order.
	public MusicSelection musicSelection;
	//Available music tracks.
	public AudioClip[] music;
	//Menu select sound clip.
	public AudioClip menuSelect;
	//Menu back sound clip.
	public AudioClip menuBack;
	//Current audio track.
	public int currentAudioTrack;
//Total number of audio tracks.
	public int numberOfTracks;
}
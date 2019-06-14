using UnityEngine;
using System.Collections;

public class DebugData : ScriptableObject {

	//Toggle Show RGT Launch window when Unity starts.
	public bool launchWindow = false;
	//Tutorial URL
	public string tutorialURL;
	//Documentation URL
	public string documentationURL;
	//Support URL
	public string supportURL;
	//Version Number
	public string projectVersion;
	//Toggle FPS counter on or off.
	public bool fpsCounter = false;

}
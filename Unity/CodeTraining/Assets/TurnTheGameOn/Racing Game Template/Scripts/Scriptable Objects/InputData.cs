using UnityEngine;
using System.Collections;

public class InputData : ScriptableObject {

	public enum Joystick{ None, JoystickButton0, JoystickButton1, JoystickButton2, JoystickButton3, JoystickButton4,
		JoystickButton5, JoystickButton6, JoystickButton7, JoystickButton8, JoystickButton9, JoystickButton10,
		JoystickButton11, JoystickButton12, JoystickButton13, JoystickButton14, JoystickButton15, JoystickButton16,
		JoystickButton17, JoystickButton18, JoystickButton19 }

	//Toggle mobile controls on or off.
	public bool useMobileController;
	//Mobile Controller prefab reference
	public GameObject mobileController;
	//KeyCode used to pause the game.
	public KeyCode pauseKey;
	//KeyCode used for camera switch key.
	public KeyCode cameraSwitchKey;
	//KeyCode used for nitro key.
	public KeyCode nitroKey;
	//KeyCode used for shift up key.
	public KeyCode shiftUp;
	//KeyCode used for shift down key.
	public KeyCode shiftDown;
	//KeyCode used for look back key.
	public KeyCode lookBack;
	//KeyCode used for pause joystick button.
	public KeyCode pauseJoystick;
	[HideInInspector] public Joystick _pauseJoystick;
	//KeyCode used for camera switch joystick button.
	public KeyCode cameraSwitchJoystick;
	[HideInInspector] public Joystick _cameraSwitchJoystick;
	//KeyCode used for nitro joystick button.
	public KeyCode nitroJoystick;
	[HideInInspector] public Joystick _nitroJoystick;
	//KeyCode used for shift up joystick button.
	public KeyCode shiftUpJoystick;
	[HideInInspector] public Joystick _shiftUpJoystick;
	//KeyCode used for shift down joystick button.
	public KeyCode shiftDownJoystick;
	[HideInInspector] public Joystick _shiftDownJoystick;
	//KeyCode used for look back joystick button.
	public KeyCode lookBackJoystick;
	[HideInInspector] public Joystick _lookBackJoystick;
}

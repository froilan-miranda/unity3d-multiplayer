using UnityEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using viJoystickLib;

public class XInputTest : MonoBehaviour
{		
	JoystickManager jm;

	void Update ()
	{
		for (int i = 0; i < jm.Joysticks.Count; i++) {
			Joystick j = jm.Joysticks[i];
			Debug.Log("Joystick: " + j.Serial + " X: " + j.X.ToString() + " Y: " + j.Y.ToString() + " Thumb: " + j.Thumb.ToString());	
		}
	}

	void Start ()
	{
		jm = new JoystickManager();
	}
	
	void OnApplicationQuit ()
	{
	
	}
}

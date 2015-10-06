using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class CmdLineInput : MonoBehaviour {

	public static string[] args; 

	void Awake(){
		args = Environment.GetCommandLineArgs ();

	}
}

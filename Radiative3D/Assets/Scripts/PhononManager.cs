using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class PhononManager : MonoBehaviour {

	public GameObject phononPrefab;
	private List<Phonon> phononData = new List<Phonon>();
	private int playSpeedMultiplier;
	private bool simStarted = false;
	private bool simPaused = false;
	private char[] splitAt = {' '};
	private InputField reportsInput;
	private Slider multiSlider;


	void Start(){
		reportsInput = GameObject.FindGameObjectWithTag ("reportsInput").GetComponent<InputField>();
		PopulatePhononData(File.ReadAllLines("nopinch.txt"));
		multiSlider = GameObject.FindGameObjectWithTag ("multiSlider").GetComponent<Slider> ();
		playSpeedMultiplier = (int)multiSlider.value;
		
	}


	void Update(){
		if(playSpeedMultiplier != (int)multiSlider.value){
			playSpeedMultiplier = (int)multiSlider.value;
			UpdateSpeed(playSpeedMultiplier);
		}
	}


	void PopulatePhononData(string[] fileIn){
			for(int i=0; i < fileIn.Length/8; i++){
				 Populate(fileIn[i].Split(splitAt,System.StringSplitOptions.RemoveEmptyEntries));
			}
	}

	void Populate(string[] input){

			int phononID = int.Parse (input [0]);
			Vector3 loc = new Vector3 ((-1)*float.Parse (input [3]), float.Parse (input [5]), float.Parse (input [4]));
			
			if (phononData.Count > 0 && phononID == phononData [phononData.Count - 1].GetID ()) {
				phononData [phononData.Count - 1].PopulateList (char.Parse (input [1]), float.Parse (input [2]), loc);
			}
			else {
				GameObject phonon = GameObject.Instantiate(phononPrefab);
				phononData.Add(phonon.GetComponent<Phonon>());
				phononData[phononData.Count-1].Initialize(phononID, char.Parse(input [1]), float.Parse (input [2]), loc);
				
			}

	}

	public void UpdateSpeed(int multi){
		playSpeedMultiplier = multi;
		foreach (Phonon p in phononData)
			p.UpdateSpeed (playSpeedMultiplier);
	}

	public void UserPopulate(){
		if (reportsInput.text != null) {
			phononData.Clear ();
			PopulatePhononData (File.ReadAllLines (reportsInput.text));
		}
	}

	public void Propagate(){
		if (!simStarted) {
			simStarted = true;
			foreach (Phonon p in phononData) {
				p.Propagate (playSpeedMultiplier);
			}
		} 
		else {
			foreach (Phonon p in phononData){
			p.Pause (simPaused);
			}
			simPaused = (simPaused)? false : true;

		}

	}

	public void Reset(){
		simStarted = false;
		simPaused = false;
		foreach (Phonon p in phononData) {
			p.Reset();
		}
	}
}

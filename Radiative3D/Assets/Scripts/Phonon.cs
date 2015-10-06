using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Phonon : MonoBehaviour {


	private List<ModeTimeLoc> timeLocList = new List<ModeTimeLoc>();
	private float simStartTime;
	private int phononID;
	private bool simStarted = false;
	private float currentTime;
	private float showTime = 1.0f;
	private float startShowTime;
	private float pauseStartTime = 0.0f;
	private float pauseTotalTime = 0.0f;
	private int playSpeedMultiplier;
	private int iterator = 0;
	private int maxIterator;
	private char currentMode = 'P';
	private Renderer rend;

	void Awake(){
		rend = gameObject.GetComponent<Renderer> ();
	}


	public void Initialize(int ID,char mode,float time, Vector3 loc){
		phononID = ID;
		timeLocList.Add (new ModeTimeLoc (mode, time, loc));
		transform.position = loc;
	}

	class ModeTimeLoc{
		private char mMode;
		private float mTime;
		private Vector3 mLoc;

		public ModeTimeLoc(char mode,float time, Vector3 loc){
			mMode = mode;
			mTime = time;
			mLoc = loc;
		}

		public float GetTime(){
			return mTime;
		}

		public Vector3 GetLoc(){
			return mLoc;
		}

		public char GetMode(){
			return mMode;
		}

	}

	public void PopulateList(char mode,float time, Vector3 loc){
		timeLocList.Add(new ModeTimeLoc(mode,time,loc));

	}

	public int GetID(){
		return phononID;
	}

	public void UpdateSpeed(int multi){
		playSpeedMultiplier = multi;
	}

	public void Reset(){
		if (!gameObject.activeInHierarchy)
			gameObject.SetActive (true);
		if (rend.enabled)
			rend.enabled = false;
		iterator = 0;
		transform.position = timeLocList [0].GetLoc ();
		simStarted = false;
		pauseStartTime = 0.0f;
		pauseTotalTime = 0.0f;

	}

	public void Propagate(int multi){
		simStartTime = Time.time;
		simStarted = true;
		maxIterator = (timeLocList.Count - 1);
		playSpeedMultiplier = multi;

	}

	public void Pause(bool b){
		simStarted = b;
		if (!b)
			pauseStartTime = Time.time;
		else {
			pauseTotalTime += (Time.time - pauseStartTime);
		}

	}

	void ChangeMode(){
		switch (currentMode) {
		case 'P':
			currentMode = 'S';
			rend.material.color = Color.blue;
			break;
		case 'S':
			currentMode = 'P';
			rend.material.color = Color.red;
			break;
		}
	}

	void Update(){

		if (simStarted) {
			if(iterator > maxIterator){
				simStarted = false;
				transform.gameObject.SetActive(false);
				return;
			}
			currentTime = Time.time - simStartTime - pauseTotalTime;

			if((currentTime - startShowTime) >= showTime)
				rend.enabled = false;


			if((currentTime*playSpeedMultiplier) >= timeLocList[iterator].GetTime()){
				transform.position = timeLocList[iterator].GetLoc();
				rend.enabled = true;
				startShowTime = currentTime;

				if(timeLocList[iterator].GetMode() != currentMode)
					ChangeMode();

				iterator++;

			}
		}


	}

}

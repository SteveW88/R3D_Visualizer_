using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserInput : MonoBehaviour {

	private Text inputField;
	public InputField gridIndexOut;
	private GameObject button;
	private GameObject button2;
	private Vector3 nodeIndex;
	//private int numBlocks;

	void Awake(){
		button = GameObject.FindGameObjectWithTag ("button");
		button2 = GameObject.FindGameObjectWithTag ("button2");
		button2.SetActive (false);
		inputField = GameObject.FindGameObjectWithTag ("inputField").GetComponent<Text> ();
		gridIndexOut = GameObject.FindGameObjectWithTag ("gridIndexOut").GetComponent<InputField> ();
		gridIndexOut.gameObject.SetActive (false);
	}

	public void ButtonClicked(){
		if (inputField.text.Length != 5)
			return;

		nodeIndex.x = int.Parse(inputField.text.Substring (0, 1));
		nodeIndex.y = int.Parse (inputField.text.Substring (2, 1));
		nodeIndex.z = int.Parse(inputField.text.Substring (4, 1));


		//numBlocks = (int)(nodeIndex.x * nodeIndex.y * nodeIndex.z);

		SetUpNodeInput ();

	}

	void SetUpNodeInput(){
		gridIndexOut.gameObject.SetActive (true);
		inputField.transform.parent.gameObject.SetActive (false);
		button.SetActive (false);
		button2.SetActive (true);

		button.GetComponent<RectTransform>().localPosition = new Vector3 (300, -150, 0);

		gridIndexOut.text = "(0,0,0)\n(0,0,1)\n(0,1,0)\n(1,0,0)";
	}
}

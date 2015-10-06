using UnityEngine;
using System.Collections;

public class GridNode : MonoBehaviour {

	private Vector3 position;
	private Vector3 nodeIndex;

	void Awake(){
		position = transform.position;
	}

	public void SetPos(Vector3 pos){
		position = pos;
		transform.position = position;
	}

	public void SetIndex(Vector3 index){
		nodeIndex = index;
	}

	public Vector3 GetPos(){
		return position;
	}

	public Vector3 GetIndex(){
		return nodeIndex;
	}

	public void Initialize(Vector3 index, Vector3 pos){
		SetPos (pos);
		SetIndex (index);
	}
}

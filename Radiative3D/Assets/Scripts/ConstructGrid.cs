using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;


public class ConstructGrid : MonoBehaviour {

	public GameObject tetraPrefab;
	public GameObject nodePrefab;
	public Transform Grid;
	private Vector3 gridIndexing;
	private char[] splitAt = {' '};
	private List<GridNode> temp = new List<GridNode>();
	private List<Renderer> tetraRendList = new List<Renderer> ();
	private List<GridNode> nodeScripts2 = new List<GridNode>(); 
	private InputField gridInput;


	void Start () {
		gridInput = GameObject.FindGameObjectWithTag ("gridInput").GetComponent<InputField>();	
		GridConstruct (File.ReadAllLines("nopinchgrid.txt"));


	}


	public void UserGrid(){
		if (gridInput.text != null) {
			ClearGrid ();
			GridConstruct (File.ReadAllLines (gridInput.text));
		}
	}

	void ClearGrid(){
		foreach (GridNode gn in nodeScripts2) {
			Destroy(gn.gameObject);
		}
		foreach (Renderer rn in tetraRendList) {
			Destroy(rn.gameObject);
		}
		nodeScripts2.Clear ();
		tetraRendList.Clear ();

	}

	GridNode RelNode(int i, int j, int k, int x, int y, int z){
		int index = (int)((x+i) + ((gridIndexing.x + 1) * (y+j)) + ((gridIndexing.y + 1) * (gridIndexing.x + 1) * (z+k))); 
		return nodeScripts2 [index];
	}


	void NodeConstruct(string[] input){
		Vector3 nodeIndex = new Vector3(int.Parse(input[0]),int.Parse(input[1]),int.Parse(input[2]));

		if (nodeScripts2.Count > 0 && nodeIndex == nodeScripts2 [nodeScripts2.Count - 1].GetIndex ())
			return;

		Vector3 nodePos = new Vector3((-1)*float.Parse(input[3]),float.Parse(input[5]),float.Parse(input[4]));

		GameObject node = GameObject.Instantiate (nodePrefab);
		nodeScripts2.Add (node.GetComponent<GridNode> ());
		nodeScripts2[nodeScripts2.Count - 1].Initialize(nodeIndex,nodePos);
		node.transform.SetParent (Grid);


                                         
	}


	void BlockConstruct(List<GridNode> nodes){
		//6,5,0,3
		//1,3,5,0
		//2,0,6,3
		//7,5,3,6
		//4,6,0,5
		TetraConstruct (nodes [6], nodes [5], nodes [0], nodes [3]);
		TetraConstruct (nodes [1], nodes [3], nodes [5], nodes [0]);
		TetraConstruct (nodes [2], nodes [0], nodes [6], nodes [3]);
		TetraConstruct (nodes [7], nodes [5], nodes [3], nodes [6]);
		TetraConstruct (nodes [4], nodes [6], nodes [0], nodes [5]);

	}


	void TetraConstruct(GridNode n0, GridNode n1, GridNode n2, GridNode n3){
		GameObject tetra = GameObject.Instantiate (tetraPrefab);
		tetra.GetComponent<Tetrahedron> ().Initialize (n0.GetPos (), n1.GetPos (), n2.GetPos (), n3.GetPos ());
		tetra.transform.SetParent(Grid);
		tetraRendList.Add (tetra.GetComponent<Renderer>());

	}

	void GridConstruct(string[] fileIn){

		for (int i=0; i<fileIn.Length; i++) {
			NodeConstruct (fileIn[i].Split(splitAt,System.StringSplitOptions.RemoveEmptyEntries));
		}
		
		
		gridIndexing = nodeScripts2[nodeScripts2.Count - 1].GetIndex ();
		
		
		for (int i = 0; i < gridIndexing.x; i++) {
			for (int j = 0; j < gridIndexing.y; j++) {
				for (int k = 0; k < gridIndexing.z; k++) {
					// Build out node octuple
					temp.Clear();
					temp.Add (RelNode(i,j,k,0,0,0));
					temp.Add (RelNode(i,j,k,0,0,1));
					temp.Add (RelNode(i,j,k,0,1,0));
					temp.Add (RelNode(i,j,k,0,1,1));
					temp.Add (RelNode(i,j,k,1,0,0));
					temp.Add (RelNode(i,j,k,1,0,1));
					temp.Add (RelNode(i,j,k,1,1,0));
					temp.Add (RelNode(i,j,k,1,1,1));
					// Build block
					// bool mirror = (((i+j+k)%2)==1);   // Reflection tracking
					// bool issurface = (k==0);          // Surface is top of stacks
					BlockConstruct(temp);
					
					
				}
			}
		}// End triple for Loop
		
		

	}

	public void ShowTetra(bool show){
		foreach (Renderer r in tetraRendList) {
			r.enabled = show;
		}
	}

	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Marrow : MonoBehaviour {
	[SerializeField]
	private List<MarrowNode> nodes;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// NodeInputs get an Update call, use this
		foreach(MarrowNode node in nodes) {
			if (node is IActiveNode) {
				((IActiveNode) node).Update();
			}
		}
	}
	
	// Return the list of Marrow nodes
	public List<MarrowNode> GetNodes() {
		if (nodes == null) {
			nodes = new List<MarrowNode>();
		}
		return nodes;
	}
	
	void OnMouseUpAsButton () {
		foreach (MarrowNode node in nodes) {
			if (node is InputNode) {
				((InputNode) node).OnInteract();
			}
		}
	}
}
